import {DialogFooter} from "./components/DialogFooter.tsx";
import {useRef, useState} from "react";
import {ITagEditorData, ITagEditorHandle, TagEditor} from "./TagEditor.tsx";

export interface ITagAddModalProps {
  onAdd?: (data: ITagEditorData) => void;
  onCancel: () => void;
}

export function TagAddModal(props: ITagAddModalProps) {
  const {onAdd, onCancel} = props;

  const [loading, setLoading] = useState(false);

  const editorRef = useRef<ITagEditorHandle>(null);

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
        <TagEditor ref={editorRef}/>
        <DialogFooter
            loading={loading}
            actionText="Add"
            onAction={add}
            onCancel={onCancel}/>
      </>
  )
}