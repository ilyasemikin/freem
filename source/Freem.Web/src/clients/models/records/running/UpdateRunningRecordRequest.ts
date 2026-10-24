import { UpdateField } from "../../UpdateField";

export class UpdateRunningRecordRequest {
  public readonly name?: UpdateField<string>;
  public readonly description?: UpdateField<string>;
  public readonly activities?: UpdateField<string[]>;
  public readonly tags?: UpdateField<string[]>;

  constructor(
    name?: UpdateField<string>,
    description?: UpdateField<string>,
    activities?: UpdateField<string[]>,
    tags?: UpdateField<string[]>
  ) {
    this.name = name;
    this.description = description;
    this.activities = activities;
    this.tags = tags;
  }
}