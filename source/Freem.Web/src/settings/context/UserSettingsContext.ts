import {createContext} from "react";
import {TimeZoneOffset} from "../../data/TimeZoneOffset.ts";

export interface IUserSettingsContext {
  timeZoneOffset: TimeZoneOffset;

  // Returns an error message, or undefined on success.
  updateTimeZoneOffset: (value: TimeZoneOffset) => Promise<string | undefined>;
}

export const UserSettingsContext = createContext<IUserSettingsContext>(undefined!);
