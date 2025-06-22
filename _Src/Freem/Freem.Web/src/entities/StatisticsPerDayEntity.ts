import {DatePeriod} from "../data/DatePeriod.ts";
import {StatisticsPerActivityEntity} from "./StatisticsPerActivityEntity.ts";
import {TimeDuration} from "../data/TimeDuration.ts";

export class StatisticsPerDayEntity {
  public period: DatePeriod;
  public recordedTime: TimeDuration;
  public activities: StatisticsPerActivityEntity[];

  constructor(period: DatePeriod, recordedTime: TimeDuration, activities: StatisticsPerActivityEntity[]) {
    this.period = period;
    this.recordedTime = recordedTime;
    this.activities = activities;
  }
}