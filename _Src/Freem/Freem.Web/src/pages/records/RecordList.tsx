import {RecordEntity} from "./entities/RecordEntity.ts";
import {RecordPanel} from "./RecordPanel.tsx";
import {IRecordEditorData} from "./RecordEditor.tsx";
import {UpdateField} from "./clients/models/UpdateField.ts";
import {UpdateRecordRequest} from "./clients/models/records/UpdateRecordRequest.ts";
import {useDialog} from "./contexts/dialog/useDialog.ts";
import {useBackendClients} from "./contexts/clients/useBackendClients.ts";
import {Divider} from "./components/Divider.tsx";

export interface IRecordListProps {
  date?: string;
  records: RecordEntity[];

  onUpdate: (id: string) => void;
  onDelete: (id: string) => void;
}

export function RecordList(props: IRecordListProps) {
  const {date, records, onUpdate, onDelete} = props;

  const {closeDialog} = useDialog();
  const clients = useBackendClients();

  async function updateRecord(id: string, data: IRecordEditorData) {
    const period = new UpdateField(data.period.toString());
    const activities = new UpdateField(data.activityIds);
    const name = new UpdateField(data.name);
    const description = new UpdateField(data.description);
    const tags = new UpdateField(data.tagIds);

    const request = new UpdateRecordRequest(period, name, description, activities, tags);

    const response = await clients.records.update(id, request);

    if (response.ok) {
      closeDialog();
      onUpdate(id);
    } else {
      throw new Error(response.statusText);
    }
  }

  async function removeRecord(id: string) {
    const response = await clients.records.remove(id);

    if (response.ok) {
      closeDialog();
      onDelete(id);
    } else {
      throw new Error(response.statusText);
    }
  }

  return (
      <>
        {records.length > 0 && date &&
            <div>
                <Divider/>
                <span style={{color: "darkgrey", marginLeft: "10px"}}>{date}</span>
                <Divider/>
            </div>}
        {records.map(record =>
            <RecordPanel
                key={record.id}
                record={record}
                onUpdate={data => updateRecord(record.id, data)}
                onDelete={() => removeRecord(record.id)}/>)}
      </>
  );
}