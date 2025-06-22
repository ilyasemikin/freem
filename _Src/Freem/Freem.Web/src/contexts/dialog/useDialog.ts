import {useContext} from "react";
import {DialogContext} from "./DialogContext.ts";

export function useDialog() {
  return useContext(DialogContext);
}