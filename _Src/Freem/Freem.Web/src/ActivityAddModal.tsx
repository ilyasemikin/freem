import {useRef, useState} from "react";
import {ActivityEditor, IActivityEditorData, IActivityEditorHandle} from "./ActivityEditor.tsx";
import {DialogFooter} from "./components/DialogFooter.tsx";

export interface IActivityAddModalProps {
  onAdd?: (data: IActivityEditorData) => void;
  onCancel?: () => void;
}

export function ActivityAddModal(props: IActivityAddModalProps) {
  const {onAdd, onCancel} = props;

  const [loading, setLoading] = useState(false);

  const editorRef = useRef<IActivityEditorHandle>(null);

  function add() {
    if (editorRef.current === null) {
      return;
    }

    const validationResult = editorRef.current.validate();
    if (!validationResult) {
      return;
    }

    const data = editorRef.current.data();
    if (onAdd !== undefined) {
      setLoading(true);
      onAdd(data);
      setLoading(false);
    }
  }

  return (
      <>
        <ActivityEditor ref={editorRef}/>
        <DialogFooter
            loading={loading}
            actionText="Add"
            onAction={add}
            onCancel={onCancel}/>
      </>
  );
}