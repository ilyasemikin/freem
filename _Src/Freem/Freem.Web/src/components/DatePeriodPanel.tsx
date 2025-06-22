import {DateUnit} from "../data/DateUnit.ts";
import {MouseEvent, useRef} from "react";
import {IPopoverPanelHandle, PopoverPanel} from "./PopoverPanel.tsx";
import {CustomButton} from "./inputs/CustomButton.tsx";
import {DateHelper} from "../helpers/DateHelper.ts";
import {DatePeriod} from "../data/DatePeriod.ts";
import {Calendar} from "primereact/calendar";
import {FormEvent} from "primereact/ts-helpers";

export interface IDatePeriodProps {
  unit: DateUnit;
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
      const value = e.value === null
          ? undefined
          : e.value
      change(value);
    }
  }

  function changeByDiff(diff: number) {
    const changed = new Date(date);

    switch (unit) {
      case DateUnit.Day: {
        changed.setDate(date.getDate() + diff);
        break;
      }
      case DateUnit.Month: {
        changed.setMonth(date.getMonth() + diff);
        break;
      }
      case DateUnit.Year: {
        changed.setFullYear(date.getFullYear() + diff);
      }
    }

    if (onChange) {
      change(changed)
    }
  }

  function current() {
    const day = DateHelper.padPart(date.getDate());
    const month = DateHelper.padPart(date.getMonth() + 1);
    const year = date.getFullYear();

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
        <div style={{display: "flex", flexDirection: "row"}}>
          <CustomButton onClick={() => changeByDiff(-1)}>
            <span style={{userSelect: "none"}} className="pi pi-chevron-left"/>
          </CustomButton>
          <span
              style={{width: "10ch", margin: "0 15px", textAlign: "center", cursor: "pointer"}}
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
              value={date}
              onChange={changeByCalendar}
              inline/>
        </PopoverPanel>
      </>
  );
}