import {ReactNode} from "react";

export interface IItemBorderProps {
  children?: ReactNode;
}

export function ItemBorder(props: IItemBorderProps) {
  const {children} = props;

  return (
      <div className="item" children={children}/>
  )
}
