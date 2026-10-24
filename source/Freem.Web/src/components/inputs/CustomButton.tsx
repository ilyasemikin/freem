import {CSSProperties, KeyboardEvent, ReactNode, RefObject, useRef} from "react";
import {Tooltip} from "primereact/tooltip";
import "./CustomButton.css"

export interface ICustomButtonProps {
  style?: CSSProperties;
  children?: ReactNode;
  label?: string;
  onClick?: () => void;
}

export function CustomButton(props: ICustomButtonProps) {
  const {style, children, label, onClick} = props;
  const areaRef = useRef<HTMLDivElement>(null);

  function handleKeyDown(e: KeyboardEvent) {
    if (e.key === "Enter" || e.key === " ") {
      e.preventDefault();
      onClick?.();
    }
  }

  return (
      <>
        <div
            ref={areaRef}
            role="button"
            tabIndex={0}
            aria-label={label}
            style={{...style, display: "flex", alignItems: "center"}}
            className="custom-button-area"
            children={children}
            onClick={onClick}
            onKeyDown={handleKeyDown}/>
        {label && <Tooltip target={areaRef as RefObject<HTMLElement>} content={label} position="bottom" showDelay={400}/>}
      </>
  );
}
