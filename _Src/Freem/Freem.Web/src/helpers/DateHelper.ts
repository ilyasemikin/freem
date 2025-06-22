import { DatePeriod } from "../data/DatePeriod.ts";
import {DateUnit} from "../data/DateUnit.ts";

export class DateHelper {
  public static toFormattedString(date: Date): string {
    return date.toISOString().slice(11, 19);
  }

  public static getPeriod(unit: DateUnit, date: Date): DatePeriod {
    const start = new Date(date);
    let end = new Date(date);

    switch (unit) {
      case DateUnit.Day:
        start.setHours(0, 0, 0, 0);
        end = new Date(start);
        end.setHours(23, 59, 59, 999);
        break;

      case DateUnit.Month:
        start.setDate(1);
        start.setHours(0, 0, 0, 0);
        end = new Date(start.getFullYear(), start.getMonth() + 1, 0);
        end.setHours(23, 59, 59, 999);
        break;

      case DateUnit.Year:
        start.setMonth(0, 1); // 1 января
        start.setHours(0, 0, 0, 0);
        end = new Date(start.getFullYear(), 11, 31); // 31 декабря
        end.setHours(23, 59, 59, 999);
        break;
    }

    const next = new Date(end);
    next.setDate(next.getDate() + 1);

    return new DatePeriod(start, next);
  }

  public static padPart(value: number): string {
    return value.toString().padStart(2, '0');
  }
}