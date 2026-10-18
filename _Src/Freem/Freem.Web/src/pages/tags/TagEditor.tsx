import {TagEntity} from "./entities/TagEntity.ts";
import {ChangeEvent, forwardRef, useImperativeHandle, useState} from "react";
import {InputText} from "./components/inputs/InputText.tsx";

export interface ITagEditorHandle {
  validate: () => boolean;
  data: () => ITagEditorData;
}

export interface ITagEditorProps {
  loading?: boolean;

  tag?: TagEntity;
}

export interface ITagEditorData {
  name: string;
}

interface ITagEditorValidationResult {
  nameError?: string;
}

export const TagEditor = forwardRef<ITagEditorHandle, ITagEditorProps>((props, ref) => {
  const {loading, tag} = props;

  const [data, setData] = useState<ITagEditorData>({
    name: tag?.name || ""
  });

  const [validationResult, setValidationResult] = useState<ITagEditorValidationResult>({});

  useImperativeHandle(ref, () => ({
    validate() {
      const newValidationResult = {...validationResult};

      if (data.name === "") {
        newValidationResult.nameError = "Name is required";
      }

      setValidationResult(newValidationResult);

      return newValidationResult.nameError === undefined;
    },
    data() {
      return data;
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

  return (
      <InputText
          name="name"
          size="medium"
          placeholder="Name"
          loading={loading}
          value={data.name}
          error={validationResult.nameError}
          onChange={change}/>
  );
});