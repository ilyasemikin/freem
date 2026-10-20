import {RecordPeriodResponse} from "./RecordPeriodResponse.ts";

export class RecordResponse {
  public readonly id: string;
  public readonly periods: RecordPeriodResponse[];
  public readonly activities: [string];

  public readonly name?: string;
  public readonly description?: string;
  public readonly tags?: [string];

  constructor(
    id: string,
    periods: RecordPeriodResponse[],
    activities: [string],
    name?: string,
    description?: string,
    tags?: [string]
  ) {
    this.id = id;
    this.periods = periods;
    this.activities = activities;

    this.name = name;
    this.description = description;
    this.tags = tags;
  }
}