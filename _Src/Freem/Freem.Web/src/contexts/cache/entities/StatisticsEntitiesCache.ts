import {StatisticsClient} from "../../../clients/StatisticsClient.ts";
import {ActivitiesEntitiesCache} from "./ActivitiesEntitiesCache.ts";
import {StatisticsPerDayEntity} from "../../../entities/StatisticsPerDayEntity.ts";
import {StatisticsPerDaysRequest} from "../../../clients/models/statistics/StatisticsPerDaysRequest.ts";
import {StatisticsPerDaysResponse} from "../../../clients/models/statistics/StatisticsPerDaysResponse.ts";
import {TimeStatistics} from "../../../clients/models/statistics/TimeStatistics.ts";
import {DatePeriod} from "../../../data/DatePeriod.ts";
import {StatisticsPerActivityEntity} from "../../../entities/StatisticsPerActivityEntity.ts";
import {TimeDuration} from "../../../data/TimeDuration.ts";
import {StatisticsPerPeriodRequest} from "../../../clients/models/statistics/StatisticsPerPeriodRequest.ts";
import {StatisticsPerPeriodResponse} from "../../../clients/models/statistics/StatisticsPerPeriodResponse.ts";
import {StatisticsPerPeriodEntity} from "../../../entities/StatisticsPerPeriodEntity.ts";

export class StatisticsEntitiesCache {
  private readonly client: StatisticsClient;

  private readonly activitiesCache: ActivitiesEntitiesCache;

  public constructor(client: StatisticsClient, activitiesCache: ActivitiesEntitiesCache) {
    this.client = client;
    this.activitiesCache = activitiesCache;
  }

  public async perDays(request: StatisticsPerDaysRequest): Promise<StatisticsPerDayEntity[]> {
    const response = await this.client.perDays(request);
    if (!response.ok) {
      throw new Error(response.statusText);
    }

    const body: StatisticsPerDaysResponse = await response.json();

    const entities: StatisticsPerDayEntity[] = [];
    for (const date in body.statistics) {
      const stats = body.statistics[date];
      const entity = await this.map(stats);

      entities.push(entity);
    }

    return entities;
  }

  public async perPeriod(request: StatisticsPerPeriodRequest): Promise<StatisticsPerPeriodEntity> {
    const response = await this.client.perPeriod(request);
    if (!response.ok) {
      throw new Error(response.statusText);
    }

    const body: StatisticsPerPeriodResponse = await response.json();
    return await this.mapPerPeriod(body.statistics);
  }

  private async map(statistics: TimeStatistics): Promise<StatisticsPerDayEntity> {
    const period = DatePeriod.parse(statistics.period);
    const duration = TimeDuration.parse(statistics.recordedTime);

    const activities: StatisticsPerActivityEntity[] = [];
    for (const statisticsPerActivity of statistics.perActivities) {
      const period = DatePeriod.parse(statisticsPerActivity.period);

      const activity = await this.activitiesCache.get(statisticsPerActivity.id);
      const activityDuration = TimeDuration.parse(statisticsPerActivity.recordedTime);
      const entity = new StatisticsPerActivityEntity(period, activityDuration, activity);

      activities.push(entity);
    }

    return new StatisticsPerDayEntity(period, duration, activities);
  }

  private async mapPerPeriod(statistics: TimeStatistics): Promise<StatisticsPerPeriodEntity> {
    const duration = TimeDuration.parse(statistics.recordedTime);

    const activities: StatisticsPerActivityEntity[] = [];
    for (const statisticsPerActivity of statistics.perActivities) {
      const period = DatePeriod.parse(statisticsPerActivity.period);

      const activity = await this.activitiesCache.get(statisticsPerActivity.id);
      const activityDuration = TimeDuration.parse(statisticsPerActivity.recordedTime);
      const entity = new StatisticsPerActivityEntity(period, activityDuration, activity);

      activities.push(entity);
    }

    return new StatisticsPerPeriodEntity(duration, activities);
  }
}