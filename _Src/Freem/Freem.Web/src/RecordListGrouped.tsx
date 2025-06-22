import {RecordEntity} from "./entities/RecordEntity.ts";
import {DateHelper} from "./helpers/DateHelper.ts";
import {useMemo} from "react";
import {RecordList} from "./RecordList.tsx";

function formatDate(date: Date): string {
  const day = DateHelper.padPart(date.getDate());
  const month = DateHelper.padPart(date.getMonth() + 1);
  const year = date.getFullYear();

  return `${day}/${month}/${year}`;
}

function group(groupRecords: boolean, records: RecordEntity[]): RecordEntity[][] {
  if (!groupRecords) {
    return [records];
  }

  const groups = new Map<string, RecordEntity[]>();

  for (const record of records) {
    const key = formatDate(record.period.startAt);

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

  const groups = useMemo(() => group(groupRecords, records), [groupRecords, records]);

  function render(group: RecordEntity[]) {
    const start = group[0]?.period.startAt ?? new Date();
    const key = formatDate(start);
    const date = groupRecords
        ? key
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