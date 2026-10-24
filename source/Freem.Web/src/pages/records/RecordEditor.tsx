import {forwardRef, useImperativeHandle, useState} from "react";
import {ActivityEntity} from "../../entities/ActivityEntity.ts";
import {TagEntity} from "../../entities/TagEntity.ts";
import {RecordEntity} from "../../entities/RecordEntity.ts";
import {InputText} from "../../components/inputs/InputText.tsx";
import {InputTextarea} from "primereact/inputtextarea";
import {ActivitiesPicker} from "../../components/inputs/ActivitiesPicker.tsx";
import {TagsPicker} from "../../components/inputs/TagsPicker.tsx";
import {CustomButton} from "../../components/inputs/CustomButton.tsx";
import {ValidationError} from "../../components/ValidationError.tsx";
import {IPeriodRowData, IPeriodRowState, RecordPeriodsHelper} from "../../helpers/RecordPeriodsHelper.ts";
import {RecordPeriodRow} from "./RecordPeriodRow.tsx";

export interface IRecordEditorHandle {
  validate: () => boolean;
  data: () => IRecordEditorData;
}

// Without a record the editor creates a new one and lets the user enter periods;
// periods of an existing record are edited separately (see RecordPeriodsPanel)
export interface IRecordEditorProps {
  loading?: boolean;
  record?: RecordEntity;
}

export interface IRecordEditorData {
  name?: string;
  description?: string;
  periods: IPeriodRowData[];
  activityIds: string[];
  tagIds?: string[];
}

interface IRecordEditorState {
  name: string;
  description: string;
  rows: IPeriodRowState[];
  activities: ActivityEntity[];
  tags: TagEntity[];
}

export interface IRecordEditorValidationResult {
  activitiesError?: string;
  periodsError?: string;
  rowErrors: Record<string, string>;
}

export const RecordEditor = forwardRef<IRecordEditorHandle, IRecordEditorProps>((props, ref) => {
  const {loading, record} = props;

  const periodsEditable = record === undefined;

  const [state, setState] = useState<IRecordEditorState>({
    name: record?.name || "",
    description: record?.description || "",
    rows: periodsEditable ? [RecordPeriodsHelper.createRow()] : [],
    activities: record?.activities || [],
    tags: record?.tags || []
  });

  const [validationResult, setValidationResult] = useState<IRecordEditorValidationResult>({rowErrors: {}});

  useImperativeHandle(ref, () => ({
    validate() {
      const newValidationResult: IRecordEditorValidationResult = {rowErrors: {}};

      if (state.activities.length === 0) {
        newValidationResult.activitiesError = "Activities are required";
      }

      if (periodsEditable) {
        if (state.rows.length === 0) {
          newValidationResult.periodsError = "At least one period is required";
        }

        newValidationResult.rowErrors = RecordPeriodsHelper.validateRows(state.rows);
      }

      setValidationResult(newValidationResult);
      return newValidationResult.activitiesError === undefined
          && newValidationResult.periodsError === undefined
          && Object.keys(newValidationResult.rowErrors).length === 0;
    },
    data() {
      const periods = state.rows.map(row => RecordPeriodsHelper.toRowData(row));
      const activityIds = state.activities.map(activity => activity.id);
      const tagIds = state.tags.map(tag => tag.id);

      return {
        name: state.name !== undefined ? state.name : undefined,
        description: state.description != undefined ? state.description : undefined,
        periods: periods,
        activityIds: activityIds,
        tagIds: tagIds
      };
    }
  }));

  function change<T>(name: string, value: T) {
    const newState = {
      ...state,
      [name]: value
    }

    setState(newState);
    setValidationResult({rowErrors: {}});
  }

  function changeRow(row: IPeriodRowState) {
    change("rows", state.rows.map(item => item.key === row.key ? row : item));
  }

  function addRow() {
    const last = state.rows[state.rows.length - 1];
    change("rows", [...state.rows, RecordPeriodsHelper.createRow(last?.endAt)]);
  }

  function removeRow(key: string) {
    change("rows", state.rows.filter(item => item.key !== key));
  }

  function renderRemoveAction(row: IPeriodRowState) {
    if (state.rows.length <= 1) {
      return undefined;
    }

    return (
        <CustomButton label="Remove period" onClick={() => removeRow(row.key)}>
          <span style={{fontSize: "18px"}} className="pi pi-trash"/>
        </CustomButton>
    );
  }

  return (
      <div style={{display: "flex", flexDirection: "column"}}>
        <InputText
            name="name"
            size="medium"
            placeholder="Name"
            loading={loading}
            value={state.name}
            onChange={e => change(e.target.name, e.target.value)}/>
        <InputTextarea
            style={{marginBottom: "3px"}}
            name="description"
            rows={5}
            cols={25}
            autoResize
            placeholder="Description"
            value={state.description}
            onChange={e => change(e.target.name, e.target.value)}/>
        {periodsEditable &&
            <>
              {state.rows.map(row =>
                  <RecordPeriodRow
                      key={row.key}
                      row={row}
                      loading={loading}
                      error={validationResult.rowErrors[row.key]}
                      actions={renderRemoveAction(row)}
                      onChange={changeRow}/>)}
              <ValidationError error={validationResult.periodsError}/>
              <CustomButton label="Add period" style={{marginBottom: "8px"}} onClick={addRow}>
                <span style={{marginRight: "5px"}} className="pi pi-plus"/>
                <span>Add period</span>
              </CustomButton>
            </>}
        <ActivitiesPicker
            loading={loading}
            activities={state.activities}
            error={validationResult.activitiesError}
            onChange={value => change("activities", value)}/>
        <TagsPicker
            loading={loading}
            tags={state.tags}
            onChange={value => change("tags", value)}/>
      </div>
  );
})