import {IRecordEditorData, IRecordEditorHandle, RecordEditor} from "./RecordEditor.tsx";
import {useRef, useState} from "react";
import {DialogFooter} from "./components/DialogFooter.tsx";

export interface IRecordAddModalProps {
  onAdd?: (data: IRecordEditorData) => void;
  onCancel?: () => void;
}

export function RecordAddModal(props: IRecordAddModalProps) {
  const {onAdd, onCancel} = props;

  const [loading, setLoading] = useState(false);

  const editorRef = useRef<IRecordEditorHandle>(null);

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
        <RecordEditor ref={editorRef}/>
        <DialogFooter
          loading={loading}
          actionText="Add"
          onAction={add}
          onCancel={onCancel}/>
      </>
  );
}