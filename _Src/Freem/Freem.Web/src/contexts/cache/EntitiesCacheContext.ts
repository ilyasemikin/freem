import {ActivitiesEntitiesCache} from "./entities/ActivitiesEntitiesCache.ts";
import {TagsEntitiesCache} from "./entities/TagsEntitiesCache.ts";
import {createContext} from "react";
import {RecordsEntitiesCache} from "./entities/RecordsEntitiesCache.ts";
import {StatisticsEntitiesCache} from "./entities/StatisticsEntitiesCache.ts";

export interface IEntitiesCacheContext {
  activities: ActivitiesEntitiesCache;
  records: RecordsEntitiesCache;
  statistics: StatisticsEntitiesCache;
  tags: TagsEntitiesCache;
}

export const EntitiesCacheContext = createContext<IEntitiesCacheContext>({
  activities: undefined!,
  records: undefined!,
  statistics: undefined!,
  tags: undefined!
});