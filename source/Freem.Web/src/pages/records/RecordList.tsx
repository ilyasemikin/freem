import {RecordEntity} from "../../entities/RecordEntity.ts";
import {RecordPanel} from "./RecordPanel.tsx";
import {IRecordEditorData} from "./RecordEditor.tsx";
import {UpdateField} from "../../clients/models/UpdateField.ts";
import {UpdateRecordRequest} from "../../clients/models/records/UpdateRecordRequest.ts";
import {useDialog} from "../../dialog/context/useDialog.ts";
import {useBackendClients} from "../../clients/context/useBackendClients.ts";
import {ErrorHelper} from "../../helpers/ErrorHelper.ts";
import {DateTimePeriod} from "../../data/DateTimePeriod.ts";

export interface IRecordListProps {
  date?: string;
  records: RecordEntity[];
  visiblePeriod?: DateTimePeriod;

  onUpdate: (id: string) => void;
  onDelete: (id: string) => void;
}

export function RecordList(props: IRecordListProps) {
  const {date, records, visiblePeriod, onUpdate, onDelete} = props;

  const {closeDialog} = useDialog();
  const clients = useBackendClients();

  async function updateRecord(id: string, data: IRecordEditorData) {
    const activities = new UpdateField(data.activityIds);
    const name = new UpdateField(data.name);
    const description = new UpdateField(data.description);
    const tags = new UpdateField(data.tagIds);

    const request = new UpdateRecordRequest(name, description, activities, tags);

    const response = await clients.records.update(id, request);
    await ErrorHelper.throwIfFailed(response);

    closeDialog();
    onUpdate(id);
  }

  async function removeRecord(id: string) {
    const response = await clients.records.remove(id);
    await ErrorHelper.throwIfFailed(response);

    closeDialog();
    onDelete(id);
  }

  return (
      <>
        {records.length > 0 && date &&
            <div className="group-title numeric">{date}</div>}
        {records.map(record =>
            <RecordPanel
                key={record.id}
                record={record}
                visiblePeriod={visiblePeriod}
                onUpdate={data => updateRecord(record.id, data)}
                onDelete={() => removeRecord(record.id)}
                onPeriodsChanged={() => onUpdate(record.id)}/>)}
      </>
  );
}