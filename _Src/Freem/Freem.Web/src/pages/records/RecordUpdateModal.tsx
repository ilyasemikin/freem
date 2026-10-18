import {RecordEntity} from "./entities/RecordEntity.ts";
import {IRecordEditorData, IRecordEditorHandle, RecordEditor} from "./RecordEditor.tsx";
import {useRef, useState} from "react";
import {CustomButton} from "./components/inputs/CustomButton.tsx";
import {DialogFooter} from "./components/DialogFooter.tsx";

export interface IRecordUpdateModalProps {
  record: RecordEntity;

  onUpdate?: (data: IRecordEditorData) => void;
  onDelete?: () => void;
  onCancel?: () => void;
}

export function RecordUpdateModal(props: IRecordUpdateModalProps) {
  const {record, onUpdate, onDelete, onCancel} = props;

  const [loading, setLoading] = useState(false);

  const editorRef = useRef<IRecordEditorHandle>(null);

  function update() {
    if (editorRef.current === null) {
      return;
    }

    const valid = editorRef.current.validate();
    if (!valid) {
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
          <CustomButton onClick={onDelete}>
            <span style={{marginLeft: "5px", fontSize: "20px"}} className="pi pi-trash"/>
          </CustomButton>
        </div>
        <RecordEditor
            ref={editorRef}
            record={record}/>
        <DialogFooter
            loading={loading}
            actionText="Save"
            onAction={update}
            onCancel={onCancel}/>
      </>
  )
}