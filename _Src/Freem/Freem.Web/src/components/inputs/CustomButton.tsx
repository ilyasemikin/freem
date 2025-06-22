import {CSSProperties, ReactNode} from "react";
import "./CustomButton.css"

export interface ICustomButtonProps {
  style?: CSSProperties;
  children?: ReactNode;
  onClick?: () => void;
}

export function CustomButton(props: ICustomButtonProps) {
  const {style, children, onClick} = props;

  return (
      <div
          style={{...style, display: "flex", justifyContent: "space-between", alignItems: "center"}}
          className="custom-button-area"
          children={children}
          onClick={onClick}/>
  );
}