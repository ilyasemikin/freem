import {DialogFooter} from "../../dialog/DialogFooter.tsx";
import {ErrorHelper} from "../../helpers/ErrorHelper.ts";
import {ValidationError} from "../../components/ValidationError.tsx";
import {useRef, useState} from "react";
import {ITagEditorData, ITagEditorHandle, TagEditor} from "./TagEditor.tsx";

export interface ITagAddModalProps {
  onAdd?: (data: ITagEditorData) => Promise<void>;
  onCancel: () => void;
}

export function TagAddModal(props: ITagAddModalProps) {
  const {onAdd, onCancel} = props;

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | undefined>();

  const editorRef = useRef<ITagEditorHandle>(null);

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
      setError(ErrorHelper.getMessage(error, "Failed to add the tag"));
    } finally {
      setLoading(false);
    }
  }

  return (
      <>
        <TagEditor ref={editorRef}/>
        <ValidationError error={error}/>
        <DialogFooter
            loading={loading}
            actionText="Add"
            onAction={add}
            onCancel={onCancel}/>
      </>
  )
}