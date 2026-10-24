import {ChangeEvent, useMemo, useRef, useState} from "react";
import {useNavigate} from "react-router";
import {Button} from "primereact/button";
import {Dropdown} from "primereact/dropdown";
import {PanelHeader} from "../../components/PanelHeader.tsx";
import {ValidationError} from "../../components/ValidationError.tsx";
import {useBackendClients} from "../../clients/context/useBackendClients.ts";
import {useUserSettings} from "../../settings/context/useUserSettings.ts";
import {ApiError} from "../../clients/http/ApiError.ts";
import {TimeZoneOffset} from "../../data/TimeZoneOffset.ts";
import {FormatResponse} from "../../clients/models/formats/FormatResponse.ts";
import {UploadImportRequest} from "../../clients/models/imports/UploadImportRequest.ts";
import {UploadImportResponse} from "../../clients/models/imports/UploadImportResponse.ts";
import {IImportPageState} from "./ImportPageState.ts";

export interface IImportUploadPanelProps {
  formats?: FormatResponse[];
}

export function ImportUploadPanel(props: IImportUploadPanelProps) {
  const {formats} = props;

  const {imports} = useBackendClients();
  const {timeZoneOffset} = useUserSettings();
  const navigate = useNavigate();

  const inputRef = useRef<HTMLInputElement>(null);
  const [format, setFormat] = useState<string | undefined>();
  const [offset, setOffset] = useState<number>(timeZoneOffset.minutes);
  const [file, setFile] = useState<File | undefined>();
  const [uploading, setUploading] = useState(false);
  const [error, setError] = useState<string | undefined>();

  const formatOptions = formats?.map(item => ({label: item.name, value: item.key})) ?? [];
  const offsetOptions = useMemo(
      () => TimeZoneOffset.all().map(item => ({label: item.toDisplayString(), value: item.minutes})),
      []);

  const selectedFormat = format ?? formats?.[0]?.key;
  const extension = formats?.find(item => item.key === selectedFormat)?.fileExtension;

  function chooseFile(e: ChangeEvent<HTMLInputElement>) {
    setFile(e.target.files?.[0]);
    setError(undefined);
  }

  async function upload() {
    if (selectedFormat === undefined || file === undefined) {
      return;
    }

    setUploading(true);
    setError(undefined);
    try {
      const value = new TimeZoneOffset(offset);
      const request = new UploadImportRequest(selectedFormat, file, value.toString());
      const response = await imports.upload(request);
      if (!response.ok) {
        throw await ApiError.fromResponse(response);
      }

      const body: UploadImportResponse = await response.json();
      const state: IImportPageState = {warnings: body.warnings};
      navigate(`/imports/${body.import.id}`, {state: state});
    } catch (e) {
      setError(e instanceof Error ? e.message : "Failed to upload the file");
      setUploading(false);
    }
  }

  return (
      <>
        <PanelHeader title="Upload"/>
        <div style={{display: "flex", flexDirection: "column", gap: "8px"}}>
          <label htmlFor="import-format" className="muted" style={{fontSize: "13px"}}>Format</label>
          <Dropdown
              inputId="import-format"
              value={selectedFormat}
              options={formatOptions}
              disabled={formats === undefined || uploading}
              onChange={e => setFormat(e.value)}/>
          <label htmlFor="import-time-zone-offset" className="muted" style={{fontSize: "13px"}}>
            Time zone: times without an offset in the file are read in it
          </label>
          <Dropdown
              inputId="import-time-zone-offset"
              value={offset}
              options={offsetOptions}
              disabled={uploading}
              onChange={e => setOffset(e.value)}/>
          <input
              ref={inputRef}
              type="file"
              accept={extension !== undefined ? `.${extension}` : undefined}
              style={{display: "none"}}
              onChange={chooseFile}/>
          <div style={{display: "flex", flexDirection: "row", gap: "8px", alignItems: "center"}}>
            <Button size="small" outlined icon="pi pi-file" label="Choose file" disabled={uploading}
                    onClick={() => inputRef.current?.click()}/>
            <span className="muted" style={{fontSize: "13px", overflowWrap: "anywhere"}}>
              {file?.name ?? "No file chosen"}
            </span>
          </div>
          <ValidationError error={error}/>
          <div style={{display: "flex", flexDirection: "row", justifyContent: "flex-end"}}>
            <Button size="small" icon="pi pi-upload" label="Upload" loading={uploading}
                    disabled={selectedFormat === undefined || file === undefined} onClick={upload}/>
          </div>
        </div>
      </>
  );
}
