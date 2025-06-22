import {TimeDuration} from "../data/TimeDuration.ts";
import {StatisticsPerActivityEntity} from "./StatisticsPerActivityEntity.ts";

export class StatisticsPerPeriodEntity {
  public recordedTime: TimeDuration;
  public perActivities: StatisticsPerActivityEntity[];

  public constructor(recordedTime: TimeDuration, perActivities: StatisticsPerActivityEntity[]) {
    this.recordedTime = recordedTime;
    this.perActivities = perActivities;
  }
}