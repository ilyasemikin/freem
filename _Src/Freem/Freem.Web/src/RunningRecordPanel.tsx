import {useEffect, useState} from "react";
import {RunningRecordEntity} from "./entities/RunningRecordEntity.ts";
import {RunningRecordUpdateModal} from "./RunningRecordUpdateModal.tsx";
import {IRunningRecordEditorData} from "./RunningRecordEditor.tsx";
import {ItemPanel} from "./components/ItemPanel.tsx";
import {CustomButton} from "./components/inputs/CustomButton.tsx";
import { useDialog } from "./contexts/dialog/useDialog.ts";
import {DateTimePeriod} from "./data/DateTimePeriod.ts";
import {ActivitiesListPanel} from "./ActivitiesListPanel.tsx";
import {DateTimePeriodDurationPanel} from "./components/DateTimePeriodDurationPanel.tsx";

export interface IRunningRecordPanelProps {
  record: RunningRecordEntity;

  onStop: () => Promise<void>;
  onUpdate: (data: IRunningRecordEditorData) => Promise<void>;
  onDelete: () => Promise<void>;
}

export function RunningRecordPanel(props: IRunningRecordPanelProps) {
  const {record, onStop, onUpdate, onDelete} = props;

  function calculateSeconds(startAt: Date) {
    const now = new Date();
    return Math.round((now.getTime() - startAt.getTime()) / 1000);
  }

  const [seconds, setSeconds] = useState<number>(calculateSeconds(record.startAt));
  const [stopped, setStopped] = useState(false);

  const {openDialog, closeDialog} = useDialog();

  useEffect(() => {
    const timer = setInterval(() => {
      const newSeconds = calculateSeconds(record.startAt);
      if (stopped) {
        return;
      }

      setSeconds(newSeconds);
    }, 500);

    return () => clearInterval(timer);
  }, [seconds, stopped, record.startAt]);

  async function stop() {
    if (stopped) {
      return;
    }

    setStopped(true);
    await onStop();
  }

  async function update(data: IRunningRecordEditorData) {
    await onUpdate(data);
    closeDialog();
  }

  async function remove() {
    await onDelete();
    closeDialog();
  }

  function openEdit() {
    openDialog({
      content: <RunningRecordUpdateModal
          record={record}
          onUpdate={update}
          onDelete={remove}
          onCancel={closeDialog}/>
    })
  }

  const now = new Date();
  const period = new DateTimePeriod(record.startAt, now);

  return (
      <ItemPanel>
        <ActivitiesListPanel activities={record.activities}/>
        <div style={{display: "flex", justifyContent: "space-between", alignItems: "right"}}>
          <DateTimePeriodDurationPanel period={period} align="right"/>
          <CustomButton onClick={stop}>
            <span style={{fontSize: "25px"}} className="pi pi-stop-circle"/>
          </CustomButton>
          <CustomButton onClick={openEdit}>
            <span style={{fontSize: "25px"}} className="pi pi-ellipsis-v"/>
          </CustomButton>
        </div>
      </ItemPanel>
  );
}