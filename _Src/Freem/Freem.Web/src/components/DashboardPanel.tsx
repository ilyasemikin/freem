import {CSSProperties, ReactNode} from "react";

export interface IDashboardPanelProps {
  style?: CSSProperties;
  children?: ReactNode;
  footer?: ReactNode;
}

export function DashboardPanel(props: IDashboardPanelProps) {
  const {style, children, footer} = props;

  return (
      <div className="panel" style={style}>
        <div className="panel-body">
          {children}
        </div>
        {footer && <div className="panel-footer">{footer}</div>}
      </div>
  );
}
