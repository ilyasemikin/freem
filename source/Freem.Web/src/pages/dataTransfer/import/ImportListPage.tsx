import {useEffect, useState} from "react";
import {AppHeader} from "../../../components/AppHeader.tsx";
import {DashboardPanel} from "../../../components/DashboardPanel.tsx";
import {ValidationError} from "../../../components/ValidationError.tsx";
import {useBackendClients} from "../../../clients/context/useBackendClients.ts";
import {ApiError} from "../../../clients/http/ApiError.ts";
import {FormatResponse} from "../../../clients/models/dataTransfer/formats/FormatResponse.ts";
import {ListFormatsResponse} from "../../../clients/models/dataTransfer/formats/ListFormatsResponse.ts";
import {ImportUploadPanel} from "./ImportUploadPanel.tsx";
import {ImportList} from "./ImportList.tsx";

export function ImportListPage() {
  const {formats: formatsClient} = useBackendClients();

  const [formats, setFormats] = useState<FormatResponse[] | undefined>();
  const [error, setError] = useState<string | undefined>();

  useEffect(() => {
    let current = true;

    async function load() {
      const response = await formatsClient.list();
      if (!response.ok) {
        throw await ApiError.fromResponse(response);
      }

      const body: ListFormatsResponse = await response.json();
      if (current) {
        setFormats(body.importFormats);
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
  }, [formatsClient]);

  return (
      <div style={{display: "flex", flexDirection: "column", height: "100%"}}>
        <AppHeader/>
        <div className="dashboard">
          <DashboardPanel style={{maxWidth: "360px"}}>
            <ValidationError error={error}/>
            <ImportUploadPanel formats={formats}/>
          </DashboardPanel>
          <DashboardPanel>
            <ImportList formats={formats}/>
          </DashboardPanel>
        </div>
      </div>
  );
}
