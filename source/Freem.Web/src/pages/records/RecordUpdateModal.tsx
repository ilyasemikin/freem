import {RecordEntity} from "../../entities/RecordEntity.ts";
import {ErrorHelper} from "../../helpers/ErrorHelper.ts";
import {ValidationError} from "../../components/ValidationError.tsx";
import {IRecordEditorData, IRecordEditorHandle, RecordEditor} from "./RecordEditor.tsx";
import {useCallback, useRef, useState} from "react";
import {CustomButton} from "../../components/inputs/CustomButton.tsx";
import {DialogFooter} from "../../dialog/DialogFooter.tsx";
import {RecordPeriodsPanel} from "./RecordPeriodsPanel.tsx";

export interface IRecordUpdateModalProps {
  record: RecordEntity;

  onUpdate?: (data: IRecordEditorData) => Promise<void>;
  onDelete?: () => Promise<void>;
  onPeriodsChanged?: () => void;
  onCancel?: () => void;
}

export function RecordUpdateModal(props: IRecordUpdateModalProps) {
  const {record, onUpdate, onDelete, onPeriodsChanged, onCancel} = props;

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | undefined>();

  const editorRef = useRef<IRecordEditorHandle>(null);
  const periodsDirtyRef = useRef(false);

  const changePeriodsDirty = useCallback((dirty: boolean) => {
    periodsDirtyRef.current = dirty;
  }, []);

  // Period changes are saved by their own buttons, so closing the dialog with unsaved ones loses them
  function confirmDiscardPeriods(): boolean {
    return !periodsDirtyRef.current
        || window.confirm("There are unsaved period changes. Discard them?");
  }

  function cancel() {
    if (confirmDiscardPeriods()) {
      onCancel?.();
    }
  }

  async function remove() {
    if (onDelete === undefined || !window.confirm("Delete this record?")) {
      return;
    }

    setLoading(true);
    try {
      await onDelete();
    } catch (error) {
      setError(ErrorHelper.getMessage(error, "Failed to delete the record"));
    } finally {
      setLoading(false);
    }
  }

  async function update() {
    if (editorRef.current === null || !confirmDiscardPeriods()) {
      return;
    }

    const valid = editorRef.current.validate();
    if (!valid) {
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
      setError(ErrorHelper.getMessage(error, "Failed to save the record"));
    } finally {
      setLoading(false);
    }
  }

  return (
      <>
        <div style={{display: "flex", justifyContent: "end", marginBottom: "5px"}}>
          <CustomButton label="Delete record" onClick={remove}>
            <span style={{marginLeft: "5px", fontSize: "20px"}} className="pi pi-trash"/>
          </CustomButton>
        </div>
        <RecordEditor
            ref={editorRef}
            record={record}/>
        <RecordPeriodsPanel
            record={record}
            onChanged={onPeriodsChanged}
            onDirtyChange={changePeriodsDirty}/>
        <ValidationError error={error}/>
        <DialogFooter
            loading={loading}
            actionText="Save"
            onAction={update}
            onCancel={cancel}/>
      </>
  )
}