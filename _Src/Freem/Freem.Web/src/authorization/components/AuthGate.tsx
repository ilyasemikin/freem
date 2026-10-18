import {ReactNode} from "react";
import {useAuth} from "../context/useAuth.ts";
import {Loading} from "../../components/Loading.tsx";
import {LoginPage} from "../../pages/login/LoginPage.tsx";

interface IAuthGateProps {
  children: ReactNode;
}

export function AuthGate({children}: IAuthGateProps) {
  const {status} = useAuth();

  if (status === "loading") {
    return <Loading/>;
  }

  if (status !== "authenticated") {
    return <LoginPage/>;
  }

  return children;
}
