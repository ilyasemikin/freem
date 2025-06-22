import {useContext} from "react";
import {AuthorizationContext} from "./AuthorizationContext.ts";

export function useAuthorization() {
  return useContext(AuthorizationContext)
}