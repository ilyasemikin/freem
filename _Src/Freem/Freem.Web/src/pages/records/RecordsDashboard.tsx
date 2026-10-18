import {useEffect, useState} from "react";
import {RecordEntity} from "../../entities/RecordEntity.ts";
import {useEntitiesCache} from "../../cache/context/useEntitiesCache.ts";
import {ListRecordByPeriodRequest} from "../../clients/models/records/ListRecordByPeriodRequest.ts";
import {Loading} from "../../components/Loading.tsx";
import {ValidationError} from "../../components/ValidationError.tsx";
import {useDialog} from "../../dialog/context/useDialog.ts";
import {CustomButton} from "../../components/inputs/CustomButton.tsx";
import {RecordAddModal} from "./RecordAddModal.tsx";
import {IRecordEditorData} from "./RecordEditor.tsx";
import {CreateRecordRequest} from "../../clients/models/records/CreateRecordRequest.ts";
import {useBackendClients} from "../../clients/context/useBackendClients.ts";
import {DatePeriodPicker} from "../../components/inputs/DatePeriodPicker.tsx";
import {DateHelper} from "../../helpers/DateHelper.ts";
import {DateUnit} from "../../data/DateUnit.ts";
import {DatePeriod} from "../../data/DatePeriod.ts";
import {Divider} from "../../components/Divider.tsx";
import {RecordListGrouped} from "./RecordListGrouped.tsx";

export function RecordsDashboard() {
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | undefined>();
  const [update, setUpdate] = useState(false);

  const [, setUnit] = useState<DateUnit>(DateUnit.Day);
  const [period, setPeriod] = useState<DatePeriod>(DateHelper.getPeriod(DateUnit.Day, new Date()));
  const [records, setRecords] = useState<RecordEntity[] | undefined>(undefined);

  const {openDialog, closeDialog} = useDialog();
  const clients = useBackendClients();
  const cache = useEntitiesCache();

  useEffect(() => {
    let current = true;

    async function load() {
      const request = new ListRecordByPeriodRequest(period.toString())
      const entities = await cache.records.listByPeriodRequest(request);

      if (current) {
        setRecords(entities);
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
  }, [update, period, cache.records]);

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
        <ValidationError error={error}/>
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