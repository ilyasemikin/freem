import {useEffect, useState} from "react";
import {useLocation, useNavigate, useParams} from "react-router";
import {Button} from "primereact/button";
import {Message} from "primereact/message";
import {AppHeader} from "../../components/AppHeader.tsx";
import {DashboardPanel} from "../../components/DashboardPanel.tsx";
import {PanelHeader} from "../../components/PanelHeader.tsx";
import {ValidationError} from "../../components/ValidationError.tsx";
import {Loading} from "../../components/Loading.tsx";
import {useBackendClients} from "../../clients/context/useBackendClients.ts";
import {ApiError} from "../../clients/http/ApiError.ts";
import {ErrorProperties, ErrorTypes} from "../../clients/models/ErrorTypes.ts";
import {ListResponse} from "../../clients/models/ListResponse.ts";
import {ListFormatsResponse} from "../../clients/models/formats/ListFormatsResponse.ts";
import {ImportResponse} from "../../clients/models/imports/ImportResponse.ts";
import {ImportActivityResponse} from "../../clients/models/imports/ImportActivityResponse.ts";
import {ImportTagResponse} from "../../clients/models/imports/ImportTagResponse.ts";
import {ListImportActivityRequest} from "../../clients/models/imports/ListImportActivityRequest.ts";
import {ListImportTagRequest} from "../../clients/models/imports/ListImportTagRequest.ts";
import {UpdateImportActivityMappingRequest} from "../../clients/models/imports/UpdateImportActivityMappingRequest.ts";
import {UpdateImportTagMappingRequest} from "../../clients/models/imports/UpdateImportTagMappingRequest.ts";
import {ApplyImportRequest} from "../../clients/models/imports/ApplyImportRequest.ts";
import {ApplyImportResponse} from "../../clients/models/imports/ApplyImportResponse.ts";
import {TimeZoneOffset} from "../../data/TimeZoneOffset.ts";
import {IImportPageState} from "./ImportPageState.ts";
import {ImportActivityMappingList} from "./ImportActivityMappingList.tsx";
import {ImportTagMappingList} from "./ImportTagMappingList.tsx";
import {ImportRowsTable} from "./ImportRowsTable.tsx";

const PageSize = 100;

// Activities and tags of an import are few and needed together, so all their pages are read at once.
async function readAllPages<T>(
    property: string,
    readPage: (offset: number, limit: number) => Promise<Response>): Promise<T[]> {
  const items: T[] = [];
  for (; ;) {
    const response = await readPage(items.length, PageSize);
    if (!response.ok) {
      throw await ApiError.fromResponse(response);
    }

    const page = await ListResponse.read<T>(response, property);
    items.push(...page.items);
    if (page.items.length === 0 || items.length >= page.totalCount) {
      return items;
    }
  }
}

function getUnmappedIds(error: ApiError, property: string): Set<string> {
  const ids = (error.body?.details ?? [])
      .map(detail => detail.properties?.[property])
      .filter(id => id !== undefined);
  return new Set(ids);
}

