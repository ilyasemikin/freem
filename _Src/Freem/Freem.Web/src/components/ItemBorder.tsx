import {ReactNode} from "react";

export interface IItemBorderProps {
  children?: ReactNode;
}

export function ItemBorder(props: IItemBorderProps) {
  const {children} = props;

  return (
      <div
          style={{
            width: "100%",
            padding: "15px 10px",
            border: "solid 1px lightgray",
            borderRadius: "7px",
            marginBottom: "5px"
          }}
          children={children}/>
  )
}