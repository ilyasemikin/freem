import {useContext} from "react";
import {BackendClientsContext} from "./BackendClientsContext.ts";

export function useBackendClients() {
  return useContext(BackendClientsContext)
}