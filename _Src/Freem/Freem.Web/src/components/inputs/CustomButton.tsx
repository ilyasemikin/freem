import {CSSProperties, KeyboardEvent, ReactNode} from "react";
import "./CustomButton.css"

export interface ICustomButtonProps {
  style?: CSSProperties;
  children?: ReactNode;
  onClick?: () => void;
}

export function CustomButton(props: ICustomButtonProps) {
  const {style, children, onClick} = props;

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
          style={{...style, display: "flex", alignItems: "center"}}
          className="custom-button-area"
          children={children}
          onClick={onClick}
          onKeyDown={handleKeyDown}/>
  );
}
