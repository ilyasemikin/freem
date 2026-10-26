import {DateUnit} from "../data/DateUnit.ts";
import {KeyboardEvent, MouseEvent, useRef} from "react";
import {IPopoverPanelHandle, PopoverPanel} from "./PopoverPanel.tsx";
import {CustomButton} from "./inputs/CustomButton.tsx";
import {DateHelper} from "../helpers/DateHelper.ts";
import {DatePeriod} from "../data/DatePeriod.ts";
import {Calendar} from "primereact/calendar";
import {FormEvent} from "primereact/ts-helpers";
import {TimeZoneHelper} from "../helpers/TimeZoneHelper.ts";
import {CalendarHelper} from "../helpers/CalendarHelper.ts";
import {useUserSettings} from "../settings/context/useUserSettings.ts";

export interface IDatePeriodProps {
  unit: DateUnit;
  // Date without time (00:00 UTC of the user's day): the start of the period
  date: Date;

  onChange?: (period: DatePeriod) => void;
}

export function DatePeriodPanel(props: IDatePeriodProps) {
  const {unit, date, onChange} = props;

  const {weekStart} = useUserSettings();

  const panelRef = useRef<IPopoverPanelHandle>(null);

  function handleKeyDown(e: KeyboardEvent<HTMLSpanElement>) {
    if (e.key === "Enter" || e.key === " ") {
      e.preventDefault();
      e.currentTarget.click();
    }
  }

  function toggle(e: MouseEvent<HTMLSpanElement>) {
    if (panelRef.current) {
      panelRef.current.toggle(e.currentTarget);
    }
  }

  function change(value?: Date) {
    if (!value) {
      return;
    }

    if (onChange) {
      const period = DateHelper.getPeriod(unit, value, weekStart);
      onChange(period);
    }
  }

  function changeByCalendar(e: FormEvent<Date>) {
    panelRef.current?.hide();

    if (onChange) {
      const value = e.value === null || e.value === undefined
          ? undefined
          : TimeZoneHelper.dateFromCalendarValue(e.value);
      change(value);
    }
  }

  function changeByDiff(diff: number) {
    const changed = DateHelper.addUnits(unit, date, diff);

    if (onChange) {
      change(changed)
    }
  }

  function current() {
    const day = DateHelper.padPart(date.getUTCDate());
    const month = DateHelper.padPart(date.getUTCMonth() + 1);
    const year = date.getUTCFullYear();

    switch (unit) {
      case DateUnit.Day:
        return `${day}/${month}/${year}`;
      case DateUnit.Week: {
        // The end of the period is exclusive: the label shows the last day of the week
        const period = DateHelper.getPeriod(unit, date, weekStart);
        const last = DateHelper.addUnits(DateUnit.Day, period.end, -1);
        const first = TimeZoneHelper.formatDayMonth(period.start);
        const lastDate = TimeZoneHelper.formatDate(last);
        return `${first} - ${lastDate}`;
      }
      case DateUnit.Month:
        return `${month}/${year}`;
      case DateUnit.Year:
        return year;
    }
  }

  function view() {
    switch (unit) {
      case DateUnit.Day:
      case DateUnit.Week:
        return "date";
      case DateUnit.Month:
        return "month";
      case DateUnit.Year:
        return "year";
    }
  }

  return (
      <>
        <div style={{display: "flex", flexDirection: "row", alignItems: "center"}}>
          <CustomButton label="Previous" onClick={() => changeByDiff(-1)}>
            <span style={{userSelect: "none"}} className="pi pi-chevron-left"/>
          </CustomButton>
          <span
              role="button"
              tabIndex={0}
              aria-label="Select date"
              title="Select date"
              className="numeric" style={{width: unit === DateUnit.Week ? "18ch" : "10ch", margin: "0 8px", textAlign: "center", cursor: "pointer", fontWeight: 500}}
              onClick={toggle}
              onKeyDown={handleKeyDown}>
            {current()}
          </span>
          <CustomButton label="Next" onClick={() => changeByDiff(1)}>
            <span className="pi pi-chevron-right"/>
          </CustomButton>
        </div>
        <PopoverPanel ref={panelRef}>
          <Calendar
              view={view()}
              locale={CalendarHelper.locale(weekStart)}
              value={TimeZoneHelper.dateToCalendarValue(date)}
              onChange={changeByCalendar}
              inline/>
        </PopoverPanel>
      </>
  );
}