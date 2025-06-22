import {ActivityEditor, IActivityEditorData, IActivityEditorHandle} from "./ActivityEditor.tsx";
import {useRef, useState} from "react";
import {ActivityEntity} from "./entities/ActivityEntity.ts";
import {DialogFooter} from "./components/DialogFooter.tsx";
import {CustomButton} from "./components/inputs/CustomButton.tsx";

export interface IActivityUpdateModalProps {
  activity: ActivityEntity;

  onUpdate?: (data: IActivityEditorData) => void;
  onDelete?: () => void;
  onCancel?: () => void;
}

export function ActivityUpdateModal(props: IActivityUpdateModalProps) {
  const {activity, onUpdate, onDelete, onCancel} = props;

  const [loading, setLoading] = useState(false);

  const editorRef = useRef<IActivityEditorHandle>(null);

  function update() {
    if (editorRef.current === null) {
      return;
    }

    const validationResult = editorRef.current.validate();
    if (!validationResult) {
      return;
    }

    const data = editorRef.current.data();
    if (onUpdate !== undefined) {
      setLoading(true);
      onUpdate(data);
      setLoading(false);
    }
  }

  return (
      <>
        <div style={{display: "flex", justifyContent: "end", marginBottom: "5px"}}>
          <CustomButton>
            <span style={{fontSize: "20px"}} className="pi pi-inbox"/>
          </CustomButton>
          <CustomButton onClick={onDelete}>
            <span style={{ marginLeft: "5px", fontSize: "20px" }} className="pi pi-trash"/>
          </CustomButton>
        </div>
        <ActivityEditor
            ref={editorRef}
            activity={activity}/>
        <DialogFooter
            loading={loading}
            actionText="Save"
            onAction={update}
            onCancel={onCancel}/>
      </>
  );
}