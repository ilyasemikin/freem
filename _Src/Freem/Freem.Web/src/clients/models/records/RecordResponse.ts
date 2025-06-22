export class RecordResponse {
  public readonly id: string;
  public readonly period: string;
  public readonly activities: [string];
  
  public readonly name?: string;
  public readonly description?: string;
  public readonly tags?: [string];

  constructor(
    id: string, 
    period: string, 
    activities: [string], 
    name?: string, 
    description?: string, 
    tags?: [string]
  ) {
    this.id = id;
    this.period = period;
    this.activities = activities;

    this.name = name;
    this.description = description;
    this.tags = tags;
  }
}