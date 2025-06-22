import {ReactNode} from "react";

export interface IItemPanelProps {
  children?: ReactNode;
}

export function ItemPanel(props: IItemPanelProps) {
  const {children} = props;

  return (
      <div
          style={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
            width: "100%",
            padding: "15px 10px",
            border: "solid 1px lightgray",
            borderRadius: "7px",
            marginBottom: "5px"
          }}
          children={children}/>
  );
}