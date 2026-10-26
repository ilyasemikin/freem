import {TagEntity} from "../../entities/TagEntity.ts";
import {ErrorHelper} from "../../helpers/ErrorHelper.ts";
import {ValidationError} from "../../components/ValidationError.tsx";
import {ITagEditorData, ITagEditorHandle, TagEditor} from "./TagEditor.tsx";
import {useRef, useState} from "react";
import {CustomButton} from "../../components/inputs/CustomButton.tsx";
import {DialogFooter} from "../../dialog/DialogFooter.tsx";

export interface ITagUpdateModalProps {
  tag?: TagEntity;

  onUpdate?: (data: ITagEditorData) => Promise<void>;
  onDelete?: () => Promise<void>;
  onCancel?: () => void;
}

export function TagUpdateModal(props: ITagUpdateModalProps) {
  const {tag, onUpdate, onDelete, onCancel} = props;

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | undefined>();

  const editorRef = useRef<ITagEditorHandle>(null);

  async function remove() {
    if (onDelete === undefined || !window.confirm("Delete this tag? It will be removed from all activities and records.")) {
      return;
    }

    setLoading(true);
    try {
      await onDelete();
    } catch (error) {
      setError(ErrorHelper.getMessage(error, "Failed to delete the tag"));
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
      setError(ErrorHelper.getMessage(error, "Failed to save the tag"));
    } finally {
      setLoading(false);
    }
  }

  return (
      <>
        <div style={{display: "flex", justifyContent: "end", marginBottom: "5px"}}>
          <CustomButton label="Delete tag" onClick={remove}>
            <span style={{marginLeft: "5px", fontSize: "20px"}} className="pi pi-trash"/>
          </CustomButton>
        </div>
        <TagEditor
            ref={editorRef}
            tag={tag}/>
        <ValidationError error={error}/>
        <DialogFooter
            loading={loading}
            actionText="Save"
            onAction={update}
            onCancel={onCancel}/>
      </>
  )
}