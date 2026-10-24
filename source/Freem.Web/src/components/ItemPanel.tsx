import {CSSProperties, ReactNode} from "react";

export interface IItemPanelProps {
  children?: ReactNode;
  running?: boolean;
  color?: string;
}

export function ItemPanel(props: IItemPanelProps) {
  const {children, running, color} = props;

  const classNames = ["item", "item-row"];
  if (color !== undefined) {
    classNames.push("item--colored");
  }
  if (running) {
    classNames.push("item--running");
  }

  const style = color !== undefined
      ? {"--item-color": color} as CSSProperties
      : undefined;

  return (
      <div className={classNames.join(" ")} style={style} children={children}/>
  );
}
