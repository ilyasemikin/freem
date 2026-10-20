import {RecordEntity} from "../../entities/RecordEntity.ts";
import {CustomButton} from "../../components/inputs/CustomButton.tsx";
import {DateHelper} from "../../helpers/DateHelper.ts";
import {useDialog} from "../../dialog/context/useDialog.ts";
import {RecordUpdateModal} from "./RecordUpdateModal.tsx";
import {IRecordEditorData} from "./RecordEditor.tsx";
import {ExpandableItemPanel} from "../../components/ExpandableItemPanel.tsx";
import {ActivitiesListPanel} from "../activities/ActivitiesListPanel.tsx";
import {TimeDurationPanel} from "../../components/TimeDurationPanel.tsx";

function formatTime(date: Date): string {
  return DateHelper.toFormattedString(date).slice(0, 5);
}

export interface IRecordPanelProps {
  record: RecordEntity;

  onUpdate: (data: IRecordEditorData) => Promise<void>
  onDelete: () => Promise<void>
  onPeriodsChanged: () => void;
}

export function RecordPanel(props: IRecordPanelProps) {
  const {record, onUpdate, onDelete, onPeriodsChanged} = props;

  const {openDialog, closeDialog} = useDialog();

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

  const start = formatTime(record.startAt);
  const end = formatTime(record.endAt);
  const periodsCount = record.periods.length;

  function renderContent() {
    return (
        <div style={{display: "flex", justifyContent: "space-between", alignItems: "center"}}>
          <div style={{display: "flex", flexDirection: "column", justifyContent: "space-between"}}>
            <ActivitiesListPanel activities={record.activities}/>
            <span className="item-subtitle">
              {record.name}
              {periodsCount > 1 && ` (${periodsCount} periods)`}
            </span>
          </div>
          <div style={{display: "flex", justifyContent: "space-between", alignItems: "center"}}>
            <span className="numeric muted" style={{fontSize: "13px", marginRight: "10px"}}>
              {start} - {end}
            </span>
            <TimeDurationPanel duration={record.duration}/>
            <CustomButton onClick={openEdit}>
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
          {record.periods.map(item =>
              <div
                  key={item.id}
                  style={{display: "flex", justifyContent: "space-between", alignItems: "center"}}>
                <span className="numeric muted" style={{fontSize: "13px", marginRight: "10px"}}>
                  {formatTime(item.period.startAt)} - {formatTime(item.period.endAt)}
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