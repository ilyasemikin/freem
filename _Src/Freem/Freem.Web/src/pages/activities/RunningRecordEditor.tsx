import {RunningRecordEntity} from "./entities/RunningRecordEntity.ts";
import {ChangeEvent, forwardRef, useImperativeHandle, useState} from "react";
import {InputText} from "./components/inputs/InputText.tsx";
import {InputTextarea} from "primereact/inputtextarea";
import {DatePicker} from "./components/inputs/DatePicker.tsx";
import {ActivitiesPicker} from "./components/inputs/ActivitiesPicker.tsx";
import {ActivityEntity} from "./entities/ActivityEntity.ts";
import {TagEntity} from "./entities/TagEntity.ts";
import {TagsPicker} from "./components/inputs/TagsPicker.tsx";

export interface IRunningRecordEditorHandle {
  validate: () => boolean;
  data: () => IRunningRecordEditorData;
}

export interface IRunningRecordEditorProps {
  loading?: boolean;
  record?: RunningRecordEntity;
}

export interface IRunningRecordEditorData {
  name: string;
  description: string;
  startAt: Date;
  activityIds: string[];
  tagIds: string[];
}

interface IRunningRecordEditorState {
  name: string;
  description: string;
  startAt?: Date;
  activities: ActivityEntity[];
  tags: TagEntity[];
}

export interface IRunningRecordEditorValidationResult {
  startAtError?: string;
  activityIdsError?: string;
}

export const RunningRecordEditor = forwardRef<IRunningRecordEditorHandle, IRunningRecordEditorProps>((props, ref) => {
  const {loading, record} = props;

  const [data, setData] = useState<IRunningRecordEditorState>({
    name: record?.name || "",
    description: record?.description || "",
    startAt: record?.startAt,
    activities: record?.activities || [],
    tags: record?.tags || []
  });

  const [validationResult, setValidationResult] = useState<IRunningRecordEditorValidationResult>({});

  useImperativeHandle(ref, () => ({
    validate() {
      const newValidationResult = {...validationResult};

      if (data.activities.length === 0) {
        newValidationResult.activityIdsError = "Activities are required"
      }

      const now = new Date();
      if (data.startAt === undefined || data.startAt === null) {
        newValidationResult.startAtError = "Start at is required";
      } else if (data.startAt > now) {
        newValidationResult.startAtError = "Start at must be less than now";
      }

      setValidationResult(newValidationResult);

      return newValidationResult.startAtError === undefined
          && newValidationResult.activityIdsError === undefined;
    },
    data() {
      if (data.startAt === null) {
        throw new Error();
      }

      const activityIds = data.activities.map(activity => activity.id);
      const tagIds = data.tags.map(tag => tag.id);
      return {
        name: data.name,
        description: data.description,
        startAt: data.startAt,
        activityIds: activityIds,
        tagIds: tagIds
      } as IRunningRecordEditorData;
    }
  }));

  function changeName(e: ChangeEvent<HTMLInputElement>) {
    const newData = {
      ...data,
      [e.target.name]: e.target.value
    };

    setData(newData);
    setValidationResult({});
  }

  function changeDescription(e: ChangeEvent<HTMLTextAreaElement>) {
    const newData = {
      ...data,
      [e.target.name]: e.target.value
    };

    setData(newData);
    setValidationResult({});
  }

  function changeStartAt(value?: Date) {
    const newData = {
      ...data,
      startAt: value
    };

    setData(newData);
    setValidationResult({});
  }

  function changeActivities(value: ActivityEntity[]) {
    const newData = {
      ...data,
      activities: value
    };

    setData(newData);
    setValidationResult({});
  }

  function changeTags(value: TagEntity[]) {
    const newData = {
      ...data,
      tags: value
    };

    setData(newData);
    setValidationResult({});
  }

  const now = new Date();

  return (
      <div style={{display: "flex", flexDirection: "column"}}>
        <InputText
            name="name"
            size="medium"
            placeholder="Name"
            loading={loading}
            value={data.name}
            onChange={changeName}/>
        <InputTextarea
            style={{marginBottom: "3px"}}
            rows={5}
            cols={25}
            autoResize
            placeholder="Description"
            onChange={changeDescription}/>
        <DatePicker
            placeholder="Start at"
            showTime
            value={data.startAt}
            error={validationResult.startAtError}
            maxValue={now}
            onChange={changeStartAt}/>
        <ActivitiesPicker
            loading={loading}
            activities={data.activities}
            error={validationResult.activityIdsError}
            onChange={changeActivities}/>
        <TagsPicker
            loading={loading}
            tags={data.tags}
            onChange={changeTags}/>
      </div>
  );
})