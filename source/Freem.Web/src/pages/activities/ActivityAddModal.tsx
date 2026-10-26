import {useRef, useState} from "react";
import {ErrorHelper} from "../../helpers/ErrorHelper.ts";
import {ValidationError} from "../../components/ValidationError.tsx";
import {ActivityEditor, IActivityEditorData, IActivityEditorHandle} from "./ActivityEditor.tsx";
import {DialogFooter} from "../../dialog/DialogFooter.tsx";

export interface IActivityAddModalProps {
  onAdd?: (data: IActivityEditorData) => Promise<void>;
  onCancel?: () => void;
}

export function ActivityAddModal(props: IActivityAddModalProps) {
  const {onAdd, onCancel} = props;

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | undefined>();

  const editorRef = useRef<IActivityEditorHandle>(null);

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
      setError(ErrorHelper.getMessage(error, "Failed to add the activity"));
    } finally {
      setLoading(false);
    }
  }

  return (
      <>
        <ActivityEditor ref={editorRef}/>
        <ValidationError error={error}/>
        <DialogFooter
            loading={loading}
            actionText="Add"
            onAction={add}
            onCancel={onCancel}/>
      </>
  );
}