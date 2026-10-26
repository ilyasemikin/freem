import {useEffect, useState} from "react";
import {RecordEntity} from "../../entities/RecordEntity.ts";
import {useEntitiesCache} from "../../cache/context/useEntitiesCache.ts";
import {ListRecordByPeriodRequest} from "../../clients/models/records/ListRecordByPeriodRequest.ts";
import {Loading} from "../../components/Loading.tsx";
import {ValidationError} from "../../components/ValidationError.tsx";
import {useDialog} from "../../dialog/context/useDialog.ts";
import {PanelHeader} from "../../components/PanelHeader.tsx";
import {RecordAddModal} from "./RecordAddModal.tsx";
import {IRecordEditorData} from "./RecordEditor.tsx";
import {CreateRecordRequest} from "../../clients/models/records/CreateRecordRequest.ts";
import {RecordPeriodRequest} from "../../clients/models/records/RecordPeriodRequest.ts";
import {useBackendClients} from "../../clients/context/useBackendClients.ts";
import {DatePeriodPicker} from "../../components/inputs/DatePeriodPicker.tsx";
import {DateHelper} from "../../helpers/DateHelper.ts";
import {DateUnit} from "../../data/DateUnit.ts";
import {DatePeriod} from "../../data/DatePeriod.ts";
import {Divider} from "../../components/Divider.tsx";
import {RecordListGrouped} from "./RecordListGrouped.tsx";
import {TimeZoneHelper} from "../../helpers/TimeZoneHelper.ts";
import {useUserSettings} from "../../settings/context/useUserSettings.ts";
import {ErrorHelper} from "../../helpers/ErrorHelper.ts";

export interface IRecordsDashboardProps {
  version?: number;
}

export function RecordsDashboard(props: IRecordsDashboardProps) {
  const {version} = props;

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | undefined>();
  const [update, setUpdate] = useState(false);

  const {timeZoneOffset} = useUserSettings();

  const [, setUnit] = useState<DateUnit>(DateUnit.Day);
  const [period, setPeriod] = useState<DatePeriod>(
      () => DateHelper.getPeriod(DateUnit.Day, TimeZoneHelper.today(timeZoneOffset)));
  const [records, setRecords] = useState<RecordEntity[] | undefined>(undefined);

  const {openDialog, closeDialog} = useDialog();
  const clients = useBackendClients();
  const cache = useEntitiesCache();

  useEffect(() => {
    let current = true;

    async function load() {
      const moments = TimeZoneHelper.toDateTimePeriod(period, timeZoneOffset);
      const value = moments.toString();
      const request = new ListRecordByPeriodRequest(value);
      const entities = await cache.records.listByPeriodRequest(request);

      if (current) {
        setRecords(entities);
        setError(undefined);
      }
    }

    load().catch(error => {
      if (current) {
        // Records of the previous period must not stay under the new date
        setRecords(undefined);
        setError(ErrorHelper.getMessage(error, "Failed to load data"));
      }
    }).finally(() => {
      if (current) {
        setLoading(false);
      }
    });

    return () => {
      current = false;
    };
  }, [update, version, period, timeZoneOffset, cache.records]);

  function openAdd() {
    openDialog({
      content: <RecordAddModal
          onAdd={addRecord}
          onCancel={closeDialog}/>
    });
  }

  async function addRecord(data: IRecordEditorData) {
    const periods = data.periods.map(item =>
        new RecordPeriodRequest(item.period.toString(), item.description));
    const request = new CreateRecordRequest(
        periods,
        data.activityIds,
        data.name,
        data.description,
        data.tagIds);

    const response = await clients.records.create(request);
    await ErrorHelper.throwIfFailed(response);

    closeDialog();
    setUpdate(value => !value);
  }

  function changePeriod(unit: DateUnit, value: DatePeriod) {
    setUnit(unit);
    setPeriod(value);
  }

  function reload() {
    setUpdate(value => !value);
  }

  if (loading) {
    return (
        <>
          <PanelHeader title="Records" onAdd={openAdd}/>
          <Loading/>
        </>
    );
  }

  const group = period.getDurationInDays() > 1;
  const visiblePeriod = TimeZoneHelper.toDateTimePeriod(period, timeZoneOffset);

  return (
      <div style={{display: "flex", flexDirection: "column", flex: "1"}}>
        <PanelHeader title="Records" onAdd={openAdd}/>
        <DatePeriodPicker onChange={changePeriod}/>
        <Divider/>
        <ValidationError error={error}/>
        {records?.length === 0 && <div className="empty">No records for this period</div>}
        {records &&
            <RecordListGrouped
                groupRecords={group}
                records={records}
                visiblePeriod={visiblePeriod}
                onUpdate={reload}
                onDelete={reload}/>}
      </div>
  );
}