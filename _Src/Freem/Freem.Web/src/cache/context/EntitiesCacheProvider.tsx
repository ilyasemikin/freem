import {ReactNode, useState} from "react";
import {TagResponse} from "../../clients/models/tags/TagResponse.ts";
import {ActivityResponse} from "../../clients/models/activities/ActivityResponse.ts";
import {TagsEntitiesCache} from "./entities/TagsEntitiesCache.ts";
import {ActivitiesEntitiesCache} from "./entities/ActivitiesEntitiesCache.ts";
import {EntitiesCache} from "./entities/data/EntitiesCache.ts";
import {useBackendClients} from "../clients/useBackendClients.ts";
import {EntitiesCacheContext, IEntitiesCacheContext} from "./EntitiesCacheContext.ts";
import {RecordsEntitiesCache} from "./entities/RecordsEntitiesCache.ts";
import {StatisticsEntitiesCache} from "./entities/StatisticsEntitiesCache.ts";

export interface IEntitiesStorageContextProps {
  children: ReactNode;
}

export function EntitiesCacheProvider(props: IEntitiesStorageContextProps) {
  const {children} = props;

  const [activities, setActivities] = useState<Record<string, ActivityResponse>>({});
  const [tags, setTags] = useState<Record<string, TagResponse>>({});

  const clients = useBackendClients();

  const activitiesCache = new EntitiesCache<ActivityResponse>(activities, values => setActivities({...values}));
  const tagsCache = new EntitiesCache<TagResponse>(tags, values => setTags({...values}));

  const tagsEntitiesCache = new TagsEntitiesCache(tagsCache, clients.tags);
  const activitiesEntitiesCache = new ActivitiesEntitiesCache(activitiesCache, clients.activities, tagsEntitiesCache);
  const recordsEntitiesCache = new RecordsEntitiesCache(clients.records, tagsEntitiesCache, activitiesEntitiesCache);
  const statisticsEntitiesCache = new StatisticsEntitiesCache(clients.statistics, activitiesEntitiesCache);

  const context: IEntitiesCacheContext = {
    activities: activitiesEntitiesCache,
    records: recordsEntitiesCache,
    statistics: statisticsEntitiesCache,
    tags: tagsEntitiesCache,
  }

  return (
      <EntitiesCacheContext.Provider value={context}>
        {children}
      </EntitiesCacheContext.Provider>
  );
}