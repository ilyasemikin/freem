import {ReactNode, useMemo} from "react";
import {HttpClient} from "../../clients/http/HttpClient.ts";
import {ActivitiesClient} from "../../clients/ActivitiesClient.ts";
import {TagsClient} from "../../clients/TagsClient.ts";
import {UserClient} from "../../clients/UserClient.ts";
import {RecordsClient} from "../../clients/RecordsClient.ts";
import {BackendClientsContext, IBackendClientsContext} from "./BackendClientsContext.ts";
import {StatisticsClient} from "../../clients/StatisticsClient.ts";
import {AuthClient} from "../../clients/AuthClient.ts";
import {AuthSession} from "../../clients/AuthSession.ts";

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