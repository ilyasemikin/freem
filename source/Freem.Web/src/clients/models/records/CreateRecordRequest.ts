import {RecordPeriodRequest} from "./RecordPeriodRequest.ts";

export class CreateRecordRequest {
  public readonly periods: RecordPeriodRequest[];
  public readonly activities: string[];

  public readonly name?: string;
  public readonly description?: string;
  public readonly tags?: string[];

  constructor(
    periods: RecordPeriodRequest[],
    activities: string[],
    name?: string,
    description?: string,
    tags?: string[]
  ) {
    this.periods = periods;
    this.activities = activities;

    this.name = name;
    this.description = description;
    this.tags = tags;
  }
}