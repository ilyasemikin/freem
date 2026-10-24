import {useEffect, useState} from "react";
import {useNavigate} from "react-router";
import {Button} from "primereact/button";
import {Column} from "primereact/column";
import {DataTable, DataTablePageEvent} from "primereact/datatable";
import {PanelHeader} from "../../components/PanelHeader.tsx";
import {ValidationError} from "../../components/ValidationError.tsx";
import {useBackendClients} from "../../clients/context/useBackendClients.ts";
import {useDialog} from "../../dialog/context/useDialog.ts";
import {ApiError} from "../../clients/http/ApiError.ts";
import {ListResponse} from "../../clients/models/ListResponse.ts";
import {FormatResponse} from "../../clients/models/formats/FormatResponse.ts";
import {ImportResponse} from "../../clients/models/imports/ImportResponse.ts";
import {ListImportRequest} from "../../clients/models/imports/ListImportRequest.ts";
import {TimeZoneOffset} from "../../data/TimeZoneOffset.ts";
import {ImportDeleteModal} from "./ImportDeleteModal.tsx";

const PageSize = 20;

// DataTable memoizes cells by row data, and the formats load separately, so the format name is put into it.
interface IImportView {
  value: ImportResponse;
  formatName: string;
}

export interface IImportListProps {
  formats?: FormatResponse[];
}

export function ImportList(props: IImportListProps) {
  const {formats} = props;

  const {imports} = useBackendClients();
  const {openDialog, closeDialog} = useDialog();
  const navigate = useNavigate();

  const [first, setFirst] = useState(0);
  const [page, setPage] = useState<ListResponse<ImportResponse> | undefined>();
  const [loading, setLoading] = useState(true);
  const [update, setUpdate] = useState(false);
  const [error, setError] = useState<string | undefined>();

  useEffect(() => {
    let current = true;

    async function load() {
      setLoading(true);
      const request = new ListImportRequest(first, PageSize);
      const response = await imports.list(request);
      if (!response.ok) {
        throw await ApiError.fromResponse(response);
      }

      const value = await ListResponse.read<ImportResponse>(response);
      if (current) {
        setPage(value);
        setError(undefined);
      }
    }

    load().catch(error => {
      if (current) {
        setError(error instanceof Error ? error.message : "Failed to load imports");
      }
    }).finally(() => {
      if (current) {
        setLoading(false);
      }
    });

    return () => {
      current = false;
    };
  }, [imports, first, update]);

  function changePage(e: DataTablePageEvent) {
    setFirst(e.first);
  }

  async function remove(item: ImportResponse) {
    const response = await imports.remove(item.id);
    if (!response.ok && response.status !== 404) {
      throw await ApiError.fromResponse(response);
    }

    closeDialog();
    setUpdate(value => !value);
  }

  function openDelete(item: ImportResponse) {
    openDialog({
      content: <ImportDeleteModal
          fileName={item.fileName}
          onDelete={() => remove(item)}
          onCancel={closeDialog}/>
    });
  }

  const views = (page?.items ?? []).map<IImportView>(item => ({
    value: item,
    formatName: formats?.find(format => format.key === item.format)?.name ?? item.format
  }));

  function fileTemplate(view: IImportView) {
    return view.value.fileName;
  }

  function formatTemplate(view: IImportView) {
    return view.formatName;
  }

  function timeZoneTemplate(view: IImportView) {
    return TimeZoneOffset.parse(view.value.timeZoneOffset).toDisplayString();
  }

  function rowsTemplate(view: IImportView) {
    const item = view.value;
    return item.rowsParsed === item.rowsInFile
        ? `${item.rowsParsed}`
        : `${item.rowsParsed} of ${item.rowsInFile}`;
  }

  function actionsTemplate(view: IImportView) {
    const item = view.value;
    return (
        <div style={{display: "flex", gap: "4px", justifyContent: "flex-end"}}>
          <Button text rounded size="small" icon="pi pi-arrow-right" aria-label="Open import"
                  onClick={() => navigate(`/imports/${item.id}`)}/>
          <Button text rounded size="small" severity="danger" icon="pi pi-trash" aria-label="Delete import"
                  onClick={() => openDelete(item)}/>
        </div>
    );
  }

  return (
      <>
        <PanelHeader title="Imports"/>
        <ValidationError error={error}/>
        <DataTable
            value={views}
            dataKey="value.id"
            size="small"
            lazy
            paginator
            first={first}
            rows={PageSize}
            totalRecords={page?.totalCount ?? 0}
            loading={loading}
            emptyMessage="No imports yet"
            onPage={changePage}>
          <Column header="File" body={fileTemplate}/>
          <Column header="Format" body={formatTemplate}/>
          <Column header="Time zone" body={timeZoneTemplate}/>
          <Column header="Rows" body={rowsTemplate}/>
          <Column body={actionsTemplate}/>
        </DataTable>
      </>
  );
}
