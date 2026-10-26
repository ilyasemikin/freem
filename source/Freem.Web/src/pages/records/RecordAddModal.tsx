import {IRecordEditorData, IRecordEditorHandle, RecordEditor} from "./RecordEditor.tsx";
import {ErrorHelper} from "../../helpers/ErrorHelper.ts";
import {ValidationError} from "../../components/ValidationError.tsx";
import {useRef, useState} from "react";
import {DialogFooter} from "../../dialog/DialogFooter.tsx";

export interface IRecordAddModalProps {
  onAdd?: (data: IRecordEditorData) => Promise<void>;
  onCancel?: () => void;
}

export function RecordAddModal(props: IRecordAddModalProps) {
  const {onAdd, onCancel} = props;

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | undefined>();

  const editorRef = useRef<IRecordEditorHandle>(null);

  async function add() {
    if (editorRef.current === null) {
      return;
    }

    const validationResult = editorRef.current.validate();
    if (!validationResult) {
      return;
    }

    const data = editorRef.current.data();
    if (onAdd === undefined) {
      return;
    }

    setLoading(true);
    try {
      await onAdd(data);
    } catch (error) {
      setError(ErrorHelper.getMessage(error, "Failed to add the record"));
    } finally {
      setLoading(false);
    }
  }

  return (
      <>
        <RecordEditor ref={editorRef}/>
        <ValidationError error={error}/>
        <DialogFooter
          loading={loading}
          actionText="Add"
          onAction={add}
          onCancel={onCancel}/>
      </>
  );
}