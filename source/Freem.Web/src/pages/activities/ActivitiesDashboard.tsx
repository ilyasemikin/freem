import {ActivityEntity} from "../../entities/ActivityEntity.ts";
import {useEffect, useState} from "react";
import {ListActivityRequest} from "../../clients/models/activities/ListActivityRequest.ts";
import {Loading} from "../../components/Loading.tsx";
import {ValidationError} from "../../components/ValidationError.tsx";
import {ActivityPanel} from "./ActivityPanel.tsx";
import {RunningRecordEntity} from "../../entities/RunningRecordEntity.ts";
import {RunningRecordPanel} from "./RunningRecordPanel.tsx";
import {StartRunningRecordRequest} from "../../clients/models/records/running/StartRunningRecordRequest.ts";
import {StopRunningRecordRequest} from "../../clients/models/records/running/StopRunningRecordRequest.ts";
import {IActivityEditorData} from "./ActivityEditor.tsx";
import {UpdateActivityRequest} from "../../clients/models/activities/UpdateActivityRequest.ts";
import {UpdateField} from "../../clients/models/UpdateField.ts";
import {ActivityAddModal} from "./ActivityAddModal.tsx";
import {CreateActivityRequest} from "../../clients/models/activities/CreateActivityRequest.ts";
import {IRunningRecordEditorData} from "./RunningRecordEditor.tsx";
import {UpdateRunningRecordRequest} from "../../clients/models/records/running/UpdateRunningRecordRequest.ts";
import {PanelHeader} from "../../components/PanelHeader.tsx";
import {RunningRecordResponse} from "../../clients/models/records/running/RunningRecordResponse.ts";
import {useBackendClients} from "../../clients/context/useBackendClients.ts";
import {useEntitiesCache} from "../../cache/context/useEntitiesCache.ts";
import {useDialog} from "../../dialog/context/useDialog.ts";
import {ErrorHelper} from "../../helpers/ErrorHelper.ts";

export interface IActivitiesDashboardProps {
  onRecordsChanged?: () => void;
}

export function ActivitiesDashboard(props: IActivitiesDashboardProps) {
  const {onRecordsChanged} = props;

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | undefined>();
  const [update, setUpdate] = useState(false);

  const [activities, setActivities] = useState<ActivityEntity[] | undefined>();
  const [running, setRunning] = useState<RunningRecordEntity | undefined>();

  const {openDialog, closeDialog} = useDialog();
  const clients = useBackendClients();
  const cache = useEntitiesCache();

  useEffect(() => {
    let current = true;

    async function loadActivities() {
      const request = new ListActivityRequest(0, 100);
      const activities = await cache.activities.listByRequest(request);

      if (current) {
        setActivities(activities);
      }
    }

    async function loadRunningRecord() {
      const response = await clients.records.getRunning();

      if (!response.ok && response.status !== 404) {
        throw new Error(response.statusText);
      }

      const payload = await response.json();
      const body: RunningRecordResponse = response.ok ? payload.runningRecord : payload;

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

        if (current) {
          setRunning(record);
        }
      } else if (current) {
        setRunning(undefined);
      }
    }

    async function load() {
      await Promise.all([loadActivities(), loadRunningRecord()]);
      if (current) {
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
  }, [update, clients.records, cache.activities, cache.tags]);

  function openAdd() {
    openDialog({
      content: <ActivityAddModal
          onAdd={addActivity}
          onCancel={closeDialog}/>
    })
  }

  async function addActivity(data: IActivityEditorData) {
    const request = new CreateActivityRequest(data.name, data.color, data.tagIds);
    const response = await clients.activities.create(request);
    await ErrorHelper.throwIfFailed(response);

    closeDialog();
    setUpdate(value => !value);
  }

  async function updateActivity(id: string, data: IActivityEditorData) {
    const nameUpdateField = new UpdateField(data.name);
    const colorUpdateField = new UpdateField(data.color);
    const tagsUpdateField = new UpdateField(data.tagIds);

    const request = new UpdateActivityRequest(nameUpdateField, colorUpdateField, tagsUpdateField);
    const response = await clients.activities.update(id, request);
    await ErrorHelper.throwIfFailed(response);

    setUpdate(value => !value);
  }

  async function deleteActivity(id: string) {
    const response = await clients.activities.remove(id);
    await ErrorHelper.throwIfFailed(response);

    setUpdate(value => !value);
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
    const startAtChanged = running === undefined || data.startAt.getTime() !== running.startAt.getTime();
    const startAt = startAtChanged ? new UpdateField(data.startAt.toISOString()) : undefined;

    const request = new UpdateRunningRecordRequest(name, description, activities, tags, startAt);
    const response = await clients.records.updateRunning(request);
    await ErrorHelper.throwIfFailed(response);

    setUpdate(value => !value);
  }

  async function stopRunningRecord() {
    const now = new Date();
    const request = new StopRunningRecordRequest(now.toISOString());
    const response = await clients.records.stop(request);

    if (response.ok || response.status === 404) {
      setRunning(undefined);
      onRecordsChanged?.();
    }
  }

  async function deleteRunningRecord() {
    const response = await clients.records.removeRunning();

    if (response.ok || response.status === 404) {
      setUpdate(value => !value);
    }
  }

  if (loading) {
    return (
        <>
          <PanelHeader title="Activities"/>
          <Loading/>
        </>
    );
  }

  if (error !== undefined) {
    return (
        <>
          <PanelHeader title="Activities"/>
          <ValidationError error={error}/>
        </>
    );
  }

  const showed = activities === undefined ? [] : activities.filter(activity => !running?.activities?.find(value => value.id === activity.id));
  return (
      <div style={{display: "flex", flexDirection: "column", flex: "1"}}>
        <PanelHeader title="Activities" onAdd={openAdd}/>
        {running &&
            <RunningRecordPanel
                record={running}
                onStop={stopRunningRecord}
                onUpdate={updateRunningRecord}
                onDelete={deleteRunningRecord}/>}
        {!running && showed.length === 0 && <div className="empty">No activities yet. Add the first one with +</div>}
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