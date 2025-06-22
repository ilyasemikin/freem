import {RecordResponse} from "../../../clients/models/records/RecordResponse.ts";
import {RecordsClient} from "../../../clients/RecordsClient.ts";
import {TagsEntitiesCache} from "./TagsEntitiesCache.ts";
import {ActivitiesEntitiesCache} from "./ActivitiesEntitiesCache.ts";
import {ListRecordByPeriodRequest} from "../../../clients/models/records/ListRecordByPeriodRequest.ts";
import {RecordEntity} from "../../../entities/RecordEntity.ts";
import {DateTimePeriod} from "../../../data/DateTimePeriod.ts";

export class RecordsEntitiesCache {
  private recordsClient: RecordsClient;

  private readonly tagsCache: TagsEntitiesCache;
  private readonly activitiesCache: ActivitiesEntitiesCache;

  public constructor(
      recordsClient: RecordsClient,
      tagsCache: TagsEntitiesCache,
      activitiesCache: ActivitiesEntitiesCache) {
    this.recordsClient = recordsClient;

    this.tagsCache = tagsCache;
    this.activitiesCache = activitiesCache;
  }

  public async listByPeriodRequest(request: ListRecordByPeriodRequest) {
    const response = await this.recordsClient.listByPeriod(request);
    if (!response.ok) {
      throw new Error(response.statusText);
    }

    const responses: RecordResponse[] = await response.json();
    return this.mapList(responses);
  }

  private async mapList(responses: RecordResponse[]): Promise<RecordEntity[]> {
    const records: RecordEntity[] = [];
    for (let i = 0; i < responses.length; i++) {
      const record = await this.map(responses[i]);
      records.push(record);
    }

    return records;
  }

  private async map(response: RecordResponse): Promise<RecordEntity> {
    const period = DateTimePeriod.parse(response.period);
    const activities = await this.activitiesCache.list(response.activities);
    const tags = response.tags !== undefined
        ? await this.tagsCache.list(response.tags)
        : [];

    return new RecordEntity(response.id, period, activities, response.name, response.description, tags);
  }
}