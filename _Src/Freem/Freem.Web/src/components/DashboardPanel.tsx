import {CSSProperties, ReactNode} from "react";
import {Divider} from "./Divider.tsx";

export interface IDashboardPanelProps {
  style?: CSSProperties;
  children?: ReactNode;
  footer?: ReactNode;
}

export function DashboardPanel(props: IDashboardPanelProps) {
  const {style, children, footer} = props;

  return (
      <div
          style={{
            ...style,
            display: "flex",
            flexDirection: "column",
            margin: "5px",
            border: "solid 1px lightgray",
            borderRadius: "7px",
            overflow: "hidden",
            boxSizing: "border-box"
          }}>
        <div
            style={{
              flex: "1",
              width: "100%",
              overflowY: "auto",
              padding: "10px"
            }}>
          {children}
        </div>
        {footer &&
            <div style={{display: "flex", flexDirection: "column"}}>
                <Divider/>
                <div style={{display: "flex", justifyContent: "center", padding: "5px 5px 10px 5px"}}>
                  {footer}
                </div>
            </div>}
      </div>
  );
}