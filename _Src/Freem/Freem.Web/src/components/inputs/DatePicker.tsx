import {Calendar} from "primereact/calendar";
import {FormEvent} from "primereact/ts-helpers";
import {ValidationError} from "../ValidationError.tsx";
import {CSSProperties} from "react";

export interface IDatePickerProps {
  style?: CSSProperties;
  placeholder?: string;
  value?: Date;
  error?: string;
  showTime?: boolean;
  minValue?: Date;
  maxValue?: Date;
  onChange?: (value?: Date) => void;
}

export function DatePicker(props: IDatePickerProps) {
  const {style, placeholder, value, error, showTime, minValue, maxValue, onChange} = props;

  function change(e: FormEvent<Date>) {
    if (onChange) {
      const value = e.value === null
          ? undefined
          : e.value;
      onChange(value);
    }
  }

  return (
      <div style={{...style, marginBottom: "3px"}}>
        <Calendar
            placeholder={placeholder}
            value={value}
            showTime={showTime}
            showSeconds
            hourFormat="24"
            minDate={minValue}
            maxDate={maxValue}
            onChange={change}/>
        {error !== undefined && <ValidationError error={error}/>}
      </div>
  );
}