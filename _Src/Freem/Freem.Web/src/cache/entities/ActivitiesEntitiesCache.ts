import {ActivityResponse} from "../../../clients/models/activities/ActivityResponse.ts";
import {ActivitiesClient} from "../../../clients/ActivitiesClient.ts";
import {ListActivityRequest} from "../../../clients/models/activities/ListActivityRequest.ts";
import {ActivityEntity} from "../../../entities/ActivityEntity.ts";
import {TagsEntitiesCache} from "./TagsEntitiesCache.ts";
import {EntitiesCache} from "./data/EntitiesCache.ts";
import {FindActivityByNameRequest} from "../../../clients/models/activities/FindActivityByNameRequest.ts";

export class ActivitiesEntitiesCache {
  private readonly cache: EntitiesCache<ActivityResponse>
  private readonly activitiesClient: ActivitiesClient;

  private readonly tagsEntitiesCache: TagsEntitiesCache;

  public constructor(
      cache: EntitiesCache<ActivityResponse>,
      activitiesClient: ActivitiesClient,
      tagsEntitiesCache: TagsEntitiesCache) {
    this.cache = cache;

    this.activitiesClient = activitiesClient;
    this.tagsEntitiesCache = tagsEntitiesCache;
  }

  public async get(id: string): Promise<ActivityEntity> {
    if (this.cache.contains(id)) {
      return await this.map(this.cache.get(id));
    }

    const response = await this.activitiesClient.get(id);
    if (!response.ok) {
      throw new Error(response.statusText);
    }

    const body: ActivityResponse = await response.json();
    this.cache.append([body]);
    return this.map(body);
  }

  public async list(ids: string[]): Promise<ActivityEntity[]> {
    const result = this.cache.list(ids);

    const fetched: ActivityResponse[] = [];
    for (const id of result.others) {
      const response = await this.activitiesClient.get(id);
      if (!response.ok) {
        throw new Error(response.statusText);
      }

      fetched.push(await response.json());
    }

    this.cache.append(fetched);
    const responses = result.founded.concat(fetched);
    return this.mapList(responses);
  }

  public async listByRequest(request: ListActivityRequest): Promise<ActivityEntity[]> {
    const response = await this.activitiesClient.list(request);
    if (!response.ok) {
      throw new Error(response.statusText);
    }

    const responses: ActivityResponse[] = await response.json();
    this.cache.append(responses);

    return this.mapList(responses);
  }

  public async findByNameRequest(request: FindActivityByNameRequest): Promise<ActivityEntity[]> {
    const response = await this.activitiesClient.findByName(request);
    if (!response.ok) {
      throw new Error(response.statusText);
    }

    const responses: ActivityResponse[] = await response.json();
    this.cache.append(responses);

    return this.mapList(responses);
  }

  private async mapList(responses: ActivityResponse[]): Promise<ActivityEntity[]> {
    const activities: ActivityEntity[] = [];
    for (let i = 0; i < responses.length; i++) {
      const activity = await this.map(responses[i]);
      activities.push(activity);
    }

    return activities;
  }

  private async map(response: ActivityResponse): Promise<ActivityEntity> {
    const tags = response.tags !== undefined
        ? await this.tagsEntitiesCache.list(response.tags)
        : [];
    return new ActivityEntity(response.id, response.name, tags);
  }
}