export class CreateRecordRequest {
  public readonly period: string;
  public readonly activities: string[];

  public readonly name?: string;
  public readonly description?: string;
  public readonly tags?: string[];

  constructor(
    period: string, 
    activities: string[],
    name?: string, 
    description?: string, 
    tags?: string[]
  ) {
    this.period = period;
    this.activities = activities;
    
    this.name = name;
    this.description = description;
    this.tags = tags;
  }
}