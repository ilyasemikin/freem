import {RunningRecordEntity} from "./entities/RunningRecordEntity.ts";
import {IRunningRecordEditorData, IRunningRecordEditorHandle, RunningRecordEditor} from "./RunningRecordEditor.tsx";
import {useRef, useState} from "react";
import {DialogFooter} from "./components/DialogFooter.tsx";
import {CustomButton} from "./components/inputs/CustomButton.tsx";

export interface IRunningRecordUpdateModalProps {
  record: RunningRecordEntity;

  onUpdate?: (data: IRunningRecordEditorData) => void;
  onDelete?: () => void;
  onCancel?: () => void;
}

export function RunningRecordUpdateModal(props: IRunningRecordUpdateModalProps) {
  const {record, onUpdate, onDelete, onCancel} = props;

  const [loading, setLoading] = useState(false);

  const editorRef = useRef<IRunningRecordEditorHandle>(null);

  function remove() {
    if (onDelete !== undefined) {
      onDelete();
    }
  }

  function update() {
    if (editorRef.current === null) {
      return;
    }

    const validationResult = editorRef.current.validate();
    if (!validationResult) {
      return;
    }

    const data = editorRef.current.data();
    if (onUpdate !== undefined) {
      setLoading(true);
      onUpdate(data);
      setLoading(false);
    }
  }

  return (
      <>
        <div style={{display: "flex", justifyContent: "end", marginBottom: "5px"}}>
          <CustomButton onClick={remove}>
            <span style={{fontSize: "20px"}} className="pi pi-trash"/>
          </CustomButton>
        </div>
        <RunningRecordEditor
            ref={editorRef}
            loading={loading}
            record={record}/>
        <DialogFooter
            loading={loading}
            actionText="Save"
            onAction={update}
            onCancel={onCancel}/>
      </>
  )
}