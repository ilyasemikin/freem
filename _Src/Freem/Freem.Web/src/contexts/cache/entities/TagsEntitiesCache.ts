import {TagResponse} from "../../../clients/models/tags/TagResponse.ts";
import {TagsClient} from "../../../clients/TagsClient.ts";
import {TagEntity} from "../../../entities/TagEntity.ts";
import {ListTagRequest} from "../../../clients/models/tags/ListTagRequest.ts";
import {EntitiesCache} from "./data/EntitiesCache.ts";

export class TagsEntitiesCache {
  private readonly cache: EntitiesCache<TagResponse>;
  private readonly tagsClient: TagsClient;

  public constructor(cache: EntitiesCache<TagResponse>, tagsClient: TagsClient) {
    this.cache = cache;
    this.tagsClient = tagsClient;
  }

  public async list(ids: string[]): Promise<TagEntity[]> {
    const result = this.cache.list(ids);

    const fetched: TagResponse[] = [];
    for (const id of result.others) {
      const response = await this.tagsClient.get(id);
      if (!response.ok) {
        throw new Error(response.statusText);
      }

      fetched.push(await response.json());
    }

    this.cache.append(fetched);
    return result.founded.concat(fetched);
  }

  public async listByRequest(request: ListTagRequest): Promise<TagEntity[]> {
    const response = await this.tagsClient.list(request);
    if (!response.ok) {
      throw new Error(response.statusText);
    }

    const responses: TagResponse[] = await response.json();
    this.cache.append(responses);
    return responses;
  }

  public async findByNameRequest(name: string): Promise<TagEntity[]> {
    const response = await this.tagsClient.findByName(name);
    if (!response.ok) {
      throw new Error(response.statusText);
    }

    const responses: TagResponse[] = await response.json();
    this.cache.append(responses);
    return responses;
  }
}