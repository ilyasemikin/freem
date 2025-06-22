import {DateUnit} from "../../data/DateUnit.ts";
import {Button} from "primereact/button";

export interface IDateUnitPickerProps {
  unit: DateUnit;

  onChange?: (value: DateUnit) => void;
}

export function DateUnitPicker(props: IDateUnitPickerProps) {
  const {unit, onChange} = props;

  function disabled(buttonUnit: DateUnit) {
    return buttonUnit === unit;
  }

  function change(buttonUnit: DateUnit) {
    if (onChange) {
      onChange(buttonUnit);
    }
  }

  return (
      <div className="p-button-group">
        <Button disabled={disabled(DateUnit.Day)} onClick={() => change(DateUnit.Day)} size="small">Day</Button>
        <Button disabled={disabled(DateUnit.Month)} onClick={() => change(DateUnit.Month)} size="small">Month</Button>
        <Button disabled={disabled(DateUnit.Year)} onClick={() => change(DateUnit.Year)} size="small">Year</Button>
      </div>
  );
}