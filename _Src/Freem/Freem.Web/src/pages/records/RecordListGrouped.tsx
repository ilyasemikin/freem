import {RecordEntity} from "../../entities/RecordEntity.ts";
import {useMemo} from "react";
import {RecordList} from "./RecordList.tsx";
import {TimeZoneHelper} from "../../helpers/TimeZoneHelper.ts";
import {TimeZoneOffset} from "../../data/TimeZoneOffset.ts";
import {useUserSettings} from "../../settings/context/useUserSettings.ts";

// ISO date of the user's day: sorts chronologically
function getDayKey(moment: Date, offset: TimeZoneOffset): string {
  const date = TimeZoneHelper.toDate(moment, offset);
  return date.toISOString().slice(0, 10);
}

function group(groupRecords: boolean, records: RecordEntity[], offset: TimeZoneOffset): RecordEntity[][] {
  if (!groupRecords) {
    return [records];
  }

  const groups = new Map<string, RecordEntity[]>();

  for (const record of records) {
    const key = getDayKey(record.startAt, offset);

    if (!groups.has(key)) {
      groups.set(key, []);
    }

    groups.get(key)!.push(record);
  }

  const keys = Array.from(groups.keys()).sort();
  return keys.map(key => groups.get(key)!);
}

export interface IRecordListGroupedProps {
  groupRecords: boolean;
  records: RecordEntity[];

  onUpdate: (id: string) => void;
  onDelete: (id: string) => void;
}

export function RecordListGrouped(props: IRecordListGroupedProps) {
  const {groupRecords, records, onUpdate, onDelete} = props;

  const {timeZoneOffset} = useUserSettings();

  const groups = useMemo(
      () => group(groupRecords, records, timeZoneOffset),
      [groupRecords, records, timeZoneOffset]);

  function render(group: RecordEntity[]) {
    const start = group[0]?.startAt ?? new Date();
    const key = getDayKey(start, timeZoneOffset);
    const day = TimeZoneHelper.toDate(start, timeZoneOffset);
    const date = groupRecords
        ? TimeZoneHelper.formatDate(day)
        : undefined;

    return (
        <RecordList
            key={key}
            date={date}
            records={group}
            onUpdate={onUpdate}
            onDelete={onDelete}/>
    );
  }

  return (
      <>
        {groups.map(group => render(group))}
      </>
  );
}