export function ImportPage() {
  const {id} = useParams();
  const importId = id!;
  const location = useLocation();
  const state = location.state as IImportPageState | null;
  const navigate = useNavigate();

  const {imports, dataTransfer} = useBackendClients();

  const [value, setValue] = useState<ImportResponse | undefined>();
  const [formatName, setFormatName] = useState<string | undefined>();
  const [activities, setActivities] = useState<ImportActivityResponse[]>([]);
  const [tags, setTags] = useState<ImportTagResponse[]>([]);
  const [error, setError] = useState<string | undefined>();

  const [selectedIds, setSelectedIds] = useState<Set<string>>(new Set());
  const [unmappedActivityIds, setUnmappedActivityIds] = useState<Set<string>>(new Set());
  const [unmappedTagIds, setUnmappedTagIds] = useState<Set<string>>(new Set());
  const [applying, setApplying] = useState(false);
  const [result, setResult] = useState<ApplyImportResponse | undefined>();
  const [skippedIds, setSkippedIds] = useState<Set<string>>(new Set());
  const [applyError, setApplyError] = useState<string | undefined>();

  useEffect(() => {
    let current = true;

    async function load() {
      const response = await imports.get(importId);
      if (!response.ok) {
        throw await ApiError.fromResponse(response);
      }

      const imported = await response.json();
      const body: ImportResponse = imported.import;
      const loadedActivities = await readAllPages<ImportActivityResponse>("activities", (offset, limit) => {
        const request = new ListImportActivityRequest(offset, limit);
        return imports.listActivities(importId, request);
      });
      const loadedTags = await readAllPages<ImportTagResponse>("tags", (offset, limit) => {
        const request = new ListImportTagRequest(offset, limit);
        return imports.listTags(importId, request);
      });

      const formatsResponse = await dataTransfer.listFormats();
      if (!formatsResponse.ok) {
        throw await ApiError.fromResponse(formatsResponse);
      }

      const formats: ListFormatsResponse = await formatsResponse.json();
      const format = formats.importFormats.find(item => item.key === body.format);

      if (current) {
        setValue(body);
        setFormatName(format?.name ?? body.format);
        setActivities(loadedActivities);
        setTags(loadedTags);
        setError(undefined);
      }
    }

    load().catch(error => {
      if (current) {
        setError(error instanceof Error ? error.message : "Failed to load the import");
      }
    });

    return () => {
      current = false;
    };
  }, [imports, dataTransfer, importId]);

  async function updateActivityMapping(importActivityId: string, activityId: string | null) {
    setApplyError(undefined);
    const request = new UpdateImportActivityMappingRequest(activityId);
    const response = await imports.updateActivityMapping(importId, importActivityId, request);
    if (!response.ok) {
      const error = await ApiError.fromResponse(response);
      setApplyError(error.message);
      return;
    }

    setActivities(items => items.map(item => item.id === importActivityId
        ? {...item, mappedActivityId: activityId}
        : item));
    setUnmappedActivityIds(ids => {
      const next = new Set(ids);
      next.delete(importActivityId);
      return next;
    });
  }

  async function updateTagMapping(importTagId: string, tagId: string | null) {
    setApplyError(undefined);
    const request = new UpdateImportTagMappingRequest(tagId);
    const response = await imports.updateTagMapping(importId, importTagId, request);
    if (!response.ok) {
      const error = await ApiError.fromResponse(response);
      setApplyError(error.message);
      return;
    }

    setTags(items => items.map(item => item.id === importTagId
        ? {...item, mappedTagId: tagId}
        : item));
    setUnmappedTagIds(ids => {
      const next = new Set(ids);
      next.delete(importTagId);
      return next;
    });
  }

  function changeSelection(ids: string[], selected: boolean) {
    setSelectedIds(current => {
      const next = new Set(current);
      for (const id of ids) {
        if (selected) {
          next.add(id);
        } else {
          next.delete(id);
        }
      }

      return next;
    });
  }

  function showApplyError(error: ApiError) {
    if (error.type === ErrorTypes.ImportUnmappedEntities) {
      const activityIds = getUnmappedIds(error, ErrorProperties.ImportActivityId);
      const tagIds = getUnmappedIds(error, ErrorProperties.ImportTagId);
      setUnmappedActivityIds(activityIds);
      setUnmappedTagIds(tagIds);
      setApplyError(`Map the highlighted activities and tags first: ${activityIds.size + tagIds.size} not mapped`);
      return;
    }

    if (error.status === 409) {
      setApplyError("The import is busy, try again in a moment");
      return;
    }

    setApplyError(error.message);
  }

  async function apply(request: ApplyImportRequest) {
    setApplying(true);
    setApplyError(undefined);
    setResult(undefined);
    try {
      const response = await imports.apply(importId, request);
      if (!response.ok) {
        showApplyError(await ApiError.fromResponse(response));
        return;
      }

      const body: ApplyImportResponse = await response.json();
      setResult(body);
      setSkippedIds(new Set(body.skippedRowIds));
      setSelectedIds(new Set());
    } catch (e) {
      setApplyError(e instanceof Error ? e.message : "Failed to save the rows");
    } finally {
      setApplying(false);
    }
  }

  function renderResult() {
    if (result === undefined) {
      return null;
    }

    const skipped = result.skippedRowIds.length;
    const text = skipped === 0
        ? `Created ${result.recordsCreated} records`
        : `Created ${result.recordsCreated} records, skipped ${skipped} rows: zero duration, overlapping periods or too many activities or tags`;
    return (<Message severity={skipped === 0 ? "success" : "warn"} text={text} style={{justifyContent: "flex-start"}}/>);
  }

  function renderWarnings() {
    const warnings = state?.warnings ?? [];
    if (warnings.length === 0) {
      return null;
    }

    return (
        <>
          <h3 className="group-title">Upload warnings: {warnings.length}</h3>
          <div style={{maxHeight: "160px", overflowY: "auto", fontSize: "13px"}}>
            {warnings.map((warning, index) =>
                <div key={index}><span className="muted numeric">Line {warning.lineNumber}:</span> {warning.message}</div>)}
          </div>
        </>
    );
  }

  if (value === undefined) {
    return (
        <div style={{display: "flex", flexDirection: "column", height: "100%"}}>
          <AppHeader/>
          <div className="dashboard">
            <DashboardPanel>
              <PanelHeader title="Import"/>
              {error !== undefined ? <ValidationError error={error}/> : <Loading/>}
              <Button text size="small" severity="secondary" label="Back" onClick={() => navigate("/imports")}/>
            </DashboardPanel>
          </div>
        </div>
    );
  }

  const offset = TimeZoneOffset.parse(value.timeZoneOffset);

  return (
      <div style={{display: "flex", flexDirection: "column", height: "100%"}}>
        <AppHeader/>
        <div className="dashboard">
          <DashboardPanel style={{maxWidth: "480px"}}>
            <PanelHeader title="Import"/>
            <div className="item-title" style={{overflowWrap: "anywhere"}}>{value.fileName}</div>
            <div className="item-subtitle">
              {formatName}, {offset.toDisplayString()}, rows parsed {value.rowsParsed} of {value.rowsInFile}
            </div>
            {renderWarnings()}
            <ImportActivityMappingList
                activities={activities}
                unmappedIds={unmappedActivityIds}
                onChange={updateActivityMapping}/>
            {tags.length > 0 &&
                <ImportTagMappingList
                    tags={tags}
                    unmappedIds={unmappedTagIds}
                    onChange={updateTagMapping}/>}
            <div style={{marginTop: "12px"}}>
              <Button text size="small" severity="secondary" label="Back" onClick={() => navigate("/imports")}/>
            </div>
          </DashboardPanel>
          <DashboardPanel>
            <PanelHeader title="Rows"/>
            <div style={{display: "flex", flexDirection: "row", flexWrap: "wrap", gap: "6px", marginBottom: "8px"}}>
              <Button size="small" label={`Save all rows (${value.rowsParsed})`} loading={applying}
                      disabled={value.rowsParsed === 0} onClick={() => apply(ApplyImportRequest.allRows())}/>
              <Button size="small" outlined label={`Save selected (${selectedIds.size})`} disabled={applying || selectedIds.size === 0}
                      onClick={() => apply(ApplyImportRequest.selectedRows([...selectedIds]))}/>
              <Button size="small" text severity="secondary" label="Clear selection" disabled={applying || selectedIds.size === 0}
                      onClick={() => setSelectedIds(new Set())}/>
            </div>
            <ValidationError error={applyError}/>
            {renderResult()}
            <ImportRowsTable
                importId={importId}
                activities={activities}
                tags={tags}
                selectedIds={selectedIds}
                skippedIds={skippedIds}
                onSelectionChange={changeSelection}/>
          </DashboardPanel>
        </div>
      </div>
  );
}
