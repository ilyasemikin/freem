import {ActivityEntity} from "./ActivityEntity.ts";
import {DatePeriod} from "../data/DatePeriod.ts";
import {TimeDuration} from "../data/TimeDuration.ts";

export class StatisticsPerActivityEntity {
  public readonly period: DatePeriod;
  public readonly recordedTime: TimeDuration;

  public readonly activity: ActivityEntity;

  public constructor(period: DatePeriod, recordedTime: TimeDuration, activity: ActivityEntity) {
    this.period = period;
    this.recordedTime = recordedTime;
    this.activity = activity;
  }
}