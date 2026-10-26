import {createContext} from "react";
import {TimeZoneOffset} from "../../data/TimeZoneOffset.ts";
import {WeekStart} from "../../data/WeekStart.ts";

// Fields that are omitted stay unchanged
export interface IUserSettingsChanges {
  timeZoneOffset?: TimeZoneOffset;
  weekStart?: WeekStart;
}

export interface IUserSettingsContext {
  timeZoneOffset: TimeZoneOffset;
  weekStart: WeekStart;

  // Returns an error message, or undefined on success.
  updateSettings: (changes: IUserSettingsChanges) => Promise<string | undefined>;
}

export const UserSettingsContext = createContext<IUserSettingsContext>(undefined!);
