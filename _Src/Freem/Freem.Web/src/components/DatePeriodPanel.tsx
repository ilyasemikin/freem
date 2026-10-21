import {DateUnit} from "../data/DateUnit.ts";
import {MouseEvent, useRef} from "react";
import {IPopoverPanelHandle, PopoverPanel} from "./PopoverPanel.tsx";
import {CustomButton} from "./inputs/CustomButton.tsx";
import {DateHelper} from "../helpers/DateHelper.ts";
import {DatePeriod} from "../data/DatePeriod.ts";
import {Calendar} from "primereact/calendar";
import {FormEvent} from "primereact/ts-helpers";
import {TimeZoneHelper} from "../helpers/TimeZoneHelper.ts";

export interface IDatePeriodProps {
  unit: DateUnit;
  // Date without time (00:00 UTC of the user's day)
  date: Date;

  onChange?: (period: DatePeriod) => void;
}

export function DatePeriodPanel(props: IDatePeriodProps) {
  const {unit, date, onChange} = props;

  const panelRef = useRef<IPopoverPanelHandle>(null);

  function toggle(e: MouseEvent<HTMLSpanElement>) {
    if (panelRef.current) {
      panelRef.current.toggle(e.target);
    }
  }

  function change(value?: Date) {
    if (!value) {
      return;
    }

    if (onChange) {
      const period = DateHelper.getPeriod(unit, value);
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
      case DateUnit.Month:
        return `${month}/${year}`;
      case DateUnit.Year:
        return year;
    }
  }

  function view() {
    switch (unit) {
      case DateUnit.Day:
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
          <CustomButton onClick={() => changeByDiff(-1)}>
            <span style={{userSelect: "none"}} className="pi pi-chevron-left"/>
          </CustomButton>
          <span
              className="numeric" style={{width: "10ch", margin: "0 8px", textAlign: "center", cursor: "pointer", fontWeight: 500}}
              onClick={toggle}>
            {current()}
          </span>
          <CustomButton onClick={() => changeByDiff(1)}>
            <span className="pi pi-chevron-right"/>
          </CustomButton>
        </div>
        <PopoverPanel ref={panelRef}>
          <Calendar
              view={view()}
              value={TimeZoneHelper.dateToCalendarValue(date)}
              onChange={changeByCalendar}
              inline/>
        </PopoverPanel>
      </>
  );
}