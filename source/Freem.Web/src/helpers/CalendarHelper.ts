import {addLocale} from "primereact/api";
import {WeekStart} from "../data/WeekStart.ts";

const registered = new Set<string>();

export class CalendarHelper {
  // Locale of PrimeReact Calendar: English with the user's first day of the week
  public static locale(weekStart: WeekStart): string {
    const name = `en-week-${weekStart.value.toLowerCase()}`;
    if (!registered.has(name)) {
      addLocale(name, {firstDayOfWeek: weekStart.day});
      registered.add(name);
    }

    return name;
  }
}
