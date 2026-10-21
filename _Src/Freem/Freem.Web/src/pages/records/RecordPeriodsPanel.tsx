import {useEffect, useRef, useState} from "react";
import {RecordEntity} from "../../entities/RecordEntity.ts";
import {useBackendClients} from "../../clients/context/useBackendClients.ts";
import {CustomButton} from "../../components/inputs/CustomButton.tsx";
import {ValidationError} from "../../components/ValidationError.tsx";
import {UpdateField} from "../../clients/models/UpdateField.ts";
import {RecordPeriodRequest} from "../../clients/models/records/RecordPeriodRequest.ts";
import {UpdateRecordPeriodRequest} from "../../clients/models/records/UpdateRecordPeriodRequest.ts";
import {AddRecordPeriodResponse} from "../../clients/models/records/AddRecordPeriodResponse.ts";
import {IPeriodRowState, RecordPeriodsHelper} from "../../helpers/RecordPeriodsHelper.ts";
import {RecordPeriodRow} from "./RecordPeriodRow.tsx";

export interface IRecordPeriodsPanelProps {
  record: RecordEntity;

  // Called after a period of the record was added, changed or removed
  onChanged?: () => void;

  // Called when the presence of unsaved period changes changes
  onDirtyChange?: (dirty: boolean) => void;
}

async function readError(response: Response): Promise<string> {
  try {
    const body = await response.json();
    const message = body?.error?.message ?? body?.message;
    if (typeof message === "string") {
      return message;
    }
  } catch {
    // The body is not JSON: fall back to the status text
  }

  return response.statusText || `Request failed (${response.status})`;
}

function createRows(source: RecordEntity): IPeriodRowState[] {
  return source.periods.map(item => ({
    key: item.id,
    id: item.id,
    startAt: item.period.startAt,
    endAt: item.period.endAt,
    description: item.description || ""
  }));
}

function isSame(left: IPeriodRowState, right: IPeriodRowState): boolean {
  return left.startAt?.getTime() === right.startAt?.getTime()
      && left.endAt?.getTime() === right.endAt?.getTime()
      && left.description === right.description;
}

// Every action calls its own API operation immediately, so periods are saved separately from the other record fields
export function RecordPeriodsPanel(props: IRecordPeriodsPanelProps) {
  const {record, onChanged, onDirtyChange} = props;

  const clients = useBackendClients();

  const [rows, setRows] = useState<IPeriodRowState[]>(() => createRows(record));
  // Last state of the periods known to the server: overlaps are checked against it, not against unsaved edits
  const [saved, setSaved] = useState<IPeriodRowState[]>(() => createRows(record));
  const [errors, setErrors] = useState<Record<string, string>>({});
  const [busy, setBusy] = useState(false);

  // The state is not updated synchronously, so double clicks are stopped by the ref
  const busyRef = useRef(false);

  const dirty = rows.some(row => {
    const savedRow = saved.find(item => item.key === row.key);
    return savedRow === undefined || !isSame(row, savedRow);
  });

  useEffect(() => {
    onDirtyChange?.(dirty);
  }, [dirty, onDirtyChange]);

  function setError(key: string, error?: string) {
    setErrors(value => {
      const newValue = {...value};
      if (error === undefined) {
        delete newValue[key];
      } else {
        newValue[key] = error;
      }

      return newValue;
    });
  }

  function changeRow(row: IPeriodRowState) {
    setRows(value => value.map(item => item.key === row.key ? row : item));
    setError(row.key);
  }

  function addRow() {
    if (busyRef.current) {
      return;
    }

    const last = rows[rows.length - 1];
    setRows(value => [...value, RecordPeriodsHelper.createRow(last?.endAt)]);
  }

  async function perform(row: IPeriodRowState, action: () => Promise<void>, failureMessage: string) {
    if (busyRef.current) {
      return;
    }

    busyRef.current = true;
    setBusy(true);
    try {
      await action();
    } catch (error) {
      setError(row.key, error instanceof Error ? error.message : failureMessage);
    } finally {
      busyRef.current = false;
      setBusy(false);
    }
  }

  async function save(row: IPeriodRowState) {
    const validationError = RecordPeriodsHelper.validateRow(row, saved);
    if (validationError !== undefined) {
      setError(row.key, validationError);
      return;
    }

    const data = RecordPeriodsHelper.toRowData(row);

    await perform(row, async () => {
      if (row.id === undefined) {
        const request = new RecordPeriodRequest(data.period.toString(), data.description);
        const response = await clients.records.addPeriod(record.id, request);
        if (!response.ok) {
          setError(row.key, await readError(response));
          return;
        }

        const added: AddRecordPeriodResponse = await response.json();
        const addedRow = {...row, id: added.id};
        setRows(value => value.map(item => item.key === row.key ? addedRow : item));
        setSaved(value => [...value, addedRow]);
      } else {
        const request = new UpdateRecordPeriodRequest(
            new UpdateField(data.period.toString()),
            new UpdateField(data.description));
        const response = await clients.records.updatePeriod(record.id, row.id, request);
        if (!response.ok) {
          setError(row.key, await readError(response));
          return;
        }

        setSaved(value => value.map(item => item.key === row.key ? row : item));
      }

      setError(row.key);
      onChanged?.();
    }, "Failed to save the period");
  }

  async function remove(row: IPeriodRowState) {
    if (row.id === undefined) {
      setRows(value => value.filter(item => item.key !== row.key));
      setError(row.key);
      return;
    }

    const rowId = row.id;

    await perform(row, async () => {
      const response = await clients.records.removePeriod(record.id, rowId);
      if (!response.ok) {
        setError(row.key, await readError(response));
        return;
      }

      setRows(value => value.filter(item => item.key !== row.key));
      setSaved(value => value.filter(item => item.key !== row.key));
      setError(row.key);
      onChanged?.();
    }, "Failed to remove the period");
  }

  function renderActions(row: IPeriodRowState) {
    // The only saved period of a record cannot be removed; a new row that is not saved yet can always be dropped
    const removable = row.id === undefined || saved.length > 1;

    return (
        <div style={{display: "flex", flexDirection: "row", gap: "6px"}}>
          <CustomButton label="Save period" onClick={() => save(row)}>
            <span style={{fontSize: "18px"}} className="pi pi-check"/>
          </CustomButton>
          {removable &&
              <CustomButton label="Remove period" onClick={() => remove(row)}>
                <span style={{fontSize: "18px"}} className="pi pi-trash"/>
              </CustomButton>}
        </div>
    );
  }

  return (
      <div style={{display: "flex", flexDirection: "column", marginTop: "8px"}}>
        <span className="item-subtitle" style={{marginBottom: "6px"}}>
          Periods are saved separately from the other fields
        </span>
        {rows.map(row =>
            <RecordPeriodRow
                key={row.key}
                row={row}
                loading={busy}
                error={errors[row.key]}
                actions={renderActions(row)}
                onChange={changeRow}/>)}
        <ValidationError error={dirty ? "There are unsaved period changes" : undefined}/>
        <CustomButton onClick={addRow}>
          <span style={{marginRight: "5px"}} className="pi pi-plus"/>
          <span>Add period</span>
        </CustomButton>
      </div>
  );
}
