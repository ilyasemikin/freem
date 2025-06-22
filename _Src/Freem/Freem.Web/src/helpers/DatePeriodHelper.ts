import {DateUnit} from "../data/DateUnit.ts";
import {DatePeriod} from "../data/DatePeriod.ts";
import {DateUnitPeriod} from "../clients/models/statistics/DateUnitPeriod.ts";

export class DatePeriodHelper {
  public static toPerPeriodRequest(unit: DateUnit, period: DatePeriod): DateUnitPeriod {
    const day = period.start.getDate();
    const month = period.start.getMonth() + 1;
    const year = period.start.getFullYear();

    switch (unit) {
      case DateUnit.Day:
        return {unit: "day", day: `${year}-${month}-${day}`};
      case DateUnit.Month:
        return {unit: "month", month: `${month}.${year}`};
      case DateUnit.Year:
        return {unit: "year", year: `${year}`};
    }
  }
}