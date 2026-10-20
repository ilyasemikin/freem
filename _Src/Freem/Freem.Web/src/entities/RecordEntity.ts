import {ActivityEntity} from "./ActivityEntity.ts";
import {TagEntity} from "./TagEntity.ts";
import {RecordPeriodEntity} from "./RecordPeriodEntity.ts";
import {TimeDuration} from "../data/TimeDuration.ts";

export class RecordEntity {
  public readonly id: string;
  public readonly periods: RecordPeriodEntity[];
  public readonly activities: ActivityEntity[];

  public readonly name?: string;
  public readonly description?: string;
  public readonly tags?: TagEntity[];

  constructor(
      id: string,
      periods: RecordPeriodEntity[],
      activities: ActivityEntity[],
      name?: string,
      description?: string,
      tags?: TagEntity[]) {
    this.id = id;
    this.periods = [...periods].sort((left, right) => +left.period.startAt - +right.period.startAt);
    this.activities = activities;

    this.name = name;
    this.description = description;
    this.tags = tags;
  }

  public get startAt(): Date {
    return this.periods[0].period.startAt;
  }

  public get endAt(): Date {
    return this.periods[this.periods.length - 1].period.endAt;
  }

  public get duration(): TimeDuration {
    const seconds = this.periods.reduce((sum, item) => sum + item.period.duration, 0);
    return new TimeDuration(seconds);
  }
}