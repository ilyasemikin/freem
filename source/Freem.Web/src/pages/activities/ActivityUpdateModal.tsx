import {ActivityEditor, IActivityEditorData, IActivityEditorHandle} from "./ActivityEditor.tsx";
import {ErrorHelper} from "../../helpers/ErrorHelper.ts";
import {ValidationError} from "../../components/ValidationError.tsx";
import {useRef, useState} from "react";
import {ActivityEntity} from "../../entities/ActivityEntity.ts";
import {DialogFooter} from "../../dialog/DialogFooter.tsx";
import {CustomButton} from "../../components/inputs/CustomButton.tsx";

export interface IActivityUpdateModalProps {
  activity: ActivityEntity;

  onUpdate?: (data: IActivityEditorData) => Promise<void>;
  onDelete?: () => Promise<void>;
  onCancel?: () => void;
}

export function ActivityUpdateModal(props: IActivityUpdateModalProps) {
  const {activity, onUpdate, onDelete, onCancel} = props;

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | undefined>();

  const editorRef = useRef<IActivityEditorHandle>(null);

  async function remove() {
    if (onDelete === undefined || !window.confirm("Delete this activity?")) {
      return;
    }

    setLoading(true);
    try {
      await onDelete();
    } catch (error) {
      setError(ErrorHelper.getMessage(error, "Failed to delete the activity"));
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
      setError(ErrorHelper.getMessage(error, "Failed to save the activity"));
    } finally {
      setLoading(false);
    }
  }

  return (
      <>
        <div style={{display: "flex", justifyContent: "end", marginBottom: "5px"}}>
          <CustomButton label="Archive activity">
            <span style={{fontSize: "20px"}} className="pi pi-inbox"/>
          </CustomButton>
          <CustomButton label="Delete activity" onClick={remove}>
            <span style={{ marginLeft: "5px", fontSize: "20px" }} className="pi pi-trash"/>
          </CustomButton>
        </div>
        <ActivityEditor
            ref={editorRef}
            activity={activity}/>
        <ValidationError error={error}/>
        <DialogFooter
            loading={loading}
            actionText="Save"
            onAction={update}
            onCancel={onCancel}/>
      </>
  );
}