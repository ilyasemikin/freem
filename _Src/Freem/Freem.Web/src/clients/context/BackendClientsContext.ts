import {createContext} from "react";
import {ActivitiesClient} from "../../clients/ActivitiesClient.ts";
import {RecordsClient} from "../../clients/RecordsClient.ts";
import {TagsClient} from "../../clients/TagsClient.ts";
import {UserClient} from "../../clients/UserClient.ts";
import {StatisticsClient} from "../../clients/StatisticsClient.ts";
import {AuthClient} from "../../clients/AuthClient.ts";
import {AuthSession} from "../../clients/AuthSession.ts";

export interface IBackendClientsContext {
  activities: ActivitiesClient;
  records: RecordsClient;
  tags: TagsClient;

  statistics: StatisticsClient;

  user: UserClient;

  auth: AuthClient;
  session: AuthSession;
}

export const BackendClientsContext = createContext<IBackendClientsContext>(undefined!);