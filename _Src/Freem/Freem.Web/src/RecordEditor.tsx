import {forwardRef, useImperativeHandle, useState} from "react";
import {DateTimePeriod} from "./data/DateTimePeriod.ts";
import {ActivityEntity} from "./entities/ActivityEntity.ts";
import {TagEntity} from "./entities/TagEntity.ts";
import {RecordEntity} from "./entities/RecordEntity.ts";
import {InputText} from "./components/inputs/InputText.tsx";
import {InputTextarea} from "primereact/inputtextarea";
import {DatePicker} from "./components/inputs/DatePicker.tsx";
import {ActivitiesPicker} from "./components/inputs/ActivitiesPicker.tsx";
import {TagsPicker} from "./components/inputs/TagsPicker.tsx";

export interface IRecordEditorHandle {
  validate: () => boolean;
  data: () => IRecordEditorData;
}

export interface IRecordEditorProps {
  loading?: boolean;
  record?: RecordEntity;
}

export interface IRecordEditorData {
  name?: string;
  description?: string;
  period: DateTimePeriod;
  activityIds: string[];
  tagIds?: string[];
}

interface IRecordEditorState {
  name: string;
  description: string;
  startAt?: Date;
  endAt?: Date;
  activities: ActivityEntity[];
  tags: TagEntity[];
}

export interface IRecordEditorValidationResult {
  startAtError?: string;
  endAtError?: string;
  activitiesError?: string;
}

export const RecordEditor = forwardRef<IRecordEditorHandle, IRecordEditorProps>((props, ref) => {
  const {loading, record} = props;

  const [state, setState] = useState<IRecordEditorState>({
    name: record?.name || "",
    description: record?.description || "",
    startAt: record?.period.startAt,
    endAt: record?.period.endAt,
    activities: record?.activities || [],
    tags: record?.tags || []
  });

  const [validationResult, setValidationResult] = useState<IRecordEditorValidationResult>({});

  useImperativeHandle(ref, () => ({
    validate() {
      const newValidationResult = {...validationResult};

      if (state.activities.length === 0) {
        newValidationResult.activitiesError = "Activities are required";
      }

      if (state.startAt === undefined || state.startAt === null) {
        newValidationResult.startAtError = "Start at is required";
      }

      if (state.endAt === undefined || state.endAt === null) {
        newValidationResult.endAtError = "End at is required";
      }

      setValidationResult(newValidationResult);
      return newValidationResult.activitiesError === undefined
          && newValidationResult.startAtError === undefined
          && newValidationResult.endAtError === undefined;
    },
    data() {
      if (state.startAt === undefined || state.startAt === null ||
          state.endAt === undefined || state.endAt === null) {
        throw new Error();
      }

      const period = new DateTimePeriod(state.startAt, state.endAt);
      const activityIds = state.activities.map(activity => activity.id);
      const tagIds = state.tags.map(tag => tag.id);

      return {
        name: state.name !== undefined ? state.name : undefined,
        description: state.description != undefined ? state.description : undefined,
        period: period,
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
    setValidationResult({});
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
            onChange={e => change(e.target.name, e.target.value)}/>
        <div style={{display: "flex", flexDirection: "row"}}>
          <DatePicker
              style={{width: "50%", marginRight: "3px"}}
              placeholder="Start at"
              showTime
              value={state.startAt}
              maxValue={state.endAt}
              error={validationResult.startAtError}
              onChange={value => change("startAt", value)}/>
          <DatePicker
              style={{width: "50%"}}
              placeholder="End at"
              showTime
              value={state.endAt}
              minValue={state.startAt}
              error={validationResult.endAtError}
              onChange={value => change("endAt", value)}/>
        </div>
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