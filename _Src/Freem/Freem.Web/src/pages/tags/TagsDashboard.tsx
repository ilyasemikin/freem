import {useEffect, useState} from "react";
import {TagEntity} from "./entities/TagEntity.ts";
import {ListTagRequest} from "./clients/models/tags/ListTagRequest.ts";
import {Loading} from "./components/Loading.tsx";
import {CustomButton} from "./components/inputs/CustomButton.tsx";
import {TagAddModal} from "./TagAddModal.tsx";
import {ITagEditorData} from "./TagEditor.tsx";
import {CreateTagRequest} from "./clients/models/tags/CreateTagRequest.ts";
import {TagPanel} from "./TagPanel.tsx";
import {UpdateTagRequest} from "./clients/models/tags/UpdateTagRequest.ts";
import {UpdateField} from "./clients/models/UpdateField.ts";
import {useBackendClients} from "./contexts/clients/useBackendClients.ts";
import {useEntitiesCache} from "./contexts/cache/useEntitiesCache.ts";
import {useDialog} from "./contexts/dialog/useDialog.ts";
import {Divider} from "./components/Divider.tsx";

export function TagsDashboard() {
  const [loading, setLoading] = useState(true);
  const [update, setUpdate] = useState<boolean>(false);

  const [tags, setTags] = useState<TagEntity[] | undefined>();

  const {openDialog, closeDialog} = useDialog();
  const clients = useBackendClients();
  const cache = useEntitiesCache();

  useEffect(() => {
    async function loadTags() {
      const request = new ListTagRequest(0, 100);
      const tags = await cache.tags.listByRequest(request);
      setTags(tags);
    }

    setLoading(true);
    loadTags();
    setLoading(false);
  }, [update, cache.tags]);

  function openAdd() {
    openDialog({
      content: <TagAddModal
          onAdd={addTag}
          onCancel={closeDialog}/>
    })
  }

  async function addTag(data: ITagEditorData) {
    const request = new CreateTagRequest(data.name);
    const response = await clients.tags.create(request);

    if (response.ok) {
      closeDialog();
      setUpdate(value => !value);
    }
  }

  async function updateTag(id: string, data: ITagEditorData) {
    const name = new UpdateField(data.name);
    const request = new UpdateTagRequest(name);
    const response = await clients.tags.update(id, request);

    if (response.ok) {
      setUpdate(value => !value);
    }
  }

  async function deleteTag(id: string) {
    const response = await clients.tags.remove(id);

    if (response.ok) {
      setUpdate(value => !value);
    }
  }

  if (loading) {
    return (<Loading/>);
  }

  return (
      <>
        <div style={{display: "flex", justifyContent: "end", margin: "0 15px 15px 0"}}>
          <CustomButton onClick={openAdd}>
            <span className="pi pi-plus"/>
          </CustomButton>
        </div>
        <Divider/>
        {tags && tags.map(tag =>
            <TagPanel
                key={tag.id}
                tag={tag}
                onUpdate={data => updateTag(tag.id, data)}
                onDelete={() => deleteTag(tag.id)}/>)}
      </>
  );
}