import {UpdateField} from "../UpdateField";

export class UpdateRecordPeriodRequest {
  public readonly period?: UpdateField<string>;
  public readonly description?: UpdateField<string>;

  constructor(period?: UpdateField<string>, description?: UpdateField<string>) {
    this.period = period;
    this.description = description;
  }
}
