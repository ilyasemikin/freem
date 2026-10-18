import {RecordEntity} from "./entities/RecordEntity.ts";
import {CustomButton} from "./components/inputs/CustomButton.tsx";
import {DateHelper} from "./helpers/DateHelper.ts";
import {useDialog} from "./contexts/dialog/useDialog.ts";
import {RecordUpdateModal} from "./RecordUpdateModal.tsx";
import {IRecordEditorData} from "./RecordEditor.tsx";
import {ExpandableItemPanel} from "./components/ExpandableItemPanel.tsx";
import {ActivitiesListPanel} from "./ActivitiesListPanel.tsx";
import {DateTimePeriodDurationPanel} from "./components/DateTimePeriodDurationPanel.tsx";

export interface IRecordPanelProps {
  record: RecordEntity;

  onUpdate: (data: IRecordEditorData) => Promise<void>
  onDelete: () => Promise<void>
}

export function RecordPanel(props: IRecordPanelProps) {
  const {record, onUpdate, onDelete} = props;

  const {openDialog, closeDialog} = useDialog();

  function openEdit() {
    openDialog({
      content: <RecordUpdateModal
          record={record}
          onUpdate={onUpdate}
          onDelete={onDelete}
          onCancel={closeDialog}/>
    });
  }

  const start = DateHelper.toFormattedString(record.period.startAt).slice(0, 5);
  const end = DateHelper.toFormattedString(record.period.endAt).slice(0, 5);

  function renderContent() {
    return (
        <div style={{display: "flex", justifyContent: "space-between", alignItems: "center"}}>
          <div style={{display: "flex", flexDirection: "column", justifyContent: "space-between"}}>
            <ActivitiesListPanel activities={record.activities}/>
            <span style={{fontSize: "13px", color: "darkslategray"}}>{record.name}</span>
          </div>
          <div style={{display: "flex", justifyContent: "space-between", alignItems: "center"}}>
            <span style={{fontSize: "13px", color: "gray", marginRight: "10px"}}>
              {start} - {end}
            </span>
            <DateTimePeriodDurationPanel period={record.period}/>
            <CustomButton onClick={openEdit}>
              <span style={{fontSize: "25px"}} className="pi pi-ellipsis-v"/>
            </CustomButton>
          </div>
        </div>
    );
  }

  function renderExpandedContent() {
    return (
        <>
          <span>Information</span>
        </>
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