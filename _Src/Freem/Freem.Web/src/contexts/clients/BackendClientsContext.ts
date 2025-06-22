import {createContext} from "react";
import {AuthorizationClient} from "../../clients/AuthorizationClient.ts";
import {ActivitiesClient} from "../../clients/ActivitiesClient.ts";
import {RecordsClient} from "../../clients/RecordsClient.ts";
import {TagsClient} from "../../clients/TagsClient.ts";
import {UserClient} from "../../clients/UserClient.ts";
import {StatisticsClient} from "../../clients/StatisticsClient.ts";

export interface IBackendClientsContext {
  authorization: AuthorizationClient;

  activities: ActivitiesClient;
  records: RecordsClient;
  tags: TagsClient;

  statistics: StatisticsClient;

  user: UserClient;
}

export const BackendClientsContext = createContext<IBackendClientsContext>(undefined!);