import {ActivityEntity} from "./entities/ActivityEntity.ts";
import {useEffect, useState} from "react";
import {ListActivityRequest} from "./clients/models/activities/ListActivityRequest.ts";
import {Loading} from "./components/Loading.tsx";
import {ActivityPanel} from "./ActivityPanel.tsx";
import {RunningRecordEntity} from "./entities/RunningRecordEntity.ts";
import {RunningRecordPanel} from "./RunningRecordPanel.tsx";
import {StartRunningRecordRequest} from "./clients/models/records/running/StartRunningRecordRequest.ts";
import {StopRunningRecordRequest} from "./clients/models/records/running/StopRunningRecordRequest.ts";
import {IActivityEditorData} from "./ActivityEditor.tsx";
import {UpdateActivityRequest} from "./clients/models/activities/UpdateActivityRequest.ts";
import {UpdateField} from "./clients/models/UpdateField.ts";
import {ActivityAddModal} from "./ActivityAddModal.tsx";
import {CreateActivityRequest} from "./clients/models/activities/CreateActivityRequest.ts";
import {IRunningRecordEditorData} from "./RunningRecordEditor.tsx";
import {UpdateRunningRecordRequest} from "./clients/models/records/running/UpdateRunningRecordRequest.ts";
import {CustomButton} from "./components/inputs/CustomButton.tsx";
import {RunningRecordResponse} from "./clients/models/records/running/RunningRecordResponse.ts";
import {useBackendClients} from "./contexts/clients/useBackendClients.ts";
import {useEntitiesCache} from "./contexts/cache/useEntitiesCache.ts";
import {useDialog} from "./contexts/dialog/useDialog.ts";
import {Divider} from "./components/Divider.tsx";

export function ActivitiesDashboard() {
  const [loading, setLoading] = useState(true);
  const [update, setUpdate] = useState(false);

  const [activities, setActivities] = useState<ActivityEntity[] | undefined>();
  const [running, setRunning] = useState<RunningRecordEntity | undefined>();

  const {openDialog, closeDialog} = useDialog();
  const clients = useBackendClients();
  const cache = useEntitiesCache();

  useEffect(() => {
    async function loadActivities() {
      const request = new ListActivityRequest(0, 100);
      const activities = await cache.activities.listByRequest(request);

      setActivities(activities);
    }

    async function loadRunningRecord() {
      const response = await clients.records.getRunning();

      if (!response.ok && response.status !== 404) {
        throw new Error(response.statusText);
      }

      const body: RunningRecordResponse = await response.json();

      if (response.ok) {
        const record = new RunningRecordEntity(
            new Date(body.startAt),
            await cache.activities.list(body.activities),
            body.name,
            body.description,
            body.tags !== undefined
                ? await cache.tags.list(body.tags)
                : undefined
        );

        setRunning(record);
      } else {
        setRunning(undefined);
      }
    }

    setLoading(true);

    loadActivities()
    loadRunningRecord();

    setLoading(false);
  }, [update, clients.records]);

  function openAdd() {
    openDialog({
      content: <ActivityAddModal
          onAdd={addActivity}
          onCancel={closeDialog}/>
    })
  }

  async function addActivity(data: IActivityEditorData) {
    const request = new CreateActivityRequest(data.name, data.tagIds);
    const response = await clients.activities.create(request);

    if (response.ok) {
      closeDialog();
      setUpdate(value => !value);
    }
  }

  async function updateActivity(id: string, data: IActivityEditorData) {
    const nameUpdateField = new UpdateField(data.name);
    const tagsUpdateField = new UpdateField(data.tagIds);

    const request = new UpdateActivityRequest(nameUpdateField, tagsUpdateField);
    const response = await clients.activities.update(id, request);

    if (response.ok) {
      setUpdate(value => !value);
    }
  }

  async function deleteActivity(id: string) {
    const response = await clients.activities.remove(id);

    if (response.ok) {
      closeDialog();
      setUpdate(value => !value);
    }
  }

  async function startRunningRecord(ids: string[]) {
    setRunning(undefined);

    const now = new Date();
    const request = new StartRunningRecordRequest(now.toISOString(), ids);
    const response = await clients.records.start(request);

    if (response.ok) {
      setUpdate(value => !value);
    }
  }

  async function updateRunningRecord(data: IRunningRecordEditorData) {
    const name = data.name !== undefined ? new UpdateField(data.name) : undefined;
    const description = data.description !== undefined ? new UpdateField(data.description) : undefined;
    const activities = new UpdateField(data.activityIds);
    const tags = new UpdateField(data.tagIds);

    const request = new UpdateRunningRecordRequest(name, description, activities, tags);
    const response = await clients.records.updateRunning(request);

    if (response.ok) {
      setUpdate(value => !value);
    } else {
      throw new Error(response.statusText);
    }
  }

  async function stopRunningRecord() {
    const now = new Date();
    const request = new StopRunningRecordRequest(now.toISOString());
    const response = await clients.records.stop(request);

    if (response.ok || response.status === 404) {
      setRunning(undefined);
    }
  }

  async function deleteRunningRecord() {
    const response = await clients.records.removeRunning();

    if (response.ok || response.status === 404) {
      setUpdate(value => !value);
    }
  }

  if (loading) {
    return (<Loading/>);
  }

  const showed = activities === undefined ? [] : activities.filter(activity => !running?.activities?.find(value => value.id === activity.id));
  return (
      <div style={{display: "flex", flexDirection: "column", flex: "1"}}>
        <div style={{display: "flex", justifyContent: "end", margin: "5px 15px 5px 0"}}>
          <CustomButton onClick={openAdd}>
            <span className="pi pi-plus"/>
          </CustomButton>
        </div>
        <Divider/>
        {running &&
            <RunningRecordPanel
                record={running}
                onStop={stopRunningRecord}
                onUpdate={updateRunningRecord}
                onDelete={deleteRunningRecord}/>}
        {showed.map(activity =>
            <ActivityPanel
                key={activity.id}
                activity={activity}
                onStart={() => startRunningRecord([activity.id])}
                onUpdate={data => updateActivity(activity.id, data)}
                onDelete={() => deleteActivity(activity.id)}/>)}
      </div>
  );
}