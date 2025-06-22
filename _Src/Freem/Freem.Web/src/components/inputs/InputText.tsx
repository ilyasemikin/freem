import {InputText as PrimeInputText} from "primereact/inputtext";
import {ValidationError} from "../ValidationError.tsx";
import {ChangeEventHandler, CSSProperties, HTMLInputTypeAttribute} from "react";

export type InputTextSize = "small" | "medium" | "large";

export interface IInputTextProps {
	style?: CSSProperties,
  loading?: boolean;
  name?: string;
	type?: HTMLInputTypeAttribute;
  placeholder?: string;
  value?: string;
  error?: string;
  size?: InputTextSize;
  onChange?: ChangeEventHandler<HTMLInputElement> | undefined;
}

function GetClassName(size?: InputTextSize) {
  if (size === "medium") {
    return undefined;
  } else if (size === "large") {
    return "p-inputtext-lg"
  } else {
    return "p-inputtext-sm";
  }
}

export function InputText(props: IInputTextProps) {
  const {style, loading, name, type, placeholder, value, error, size, onChange} = props;

  const className = GetClassName(size);

  const disabled = loading !== undefined
      ? loading
      : false;

  return (
      <div style={{marginBottom: "3px"}} className="pc-input">
        <PrimeInputText
            style={{...style, display: "block", width: "100%"}}
						type={type}
            className={className}
            disabled={disabled}
            name={name}
            placeholder={placeholder}
            value={value}
            onChange={onChange}/>
        <ValidationError error={error}/>
      </div>
  );
}