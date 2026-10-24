import {DateTimePeriod} from "../data/DateTimePeriod.ts";

export class RecordPeriodEntity {
  public readonly id: string;
  public readonly period: DateTimePeriod;
  public readonly description?: string;

  constructor(id: string, period: DateTimePeriod, description?: string) {
    this.id = id;
    this.period = period;
    this.description = description;
  }
}
