import {ChangeEvent, forwardRef, useImperativeHandle, useState} from "react";
import {ActivityEntity} from "./entities/ActivityEntity.ts";
import {InputText} from "./components/inputs/InputText.tsx";
import {TagsPicker} from "./components/inputs/TagsPicker.tsx";
import {TagEntity} from "./entities/TagEntity.ts";

export interface IActivityEditorHandle {
  validate: () => boolean;
  data: () => IActivityEditorData;
}

export interface IActivityEditorProps {
  loading?: boolean;

  activity?: ActivityEntity
}

export interface IActivityEditorData {
  name: string;
  tagIds: string[];
}

export interface IActivityEditorInternalData {
  name: string;
  tags: TagEntity[];
}

interface IActivityEditorValidationResult {
  nameError?: string;
}

export const ActivityEditor = forwardRef<IActivityEditorHandle, IActivityEditorProps>((props, ref) => {
  const {loading, activity} = props;

  const [data, setData] = useState<IActivityEditorInternalData>({
    name: activity?.name || "",
    tags: activity?.tags ?? [],
  });

  const [validationResult, setValidationResult] = useState<IActivityEditorValidationResult>({});

  useImperativeHandle(ref, () => ({
    validate() {
      const newValidationResult = {...validationResult};

      if (data.name === "") {
        newValidationResult.nameError = "Name is required"
      }

      setValidationResult(newValidationResult);

      return newValidationResult.nameError === undefined;
    },
    data() {
      const tagIds = data.tags.map(tag => tag.id);
      return {
        name: data.name,
        tagIds: tagIds
      };
    }
  }));

  function change(e: ChangeEvent<HTMLInputElement>) {
    const newData = {
      ...data,
      [e.target.name]: e.target.value
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

  return (
      <>
        <InputText
            name="name"
            size="medium"
            placeholder="Name"
            loading={loading}
            value={data.name}
            error={validationResult.nameError}
            onChange={change}/>
        <TagsPicker
            loading={loading}
            tags={data.tags}
            onChange={changeTags}/>
      </>
  );
})