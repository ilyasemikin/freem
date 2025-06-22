import {useEffect, useState} from "react";
import {RecordEntity} from "./entities/RecordEntity.ts";
import {useEntitiesCache} from "./contexts/cache/useEntitiesCache.ts";
import {ListRecordByPeriodRequest} from "./clients/models/records/ListRecordByPeriodRequest.ts";
import {Loading} from "./components/Loading.tsx";
import {useDialog} from "./contexts/dialog/useDialog.ts";
import {CustomButton} from "./components/inputs/CustomButton.tsx";
import {RecordAddModal} from "./RecordAddModal.tsx";
import {IRecordEditorData} from "./RecordEditor.tsx";
import {CreateRecordRequest} from "./clients/models/records/CreateRecordRequest.ts";
import {useBackendClients} from "./contexts/clients/useBackendClients.ts";
import {DatePeriodPicker} from "./components/inputs/DatePeriodPicker.tsx";
import {DateHelper} from "./helpers/DateHelper.ts";
import {DateUnit} from "./data/DateUnit.ts";
import {DatePeriod} from "./data/DatePeriod.ts";
import {Divider} from "./components/Divider.tsx";
import {RecordListGrouped} from "./RecordListGrouped.tsx";

export function RecordsDashboard() {
  const [loading, setLoading] = useState(true);
  const [update, setUpdate] = useState(false);

  const [, setUnit] = useState<DateUnit>(DateUnit.Day);
  const [period, setPeriod] = useState<DatePeriod>(DateHelper.getPeriod(DateUnit.Day, new Date()));
  const [records, setRecords] = useState<RecordEntity[] | undefined>(undefined);

  const {openDialog, closeDialog} = useDialog();
  const clients = useBackendClients();
  const cache = useEntitiesCache();

  useEffect(() => {
    async function loadRecords() {
      const request = new ListRecordByPeriodRequest(period.toString())
      const entities = await cache.records.listByPeriodRequest(request);

      setRecords(entities);
    }

    setLoading(true);
    loadRecords();
    setLoading(false);
  }, [update, period]);

  function openAdd() {
    openDialog({
      content: <RecordAddModal
          onAdd={addRecord}
          onCancel={closeDialog}/>
    });
  }

  async function addRecord(data: IRecordEditorData) {
    const request = new CreateRecordRequest(
        data.period.toString(),
        data.activityIds,
        data.name,
        data.description,
        data.tagIds);

    const response = await clients.records.create(request);

    if (response.ok) {
      closeDialog();
      setUpdate(value => !value);
    } else {
      throw new Error(response.statusText)
    }
  }

  function changePeriod(unit: DateUnit, value: DatePeriod) {
    setUnit(unit);
    setPeriod(value);
  }

  function reload() {
    setUpdate(value => !value);
  }

  if (loading) {
    return <Loading/>;
  }

  const group = period.getDurationInDays() > 1;

  return (
      <div style={{display: "flex", flexDirection: "column", flex: "1"}}>
        <DatePeriodPicker onChange={changePeriod}/>
        <div style={{display: "flex", justifyContent: "end"}}>
          <CustomButton onClick={openAdd}>
            <span className="pi pi-plus"/>
          </CustomButton>
        </div>
        <Divider/>
        {records?.length === 0 && <span>No records</span>}
        {records &&
            <RecordListGrouped
                groupRecords={group}
                records={records}
                onUpdate={reload}
                onDelete={reload}/>}
      </div>
  );
}