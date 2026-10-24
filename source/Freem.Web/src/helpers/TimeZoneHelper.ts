import {TimeZoneOffset} from "../data/TimeZoneOffset.ts";
import {DateHelper} from "./DateHelper.ts";
import {DatePeriod} from "../data/DatePeriod.ts";
import {DateTimePeriod} from "../data/DateTimePeriod.ts";

// A moment is a Date pointing to an exact instant. A wall clock value is a Date whose UTC fields hold
// the time on the user's clock. A date (without time) is a Date at 00:00 UTC of that calendar day.
export class TimeZoneHelper {
  public static toWallClock(moment: Date, offset: TimeZoneOffset): Date {
    return new Date(moment.getTime() + offset.milliseconds);
  }

  public static fromWallClock(wallClock: Date, offset: TimeZoneOffset): Date {
    return new Date(wallClock.getTime() - offset.milliseconds);
  }

  // Calendar of PrimeReact works in the browser local time: its value has local fields equal to the user's clock
  public static toCalendarValue(moment: Date, offset: TimeZoneOffset): Date {
    const wallClock = TimeZoneHelper.toWallClock(moment, offset);
    return new Date(
        wallClock.getUTCFullYear(),
        wallClock.getUTCMonth(),
        wallClock.getUTCDate(),
        wallClock.getUTCHours(),
        wallClock.getUTCMinutes(),
        wallClock.getUTCSeconds(),
        wallClock.getUTCMilliseconds());
  }

  public static fromCalendarValue(value: Date, offset: TimeZoneOffset): Date {
    const wallClock = new Date(Date.UTC(
        value.getFullYear(),
        value.getMonth(),
        value.getDate(),
        value.getHours(),
        value.getMinutes(),
        value.getSeconds(),
        value.getMilliseconds()));
    return TimeZoneHelper.fromWallClock(wallClock, offset);
  }

  // Date picked in a date-only Calendar (local midnight) as a date without time
  public static dateFromCalendarValue(value: Date): Date {
    return new Date(Date.UTC(value.getFullYear(), value.getMonth(), value.getDate()));
  }

  public static dateToCalendarValue(date: Date): Date {
    return new Date(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate());
  }

  public static toDate(moment: Date, offset: TimeZoneOffset): Date {
    const wallClock = TimeZoneHelper.toWallClock(moment, offset);
    return new Date(Date.UTC(wallClock.getUTCFullYear(), wallClock.getUTCMonth(), wallClock.getUTCDate()));
  }

  public static today(offset: TimeZoneOffset): Date {
    const now = new Date();
    return TimeZoneHelper.toDate(now, offset);
  }

  // Day D of the user starts at D 00:00 UTC minus the offset
  public static getDayStart(date: Date, offset: TimeZoneOffset): Date {
    return TimeZoneHelper.fromWallClock(date, offset);
  }

  // Exact moments of the user's days: [start 00:00, end 00:00) on the user's clock
  public static toDateTimePeriod(period: DatePeriod, offset: TimeZoneOffset): DateTimePeriod {
    const start = TimeZoneHelper.getDayStart(period.start, offset);
    const end = TimeZoneHelper.getDayStart(period.end, offset);
    return new DateTimePeriod(start, end);
  }

  public static formatTime(moment: Date, offset: TimeZoneOffset): string {
    const wallClock = TimeZoneHelper.toWallClock(moment, offset);
    const hours = DateHelper.padPart(wallClock.getUTCHours());
    const minutes = DateHelper.padPart(wallClock.getUTCMinutes());
    return `${hours}:${minutes}`;
  }

  public static formatDate(date: Date): string {
    const day = DateHelper.padPart(date.getUTCDate());
    const month = DateHelper.padPart(date.getUTCMonth() + 1);
    const year = date.getUTCFullYear();
    return `${day}/${month}/${year}`;
  }

  public static formatDayMonth(date: Date): string {
    const day = DateHelper.padPart(date.getUTCDate());
    const month = DateHelper.padPart(date.getUTCMonth() + 1);
    return `${day}/${month}`;
  }

  // "17:00 - 08:00 22/10" when the end falls on another day of the user
  public static formatRange(startAt: Date, endAt: Date, offset: TimeZoneOffset): string {
    const start = TimeZoneHelper.formatTime(startAt, offset);
    const end = TimeZoneHelper.formatTime(endAt, offset);

    const startDate = TimeZoneHelper.toDate(startAt, offset);
    const endDate = TimeZoneHelper.toDate(endAt, offset);
    if (startDate.getTime() === endDate.getTime()) {
      return `${start} - ${end}`;
    }

    return `${start} - ${end} ${TimeZoneHelper.formatDayMonth(endDate)}`;
  }
}
