import {RecordEntity} from "../../entities/RecordEntity.ts";
import {CustomButton} from "../../components/inputs/CustomButton.tsx";
import {TimeZoneHelper} from "../../helpers/TimeZoneHelper.ts";
import {useUserSettings} from "../../settings/context/useUserSettings.ts";
import {useDialog} from "../../dialog/context/useDialog.ts";
import {RecordUpdateModal} from "./RecordUpdateModal.tsx";
import {IRecordEditorData} from "./RecordEditor.tsx";
import {ExpandableItemPanel} from "../../components/ExpandableItemPanel.tsx";
import {ActivitiesListPanel} from "../activities/ActivitiesListPanel.tsx";
import {TimeDurationPanel} from "../../components/TimeDurationPanel.tsx";
import {DateTimePeriod} from "../../data/DateTimePeriod.ts";

export interface IRecordPanelProps {
  record: RecordEntity;
  // Shown period: the duration counts only the time inside it
  visiblePeriod?: DateTimePeriod;

  onUpdate: (data: IRecordEditorData) => Promise<void>
  onDelete: () => Promise<void>
  onPeriodsChanged: () => void;
}

export function RecordPanel(props: IRecordPanelProps) {
  const {record, visiblePeriod, onUpdate, onDelete, onPeriodsChanged} = props;

  const {openDialog, closeDialog} = useDialog();
  const {timeZoneOffset} = useUserSettings();

  function openEdit() {
    openDialog({
      content: <RecordUpdateModal
          record={record}
          onUpdate={onUpdate}
          onDelete={onDelete}
          onPeriodsChanged={onPeriodsChanged}
          onCancel={closeDialog}/>
    });
  }

  const range = TimeZoneHelper.formatRange(record.startAt, record.endAt, timeZoneOffset);
  const periodsCount = record.periods.length;

  const startDay = TimeZoneHelper.toDate(record.startAt, timeZoneOffset);
  const startsBefore = visiblePeriod !== undefined && record.startAt < visiblePeriod.startAt;
  const duration = visiblePeriod !== undefined
      ? record.getDurationIn(visiblePeriod)
      : record.duration;
  const clipped = duration.seconds !== record.duration.seconds;

  function renderContent() {
    return (
        <div style={{display: "flex", justifyContent: "space-between", alignItems: "center"}}>
          <div style={{display: "flex", flexDirection: "column", justifyContent: "space-between"}}>
            <ActivitiesListPanel activities={record.activities}/>
            <span className="item-subtitle">
              {record.name}
              {periodsCount > 1 && ` (${periodsCount} periods)`}
              {startsBefore && ` · from ${TimeZoneHelper.formatDayMonth(startDay)}`}
            </span>
          </div>
          <div style={{display: "flex", justifyContent: "space-between", alignItems: "center"}}>
            <span className="numeric muted record-range" style={{fontSize: "13px", marginRight: "10px", whiteSpace: "nowrap"}}>
              {range}
            </span>
            <TimeDurationPanel duration={duration}/>
            <CustomButton label="Edit record" onClick={openEdit}>
              <span style={{fontSize: "25px"}} className="pi pi-ellipsis-v"/>
            </CustomButton>
          </div>
        </div>
    );
  }

  function renderExpandedContent() {
    return (
        <div style={{display: "flex", flexDirection: "column", gap: "4px"}}>
          {record.description && <span>{record.description}</span>}
          {clipped &&
              <span className="item-subtitle">
                {`Whole record: ${record.duration.toString()}, in this period: ${duration.toString()}`}
              </span>}
          {record.periods.map(item =>
              <div
                  key={item.id}
                  style={{display: "flex", justifyContent: "space-between", alignItems: "center"}}>
                <span className="numeric muted" style={{fontSize: "13px", marginRight: "10px"}}>
                  {TimeZoneHelper.formatRange(item.period.startAt, item.period.endAt, timeZoneOffset, startDay)}
                </span>
                <span className="item-subtitle" style={{flex: 1}}>{item.description}</span>
                <span className="numeric muted">{item.period.getDurationFormatted()}</span>
              </div>)}
        </div>
    );
  }

  return (
      <>
        <ExpandableItemPanel
            content={renderContent()}
            expandedContent={renderExpandedContent()}/>
      </>
  );
}