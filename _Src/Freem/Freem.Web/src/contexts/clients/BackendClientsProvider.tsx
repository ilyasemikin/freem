import {ReactNode} from "react";
import {HttpClient} from "../../clients/http/HttpClient.ts";
import {ActivitiesClient} from "../../clients/ActivitiesClient.ts";
import {TagsClient} from "../../clients/TagsClient.ts";
import {UserClient} from "../../clients/UserClient.ts";
import {RecordsClient} from "../../clients/RecordsClient.ts";
import {BackendClientsContext, IBackendClientsContext} from "./BackendClientsContext.ts";
import {StatisticsClient} from "../../clients/StatisticsClient.ts";

interface IBackendClientsProviderProps {
  children: ReactNode;
}

export function BackendClientsProvider({children}: IBackendClientsProviderProps) {
  const httpClient = new HttpClient(import.meta.env.VITE_API_URL);

  const activities = new ActivitiesClient(httpClient);
  const records = new RecordsClient(httpClient);
  const tags = new TagsClient(httpClient);
  const statistics = new StatisticsClient(httpClient);
  const user = new UserClient(httpClient);

  const clients: IBackendClientsContext = {
    activities: activities,
    records: records,
    tags: tags,
    statistics: statistics,
    user: user,
  }

  return (
      <BackendClientsContext value={clients}>
        {children}
      </BackendClientsContext>
  );
}