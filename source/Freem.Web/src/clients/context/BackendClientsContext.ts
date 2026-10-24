import {createContext} from "react";
import {ActivitiesClient} from "../ActivitiesClient.ts";
import {RecordsClient} from "../RecordsClient.ts";
import {TagsClient} from "../TagsClient.ts";
import {UserClient} from "../UserClient.ts";
import {StatisticsClient} from "../StatisticsClient.ts";
import {AuthClient} from "../AuthClient.ts";
import {AuthSession} from "../AuthSession.ts";
import {ExportClient} from "../dataTransfer/ExportClient.ts";
import {FormatsClient} from "../dataTransfer/FormatsClient.ts";
import {ImportClient} from "../dataTransfer/ImportClient.ts";

export interface IBackendClientsContext {
  activities: ActivitiesClient;
  records: RecordsClient;
  tags: TagsClient;

  statistics: StatisticsClient;

  exports: ExportClient;
  formats: FormatsClient;
  imports: ImportClient;

  user: UserClient;

  auth: AuthClient;
  session: AuthSession;
}

export const BackendClientsContext = createContext<IBackendClientsContext>(undefined!);