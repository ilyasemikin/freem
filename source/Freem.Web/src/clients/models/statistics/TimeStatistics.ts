import {TimeStatisticsPerActivity} from "./TimeStatisticsPerActivity.ts";

export class TimeStatistics {
  public period: string;
  public recordedTime: string;

  public perActivities: TimeStatisticsPerActivity[];

  public constructor(
      period: string,
      recordedTime: string,
      perActivities: TimeStatisticsPerActivity[])
  {
    this.period = period;
    this.recordedTime = recordedTime;
    this.perActivities = perActivities;
  }
}