import {DatePeriod} from "../../data/DatePeriod.ts";
import {DateUnitPicker} from "./DateUnitPicker.tsx";
import {DateUnit} from "../../data/DateUnit.ts";
import {useState} from "react";
import {DateHelper} from "../../helpers/DateHelper.ts";
import {DatePeriodPanel} from "../DatePeriodPanel.tsx";
import {TimeZoneHelper} from "../../helpers/TimeZoneHelper.ts";
import {useUserSettings} from "../../settings/context/useUserSettings.ts";

export interface IDatePeriodPickerProps {
  onChange?: (unit: DateUnit, period: DatePeriod) => void;
}

export function DatePeriodPicker(props: IDatePeriodPickerProps) {
  const {onChange} = props;

  const {timeZoneOffset, weekStart} = useUserSettings();

  const [unit, setUnit] = useState<DateUnit>(DateUnit.Day);
  const [start, setStart] = useState<Date>(() => TimeZoneHelper.today(timeZoneOffset));

  function changeUnit(value: DateUnit) {
    setUnit(value);

    const today = TimeZoneHelper.today(timeZoneOffset);
    const period = DateHelper.getPeriod(value, today, weekStart);
    setStart(period.start);
    if (onChange) {
      onChange(value, period);
    }
  }

  function change(period: DatePeriod) {
    // Within a unit the start defines the period: picking another day of the shown week must not reload data
    if (period.start.getTime() === start.getTime()) {
      return;
    }

    setStart(period.start);
    if (onChange) {
      onChange(unit, period);
    }
  }

  return (
      <div style={{display: "flex", flexDirection: "column", alignItems: "center"}}>
        <div style={{marginBottom: "10px"}}>
          <DateUnitPicker
              unit={unit}
              onChange={changeUnit}/>
        </div>
        <div style={{display: "flex", flexDirection: "row", marginBottom: "10px", alignItems: "center", userSelect: "none"}}>
          <DatePeriodPanel unit={unit} date={start} onChange={change} />
        </div>
      </div>
  );
}