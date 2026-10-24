import {useEffect, useState} from "react";
import {Checkbox} from "primereact/checkbox";
import {Column} from "primereact/column";
import {DataTable, DataTablePageEvent} from "primereact/datatable";
import {Tag} from "primereact/tag";
import {ValidationError} from "../../components/ValidationError.tsx";
import {ColorDot} from "../../components/ColorDot.tsx";
import {useBackendClients} from "../../clients/context/useBackendClients.ts";
import {useUserSettings} from "../../settings/context/useUserSettings.ts";
import {ApiError} from "../../clients/http/ApiError.ts";
import {ListResponse} from "../../clients/models/ListResponse.ts";
import {ImportActivityResponse} from "../../clients/models/imports/ImportActivityResponse.ts";
import {ImportTagResponse} from "../../clients/models/imports/ImportTagResponse.ts";
import {ImportRowResponse} from "../../clients/models/imports/ImportRowResponse.ts";
import {ListImportRowRequest} from "../../clients/models/imports/ListImportRowRequest.ts";
import {DateTimePeriod} from "../../data/DateTimePeriod.ts";
import {TimeZoneHelper} from "../../helpers/TimeZoneHelper.ts";

export interface IImportRowsTableProps {
  importId: string;
  activities: ImportActivityResponse[];
  tags: ImportTagResponse[];
  selectedIds: Set<string>;
  skippedIds: Set<string>;
  // Memoized cells keep old callbacks, so the change is passed as ids, not as a new selection.
  onSelectionChange: (ids: string[], selected: boolean) => void;
}

// DataTable memoizes cells by row data, so the selection and the skip mark are put into it.
interface IImportRowView {
  row: ImportRowResponse;
  selected: boolean;
  skipped: boolean;
}

// Rows are paged on the server; the selection is kept by id across pages.
export function ImportRowsTable(props: IImportRowsTableProps) {
  const {importId, activities, tags, selectedIds, skippedIds, onSelectionChange} = props;

  const {imports} = useBackendClients();
  const {timeZoneOffset} = useUserSettings();

  const [first, setFirst] = useState(0);
  const [rows, setRows] = useState(50);
  const [page, setPage] = useState<ListResponse<ImportRowResponse> | undefined>();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | undefined>();

  useEffect(() => {
    let current = true;

    async function load() {
      setLoading(true);
      const request = new ListImportRowRequest(first, rows);
      const response = await imports.listRows(importId, request);
      if (!response.ok) {
        throw await ApiError.fromResponse(response);
      }

      const value = await ListResponse.read<ImportRowResponse>(response);
      if (current) {
        setPage(value);
        setError(undefined);
      }
    }

    load().catch(error => {
      if (current) {
        setError(error instanceof Error ? error.message : "Failed to load rows");
      }
    }).finally(() => {
      if (current) {
        setLoading(false);
      }
    });

    return () => {
      current = false;
    };
  }, [imports, importId, first, rows]);

  const items = page?.items ?? [];
  const views = items.map<IImportRowView>(item => ({
    row: item,
    selected: selectedIds.has(item.id),
    skipped: skippedIds.has(item.id)
  }));
  const pageSelected = items.length > 0 && items.every(item => selectedIds.has(item.id));

  function changePage(e: DataTablePageEvent) {
    setFirst(e.first);
    setRows(e.rows);
  }

  function toggle(id: string, checked: boolean) {
    onSelectionChange([id], checked);
  }

  function togglePage(checked: boolean) {
    const ids = items.map(item => item.id);
    onSelectionChange(ids, checked);
  }

  function selectionHeader() {
    return (
        <Checkbox checked={pageSelected} aria-label="Select rows on the page"
                  onChange={e => togglePage(e.checked ?? false)}/>
    );
  }

  function selectionTemplate(view: IImportRowView) {
    return (
        <Checkbox checked={view.selected} aria-label="Select row"
                  onChange={e => toggle(view.row.id, e.checked ?? false)}/>
    );
  }

  function periodsTemplate(view: IImportRowView) {
    const item = view.row;
    return (
        <div style={{display: "flex", flexDirection: "column"}} className="numeric">
          {item.periods.map((value, index) => {
            const period = DateTimePeriod.parse(value.period);
            const date = TimeZoneHelper.toDate(period.startAt, timeZoneOffset);
            const range = TimeZoneHelper.formatRange(period.startAt, period.endAt, timeZoneOffset);
            return (<span key={index}>{TimeZoneHelper.formatDate(date)} {range}</span>);
          })}
        </div>
    );
  }

  function activitiesTemplate(view: IImportRowView) {
    const item = view.row;
    return (
        <div style={{display: "flex", flexDirection: "column"}}>
          {item.activityIds.map(id => {
            const activity = activities.find(value => value.id === id);
            return (
                <span key={id}>
                  {activity !== undefined && activity.color !== null && <ColorDot color={activity.color}/>}
                  {activity?.name ?? id}
                </span>
            );
          })}
        </div>
    );
  }

  function nameTemplate(view: IImportRowView) {
    const item = view.row;
    const names = item.tagIds.map(id => tags.find(value => value.id === id)?.name ?? id);
    return (
        <div style={{display: "flex", flexDirection: "column"}}>
          {item.name !== null && <span>{item.name}</span>}
          {item.description !== null && <span className="item-subtitle">{item.description}</span>}
          {names.length > 0 && <span className="item-subtitle">#{names.join(" #")}</span>}
        </div>
    );
  }

  function stateTemplate(view: IImportRowView) {
    return view.skipped
        ? <Tag severity="warning" value="Skipped"/>
        : null;
  }

  return (
      <>
        <ValidationError error={error}/>
        <DataTable
            value={views}
            dataKey="row.id"
            size="small"
            lazy
            paginator
            first={first}
            rows={rows}
            rowsPerPageOptions={[50, 100, 500]}
            totalRecords={page?.totalCount ?? 0}
            loading={loading}
            emptyMessage="No rows"
            onPage={changePage}>
          <Column header={selectionHeader} body={selectionTemplate} style={{width: "3rem"}}/>
          <Column header="Periods" body={periodsTemplate}/>
          <Column header="Activities" body={activitiesTemplate}/>
          <Column header="Name" body={nameTemplate}/>
          <Column body={stateTemplate}/>
        </DataTable>
      </>
  );
}
