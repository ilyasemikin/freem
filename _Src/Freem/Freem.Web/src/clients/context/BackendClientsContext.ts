import {createContext} from "react";
import {ActivitiesClient} from "../ActivitiesClient.ts";
import {RecordsClient} from "../RecordsClient.ts";
import {TagsClient} from "../TagsClient.ts";
import {UserClient} from "../UserClient.ts";
import {StatisticsClient} from "../StatisticsClient.ts";
import {AuthClient} from "../AuthClient.ts";
import {AuthSession} from "../AuthSession.ts";

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