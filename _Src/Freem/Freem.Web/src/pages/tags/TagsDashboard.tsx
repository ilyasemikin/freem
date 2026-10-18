import {useEffect, useState} from "react";
import {TagEntity} from "../../entities/TagEntity.ts";
import {ListTagRequest} from "../../clients/models/tags/ListTagRequest.ts";
import {Loading} from "../../components/Loading.tsx";
import {ValidationError} from "../../components/ValidationError.tsx";
import {CustomButton} from "../../components/inputs/CustomButton.tsx";
import {TagAddModal} from "./TagAddModal.tsx";
import {ITagEditorData} from "./TagEditor.tsx";
import {CreateTagRequest} from "../../clients/models/tags/CreateTagRequest.ts";
import {TagPanel} from "./TagPanel.tsx";
import {UpdateTagRequest} from "../../clients/models/tags/UpdateTagRequest.ts";
import {UpdateField} from "../../clients/models/UpdateField.ts";
import {useBackendClients} from "../../clients/context/useBackendClients.ts";
import {useEntitiesCache} from "../../cache/context/useEntitiesCache.ts";
import {useDialog} from "../../dialog/context/useDialog.ts";
import {Divider} from "../../components/Divider.tsx";

export function TagsDashboard() {
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | undefined>();
  const [update, setUpdate] = useState<boolean>(false);

  const [tags, setTags] = useState<TagEntity[] | undefined>();

  const {openDialog, closeDialog} = useDialog();
  const clients = useBackendClients();
  const cache = useEntitiesCache();

  useEffect(() => {
    let current = true;

    async function load() {
      const request = new ListTagRequest(0, 100);
      const tags = await cache.tags.listByRequest(request);
      if (current) {
        setTags(tags);
        setError(undefined);
      }
    }

    load().catch(error => {
      if (current) {
        setError(error instanceof Error ? error.message : "Failed to load data");
      }
    }).finally(() => {
      if (current) {
        setLoading(false);
      }
    });

    return () => {
      current = false;
    };
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

  if (error !== undefined) {
    return <ValidationError error={error}/>;
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