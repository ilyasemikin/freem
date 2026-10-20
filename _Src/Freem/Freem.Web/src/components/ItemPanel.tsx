import {ReactNode} from "react";

export interface IItemPanelProps {
  children?: ReactNode;
  running?: boolean;
}

export function ItemPanel(props: IItemPanelProps) {
  const {children, running} = props;

  const className = running ? "item item-row item--running" : "item item-row";

  return (
      <div className={className} children={children}/>
  );
}
