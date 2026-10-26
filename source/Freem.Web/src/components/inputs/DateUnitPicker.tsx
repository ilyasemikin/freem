import {DateUnit} from "../../data/DateUnit.ts";
import {Button} from "primereact/button";

export interface IDateUnitPickerProps {
  unit: DateUnit;

  onChange?: (value: DateUnit) => void;
}

export function DateUnitPicker(props: IDateUnitPickerProps) {
  const {unit, onChange} = props;

  function active(buttonUnit: DateUnit) {
    return buttonUnit === unit;
  }

  function change(buttonUnit: DateUnit) {
    if (onChange) {
      onChange(buttonUnit);
    }
  }

  return (
      <div className="segmented">
        {[[DateUnit.Day, "Day"], [DateUnit.Week, "Week"], [DateUnit.Month, "Month"], [DateUnit.Year, "Year"]].map(([value, label]) =>
            <Button key={label} text={!active(value as DateUnit)} aria-pressed={active(value as DateUnit)}
                    onClick={() => change(value as DateUnit)} size="small">{label}</Button>)}
      </div>
  );
}