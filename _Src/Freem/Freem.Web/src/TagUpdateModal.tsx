import {TagEntity} from "./entities/TagEntity.ts";
import {ITagEditorData, ITagEditorHandle, TagEditor} from "./TagEditor.tsx";
import {useRef, useState} from "react";
import {CustomButton} from "./components/inputs/CustomButton.tsx";
import {DialogFooter} from "./components/DialogFooter.tsx";

export interface ITagUpdateModalProps {
  tag?: TagEntity;

  onUpdate?: (data: ITagEditorData) => void;
  onDelete?: () => void;
  onCancel?: () => void;
}

export function TagUpdateModal(props: ITagUpdateModalProps) {
  const {tag, onUpdate, onDelete, onCancel} = props;

  const [loading, setLoading] = useState(false);

  const editorRef = useRef<ITagEditorHandle>(null);

  function update() {
    if (editorRef.current === null) {
      return;
    }

    const validationResult = editorRef.current.validate();
    if (!validationResult) {
      return;
    }

    const data = editorRef.current.data();
    if (onUpdate != undefined) {
      setLoading(true);
      onUpdate(data);
      setLoading(false);
    }
  }

  return (
      <>
        <div style={{display: "flex", justifyContent: "end", marginBottom: "5px"}}>
          <CustomButton onClick={onDelete}>
            <span style={{marginLeft: "5px", fontSize: "20px"}} className="pi pi-trash"/>
          </CustomButton>
        </div>
        <TagEditor
            ref={editorRef}
            tag={tag}/>
        <DialogFooter
            loading={loading}
            actionText="Save"
            onAction={update}
            onCancel={onCancel}/>
      </>
  )
}