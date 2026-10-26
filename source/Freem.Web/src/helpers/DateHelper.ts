import { DatePeriod } from "../data/DatePeriod.ts";
import {DateUnit} from "../data/DateUnit.ts";
import {WeekStart} from "../data/WeekStart.ts";

const DaysInWeek = 7;

export class DateHelper {
  // Works with dates without time (00:00 UTC of the day), see TimeZoneHelper; the end is exclusive
  public static getPeriod(unit: DateUnit, date: Date, weekStart: WeekStart): DatePeriod {
    const year = date.getUTCFullYear();
    const month = date.getUTCMonth();

    switch (unit) {
      case DateUnit.Day: {
        const start = new Date(Date.UTC(year, month, date.getUTCDate()));
        const end = new Date(Date.UTC(year, month, date.getUTCDate() + 1));
        return new DatePeriod(start, end);
      }
      case DateUnit.Week: {
        const start = weekStart.getWeekStart(date);
        const end = new Date(Date.UTC(start.getUTCFullYear(), start.getUTCMonth(), start.getUTCDate() + DaysInWeek));
        return new DatePeriod(start, end);
      }
      case DateUnit.Month: {
        const start = new Date(Date.UTC(year, month, 1));
        const end = new Date(Date.UTC(year, month + 1, 1));
        return new DatePeriod(start, end);
      }
      case DateUnit.Year: {
        const start = new Date(Date.UTC(year, 0, 1));
        const end = new Date(Date.UTC(year + 1, 0, 1));
        return new DatePeriod(start, end);
      }
    }
  }

  public static addUnits(unit: DateUnit, date: Date, count: number): Date {
    const year = date.getUTCFullYear();
    const month = date.getUTCMonth();
    const day = date.getUTCDate();

    switch (unit) {
      case DateUnit.Day:
        return new Date(Date.UTC(year, month, day + count));
      case DateUnit.Week:
        return new Date(Date.UTC(year, month, day + count * DaysInWeek));
      case DateUnit.Month:
        return new Date(Date.UTC(year, month + count, 1));
      case DateUnit.Year:
        return new Date(Date.UTC(year + count, 0, 1));
    }
  }

  public static padPart(value: number): string {
    return value.toString().padStart(2, '0');
  }
}