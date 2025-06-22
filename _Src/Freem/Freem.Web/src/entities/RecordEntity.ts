import {ActivityEntity} from "./ActivityEntity.ts";
import {TagEntity} from "./TagEntity.ts";
import {DateTimePeriod} from "../data/DateTimePeriod.ts";

export class RecordEntity {
  public readonly id: string;
  public readonly period: DateTimePeriod;
  public readonly activities: ActivityEntity[];

  public readonly name?: string;
  public readonly description?: string;
  public readonly tags?: TagEntity[];

  constructor(
      id: string,
      period: DateTimePeriod,
      activities: ActivityEntity[],
      name?: string,
      description?: string,
      tags?: TagEntity[]) {
    this.id = id;
    this.period = period;
    this.activities = activities;

    this.name = name;
    this.description = description;
    this.tags = tags;
  }
}