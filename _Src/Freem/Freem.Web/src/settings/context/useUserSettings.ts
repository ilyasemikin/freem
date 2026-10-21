import {useContext} from "react";
import {UserSettingsContext} from "./UserSettingsContext.ts";

export function useUserSettings() {
  return useContext(UserSettingsContext);
}
