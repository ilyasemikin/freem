import {TagEntity} from "./entities/TagEntity.ts";
import {ItemPanel} from "./components/ItemPanel.tsx";
import {CustomButton} from "./components/inputs/CustomButton.tsx";
import {ITagEditorData} from "./TagEditor.tsx";
import {TagUpdateModal} from "./TagUpdateModal.tsx";
import {useDialog} from "./contexts/dialog/useDialog.ts";

export interface ITagPanelProps {
  tag: TagEntity;

  onUpdate: (data: ITagEditorData) => void;
  onDelete: () => void;
}

export function TagPanel(props: ITagPanelProps) {
  const {tag, onUpdate, onDelete} = props;

  const {openDialog, closeDialog} = useDialog();

  function update(data: ITagEditorData) {
    onUpdate(data);
    closeDialog();
  }

  function remove() {
    onDelete();
    closeDialog();
  }

  function openEdit() {
    openDialog({
      content: <TagUpdateModal
          tag={tag}
          onUpdate={update}
          onDelete={remove}
          onCancel={closeDialog}/>
    })
  }

  return (
      <ItemPanel>
        <span>{tag.name}</span>
        <div>
          <CustomButton onClick={openEdit}>
            <span style={{fontSize: "25px"}} className="pi pi-ellipsis-v"/>
          </CustomButton>
        </div>
      </ItemPanel>
  );
}