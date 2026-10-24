import {ReactNode, useMemo} from "react";
import {HttpClient} from "../http/HttpClient.ts";
import {ActivitiesClient} from "../ActivitiesClient.ts";
import {TagsClient} from "../TagsClient.ts";
import {UserClient} from "../UserClient.ts";
import {RecordsClient} from "../RecordsClient.ts";
import {BackendClientsContext, IBackendClientsContext} from "./BackendClientsContext.ts";
import {StatisticsClient} from "../StatisticsClient.ts";
import {AuthClient} from "../AuthClient.ts";
import {AuthSession} from "../AuthSession.ts";
import {ExportClient} from "../dataTransfer/ExportClient.ts";
import {FormatsClient} from "../dataTransfer/FormatsClient.ts";
import {ImportClient} from "../dataTransfer/ImportClient.ts";

interface IBackendClientsProviderProps {
  children: ReactNode;
}

export function BackendClientsProvider({children}: IBackendClientsProviderProps) {
  const clients = useMemo<IBackendClientsContext>(() => {
    const baseUrl = import.meta.env.VITE_API_URL;

    const session = new AuthSession(baseUrl);
    const httpClient = new HttpClient(baseUrl, session);

    return {
      activities: new ActivitiesClient(httpClient),
      records: new RecordsClient(httpClient),
      tags: new TagsClient(httpClient),
      statistics: new StatisticsClient(httpClient),
      exports: new ExportClient(httpClient),
      formats: new FormatsClient(httpClient),
      imports: new ImportClient(httpClient),
      user: new UserClient(httpClient),
      auth: new AuthClient(httpClient),
      session: session,
    };
  }, []);

  return (
      <BackendClientsContext value={clients}>
        {children}
      </BackendClientsContext>
  );
}