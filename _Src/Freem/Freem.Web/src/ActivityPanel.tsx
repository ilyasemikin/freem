import {ActivityEntity} from "./entities/ActivityEntity.ts";
import {IActivityEditorData} from "./ActivityEditor.tsx";
import {ActivityUpdateModal} from "./ActivityUpdateModal.tsx";
import {CustomButton} from "./components/inputs/CustomButton.tsx";
import {ItemPanel} from "./components/ItemPanel.tsx";
import {useDialog} from "./contexts/dialog/useDialog.ts";

export interface IActivityPanelProps {
  activity: ActivityEntity;

  onStart: () => void;
  onUpdate: (data: IActivityEditorData) => void;
  onDelete: () => void;
}

export function ActivityPanel(props: IActivityPanelProps) {
  const {activity, onStart, onUpdate, onDelete} = props;

  const {openDialog, closeDialog} = useDialog();

  function start() {
    onStart()
  }

  function update(data: IActivityEditorData) {
    onUpdate(data)
    closeDialog();
  }

  function openEdit() {
    openDialog({
      content: <ActivityUpdateModal
          activity={activity}
          onUpdate={update}
          onCancel={closeDialog}
          onDelete={onDelete}/>
    })
  }

  return (
      <ItemPanel>
        <span>{activity.name}</span>
        <div style={{display: "flex", justifyContent: "space-between", alignItems: "center"}}>
          <CustomButton onClick={start}>
            <span style={{fontSize: "25px"}} className="pi pi-play-circle"/>
          </CustomButton>
          <CustomButton onClick={openEdit}>
            <span style={{fontSize: "25px"}} className="pi pi-ellipsis-v"/>
          </CustomButton>
        </div>
      </ItemPanel>
  );
}