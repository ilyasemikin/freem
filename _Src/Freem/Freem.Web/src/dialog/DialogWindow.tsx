import {ReactNode} from "react";

export interface IDialogWindowProps {
  children?: ReactNode;
}

export function DialogWindow(props: IDialogWindowProps) {
  const {children} = props;

  return (
      <div style={{backgroundColor: "white", padding: "25px", borderRadius: "8px"}}>
        {children}
      </div>
  );
}