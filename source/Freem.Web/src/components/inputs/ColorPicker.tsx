import {ChangeEvent} from "react";
import {ActivityColor} from "../../helpers/ActivityColor.ts";
import "./ColorPicker.css"

export interface IColorPickerProps {
  value: string;
  disabled?: boolean;
  onChange?: (value: string) => void;
}

export function ColorPicker(props: IColorPickerProps) {
  const {value, disabled, onChange} = props;

  const custom = !ActivityColor.isFromPalette(value);

  function changeCustom(e: ChangeEvent<HTMLInputElement>) {
    onChange?.(e.target.value);
  }

  return (
      <div className="color-picker" role="radiogroup" aria-label="Color">
        {ActivityColor.palette.map(color =>
            <button
                key={color}
                type="button"
                role="radio"
                aria-checked={color === value.toLowerCase()}
                aria-label={color}
                disabled={disabled}
                className="color-picker-swatch"
                style={{background: color}}
                onClick={() => onChange?.(color)}/>)}
        <label
            className={custom ? "color-picker-custom color-picker-custom--selected" : "color-picker-custom"}
            style={custom ? {background: value} : undefined}
            aria-label="Custom color">
          {!custom && <span className="pi pi-plus"/>}
          <input
              type="color"
              value={value}
              disabled={disabled}
              onChange={changeCustom}/>
        </label>
      </div>
  );
}
