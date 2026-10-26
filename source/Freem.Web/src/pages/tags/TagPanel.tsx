import {TagEntity} from "../../entities/TagEntity.ts";
import {ItemPanel} from "../../components/ItemPanel.tsx";
import {CustomButton} from "../../components/inputs/CustomButton.tsx";
import {ITagEditorData} from "./TagEditor.tsx";
import {TagUpdateModal} from "./TagUpdateModal.tsx";
import {useDialog} from "../../dialog/context/useDialog.ts";

export interface ITagPanelProps {
  tag: TagEntity;

  onUpdate: (data: ITagEditorData) => Promise<void>;
  onDelete: () => Promise<void>;
}

export function TagPanel(props: ITagPanelProps) {
  const {tag, onUpdate, onDelete} = props;

  const {openDialog, closeDialog} = useDialog();

  async function update(data: ITagEditorData) {
    await onUpdate(data);
    closeDialog();
  }

  async function remove() {
    await onDelete();
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
        <span className="item-title">{tag.name}</span>
        <div>
          <CustomButton label="Edit tag" onClick={openEdit}>
            <span style={{fontSize: "25px"}} className="pi pi-ellipsis-v"/>
          </CustomButton>
        </div>
      </ItemPanel>
  );
}