import {DateUnitPeriod} from "./DateUnitPeriod.ts";

export class StatisticsPerPeriodRequest {
  public readonly period: DateUnitPeriod;

  public constructor(period: DateUnitPeriod) {
     this.period = period;
  }
}