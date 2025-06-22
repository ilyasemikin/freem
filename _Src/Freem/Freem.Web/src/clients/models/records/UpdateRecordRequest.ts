import {UpdateField} from "../UpdateField";

export class UpdateRecordRequest {
  public readonly period?: UpdateField<string>;
  public readonly name?: UpdateField<string>;
  public readonly description?: UpdateField<string>;
  public readonly activities?: UpdateField<string[]>;
  public readonly tags?: UpdateField<string[]>;

  constructor(
      period: UpdateField<string>,
      name?: UpdateField<string>,
      description?: UpdateField<string>,
      activities?: UpdateField<string[]>,
      tags?: UpdateField<string[]>
  ) {
    this.period = period;
    this.name = name;
    this.description = description;
    this.activities = activities;
    this.tags = tags;
  }
}