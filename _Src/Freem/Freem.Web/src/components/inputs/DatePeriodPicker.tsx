import {DatePeriod} from "../../data/DatePeriod.ts";
import {DateUnitPicker} from "./DateUnitPicker.tsx";
import {DateUnit} from "../../data/DateUnit.ts";
import {useState} from "react";
import {DateHelper} from "../../helpers/DateHelper.ts";
import {DatePeriodPanel} from "../DatePeriodPanel.tsx";

export interface IDatePeriodPickerProps {
  onChange?: (unit: DateUnit, period: DatePeriod) => void;
}

export function DatePeriodPicker(props: IDatePeriodPickerProps) {
  const {onChange} = props;

  const [unit, setUnit] = useState<DateUnit>(DateUnit.Day);
  const [start, setStart] = useState<Date>(new Date());

  function changeUnit(value: DateUnit) {
    setUnit(value);

    const now = new Date();
    setStart(now);
    if (onChange) {
      const period = DateHelper.getPeriod(value, now);
      onChange(value, period);
    }
  }

  function change(period: DatePeriod) {
    setStart(period.start);
    if (onChange) {
      const period = DateHelper.getPeriod(unit, start);
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