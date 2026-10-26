import {Calendar} from "primereact/calendar";
import {FormEvent} from "primereact/ts-helpers";
import {ValidationError} from "../ValidationError.tsx";
import {CSSProperties} from "react";
import {useUserSettings} from "../../settings/context/useUserSettings.ts";
import {TimeZoneHelper} from "../../helpers/TimeZoneHelper.ts";
import {CalendarHelper} from "../../helpers/CalendarHelper.ts";

// Values are moments; the calendar shows and takes them in the user's time zone
export interface IDatePickerProps {
  style?: CSSProperties;
  placeholder?: string;
  value?: Date;
  error?: string;
  showTime?: boolean;
  disabled?: boolean;
  minValue?: Date;
  maxValue?: Date;
  onChange?: (value?: Date) => void;
}

export function DatePicker(props: IDatePickerProps) {
  const {style, placeholder, value, error, showTime, disabled, minValue, maxValue, onChange} = props;

  const {timeZoneOffset, weekStart} = useUserSettings();

  function toCalendarValue(moment?: Date) {
    return moment === undefined
        ? undefined
        : TimeZoneHelper.toCalendarValue(moment, timeZoneOffset);
  }

  function change(e: FormEvent<Date>) {
    if (onChange) {
      const value = e.value === null || e.value === undefined
          ? undefined
          : TimeZoneHelper.fromCalendarValue(e.value, timeZoneOffset);
      onChange(value);
    }
  }

  return (
      <div style={{...style, marginBottom: "8px"}}>
        <Calendar
            placeholder={placeholder}
            value={toCalendarValue(value)}
            showTime={showTime}
            disabled={disabled}
            dateFormat="dd/mm/yy"
            locale={CalendarHelper.locale(weekStart)}
            showSeconds
            hourFormat="24"
            minDate={toCalendarValue(minValue)}
            maxDate={toCalendarValue(maxValue)}
            onChange={change}/>
        {error !== undefined && <ValidationError error={error}/>}
      </div>
  );
}