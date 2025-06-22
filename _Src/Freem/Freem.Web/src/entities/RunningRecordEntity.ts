import { ActivityEntity } from "./ActivityEntity.ts";
import {TagEntity} from "./TagEntity.ts";

export class RunningRecordEntity {
  public readonly startAt: Date;
  public readonly activities: ActivityEntity[];
  
  public readonly name?: string;
  public readonly description?: string;
  public readonly tags?: TagEntity[];

  constructor(startAt: Date, activities: ActivityEntity[], name?: string, description?: string, tags?: TagEntity[]) {
    this.startAt = startAt;
    this.activities = activities;

    this.name = name;
    this.description = description;
    this.tags = tags;
  }
}