import {ReactNode} from "react";

export interface IPasswordCredentialsPanelProps {
  children?: ReactNode
}

export function AuthorizationPanel(props: IPasswordCredentialsPanelProps) {
  const {children} = props;

  return (
      <div style={{width: "400px"}}>
        {children}
      </div>
  );
}