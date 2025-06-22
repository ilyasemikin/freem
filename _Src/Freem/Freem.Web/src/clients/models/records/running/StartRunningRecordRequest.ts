export class StartRunningRecordRequest {
  public readonly startAt: string;
  public readonly activities: string[];

  public readonly name?: string;
  public readonly description?: string;
  public readonly tags?: string[];

  constructor(
    startAt: string,
    activities: string[],
    name?: string,
    description?: string,
    tags?: string[]
  ) {
    this.startAt = startAt;
    this.activities = activities;

    this.name = name;
    this.description = description;
    this.tags = tags;
  }
}