import {ReactNode} from "react";

export interface IDialogWindowProps {
  children?: ReactNode;
}

export function DialogWindow(props: IDialogWindowProps) {
  const {children} = props;

  return (
      <div style={{backgroundColor: "white", padding: "24px", borderRadius: "var(--radius)", boxShadow: "0 12px 40px rgba(16, 24, 40, 0.2)"}}>
        {children}
      </div>
  );
}
