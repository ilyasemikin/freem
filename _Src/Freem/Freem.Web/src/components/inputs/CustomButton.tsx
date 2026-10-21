import {CSSProperties, KeyboardEvent, ReactNode} from "react";
import "./CustomButton.css"

export interface ICustomButtonProps {
  style?: CSSProperties;
  children?: ReactNode;
  label?: string;
  onClick?: () => void;
}

export function CustomButton(props: ICustomButtonProps) {
  const {style, children, label, onClick} = props;

  function handleKeyDown(e: KeyboardEvent) {
    if (e.key === "Enter" || e.key === " ") {
      e.preventDefault();
      onClick?.();
    }
  }

  return (
      <div
          role="button"
          tabIndex={0}
          aria-label={label}
          style={{...style, display: "flex", alignItems: "center"}}
          className="custom-button-area"
          children={children}
          onClick={onClick}
          onKeyDown={handleKeyDown}/>
  );
}
