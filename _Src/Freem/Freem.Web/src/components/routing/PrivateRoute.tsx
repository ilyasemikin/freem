import {ReactNode} from "react";
import {Navigate} from "react-router";
import {Loading} from "../Loading.tsx";
import {useAuthorization} from "../../contexts/authorization/useAuthorization.ts";

interface IPrivateRouteProps {
  children: ReactNode;
}

export function PrivateRoute({children}: IPrivateRouteProps) {
  const {user, loading} = useAuthorization();

  if (loading) {
    return <Loading/>
  }

  return user !== undefined
      ? children
      : <Navigate to="/login"/>;
}