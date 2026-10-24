export class RunningRecordResponse {
  public readonly id: string;
  public readonly startAt: string;
  public readonly activities: string[];

  public readonly name?: string;
  public readonly description?: string;
  public readonly tags?: string[];

  constructor(
    id: string,
    startAt: string,
    activities: string[],
    name?: string,
    description?: string,
    tags?: string[]
  ) {
    this.id = id;
    this.startAt = startAt;
    this.activities = activities;

    this.name = name;
    this.description = description;
    this.tags = tags;
  }
}