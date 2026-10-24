import {useEffect, useState} from "react";
import {useNavigate} from "react-router";
import {Button} from "primereact/button";
import {Dropdown, DropdownChangeEvent} from "primereact/dropdown";
import {AppHeader} from "../../components/AppHeader.tsx";
import {DashboardPanel} from "../../components/DashboardPanel.tsx";
import {PanelHeader} from "../../components/PanelHeader.tsx";
import {ValidationError} from "../../components/ValidationError.tsx";
import {Loading} from "../../components/Loading.tsx";
import {useBackendClients} from "../../clients/context/useBackendClients.ts";
import {ApiError} from "../../clients/http/ApiError.ts";
import {FormatResponse} from "../../clients/models/formats/FormatResponse.ts";
import {ListFormatsResponse} from "../../clients/models/formats/ListFormatsResponse.ts";
import {FileDownloadHelper} from "../../helpers/FileDownloadHelper.ts";

export function ExportPage() {
  const {dataTransfer} = useBackendClients();
  const navigate = useNavigate();

  const [formats, setFormats] = useState<FormatResponse[] | undefined>();
  const [selected, setSelected] = useState<string | undefined>();
  const [exporting, setExporting] = useState(false);
  const [error, setError] = useState<string | undefined>();

  useEffect(() => {
    let current = true;

    async function load() {
      const response = await dataTransfer.listFormats();
      if (!response.ok) {
        throw await ApiError.fromResponse(response);
      }

      const body: ListFormatsResponse = await response.json();
      if (current) {
        setFormats(body.exportFormats);
        setSelected(body.exportFormats[0]?.key);
      }
    }

    load().catch(error => {
      if (current) {
        setError(error instanceof Error ? error.message : "Failed to load formats");
      }
    });

    return () => {
      current = false;
    };
  }, [dataTransfer]);

  function change(e: DropdownChangeEvent) {
    setSelected(e.value);
    setError(undefined);
  }

  async function download() {
    const format = formats?.find(item => item.key === selected);
    if (format === undefined) {
      return;
    }

    setExporting(true);
    setError(undefined);
    try {
      const response = await dataTransfer.export(format.key);
      if (!response.ok) {
        throw await ApiError.fromResponse(response);
      }

      const blob = await response.blob();
      const header = response.headers.get("Content-Disposition");
      const fileName = FileDownloadHelper.getFileName(header) ?? `freem-export.${format.fileExtension}`;
      FileDownloadHelper.save(blob, fileName);
    } catch (e) {
      setError(e instanceof Error ? e.message : "Failed to export data");
    } finally {
      setExporting(false);
    }
  }

  const options = formats?.map(item => ({label: item.name, value: item.key})) ?? [];

  return (
      <div style={{display: "flex", flexDirection: "column", height: "100%"}}>
        <AppHeader/>
        <div className="dashboard">
          <DashboardPanel style={{maxWidth: "480px", margin: "0 auto"}}>
            <PanelHeader title="Export"/>
            {formats === undefined && error === undefined && <Loading/>}
            <div style={{display: "flex", flexDirection: "column", gap: "8px"}}>
              <label htmlFor="export-format" className="muted" style={{fontSize: "13px"}}>
                Format: all tags, activities and records are written to one file
              </label>
              <Dropdown
                  inputId="export-format"
                  value={selected}
                  options={options}
                  disabled={formats === undefined || exporting}
                  onChange={change}/>
              <ValidationError error={error}/>
              <div style={{display: "flex", flexDirection: "row", gap: "6px", justifyContent: "flex-end"}}>
                <Button text size="small" severity="secondary" label="Back" onClick={() => navigate("/")}/>
                <Button size="small" icon="pi pi-download" label="Download" loading={exporting}
                        disabled={selected === undefined} onClick={download}/>
              </div>
            </div>
          </DashboardPanel>
        </div>
      </div>
  );
}
