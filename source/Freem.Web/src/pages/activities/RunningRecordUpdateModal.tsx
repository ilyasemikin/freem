import {RunningRecordEntity} from "../../entities/RunningRecordEntity.ts";
import {ErrorHelper} from "../../helpers/ErrorHelper.ts";
import {ValidationError} from "../../components/ValidationError.tsx";
import {IRunningRecordEditorData, IRunningRecordEditorHandle, RunningRecordEditor} from "./RunningRecordEditor.tsx";
import {useRef, useState} from "react";
import {DialogFooter} from "../../dialog/DialogFooter.tsx";
import {CustomButton} from "../../components/inputs/CustomButton.tsx";

export interface IRunningRecordUpdateModalProps {
  record: RunningRecordEntity;

  onUpdate?: (data: IRunningRecordEditorData) => Promise<void>;
  onDelete?: () => Promise<void>;
  onCancel?: () => void;
}

export function RunningRecordUpdateModal(props: IRunningRecordUpdateModalProps) {
  const {record, onUpdate, onDelete, onCancel} = props;

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | undefined>();

  const editorRef = useRef<IRunningRecordEditorHandle>(null);

  async function remove() {
    if (onDelete === undefined || !window.confirm("Delete the running record? The tracked time will be lost.")) {
      return;
    }

    setLoading(true);
    try {
      await onDelete();
    } catch (error) {
      setError(ErrorHelper.getMessage(error, "Failed to delete the running record"));
    } finally {
      setLoading(false);
    }
  }

  async function update() {
    if (editorRef.current === null) {
      return;
    }

    const validationResult = editorRef.current.validate();
    if (!validationResult) {
      return;
    }

    const data = editorRef.current.data();
    if (onUpdate === undefined) {
      return;
    }

    setLoading(true);
    try {
      await onUpdate(data);
    } catch (error) {
      setError(ErrorHelper.getMessage(error, "Failed to save the running record"));
    } finally {
      setLoading(false);
    }
  }

  return (
      <>
        <div style={{display: "flex", justifyContent: "end", marginBottom: "5px"}}>
          <CustomButton label="Delete running record" onClick={remove}>
            <span style={{fontSize: "20px"}} className="pi pi-trash"/>
          </CustomButton>
        </div>
        <RunningRecordEditor
            ref={editorRef}
            loading={loading}
            record={record}/>
        <ValidationError error={error}/>
        <DialogFooter
            loading={loading}
            actionText="Save"
            onAction={update}
            onCancel={onCancel}/>
      </>
  )
}