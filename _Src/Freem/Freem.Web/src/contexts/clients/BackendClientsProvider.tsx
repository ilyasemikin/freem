import {ReactNode} from "react";
import {HttpClient} from "../../clients/http/HttpClient.ts";
import {ActivitiesClient} from "../../clients/ActivitiesClient.ts";
import {TagsClient} from "../../clients/TagsClient.ts";
import {AuthorizedHttpClient} from "../../clients/http/AuthorizedHttpClient.ts";
import {AuthorizationClient} from "../../clients/AuthorizationClient.ts";
import {UserClient} from "../../clients/UserClient.ts";
import {RecordsClient} from "../../clients/RecordsClient.ts";
import {BackendClientsContext, IBackendClientsContext} from "./BackendClientsContext.ts";
import {StatisticsClient} from "../../clients/StatisticsClient.ts";

interface IBackendClientsProviderProps {
  children: ReactNode;
}

export function BackendClientsProvider({children}: IBackendClientsProviderProps) {
  const httpClient = new HttpClient("http://localhost:30000");

  const authorization = new AuthorizationClient(httpClient);

  const authorizedHttpClient = new AuthorizedHttpClient(httpClient, authorization.refresh.bind(authorization));

  const activities = new ActivitiesClient(authorizedHttpClient);
  const records = new RecordsClient(authorizedHttpClient);
  const tags = new TagsClient(authorizedHttpClient);
  const statistics = new StatisticsClient(authorizedHttpClient);
  const user = new UserClient(authorizedHttpClient);

  const clients: IBackendClientsContext = {
    authorization: authorization,
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