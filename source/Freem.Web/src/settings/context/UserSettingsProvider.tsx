import {ReactNode, useEffect, useMemo, useState} from "react";
import {IUserSettingsChanges, IUserSettingsContext, UserSettingsContext} from "./UserSettingsContext.ts";
import {useBackendClients} from "../../clients/context/useBackendClients.ts";
import {TimeZoneOffset} from "../../data/TimeZoneOffset.ts";
import {WeekStart} from "../../data/WeekStart.ts";
import {UserSettingsResponse} from "../../clients/models/users/Settings/UserSettingsResponse.ts";
import {UpdateUserSettingsRequest} from "../../clients/models/users/Settings/UpdateUserSettingsRequest.ts";
import {UpdateField} from "../../clients/models/UpdateField.ts";
import {Loading} from "../../components/Loading.tsx";
import {ValidationError} from "../../components/ValidationError.tsx";

interface IUserSettingsProviderProps {
  children: ReactNode;
}

// Everything that shows or reads time depends on the settings, so children are rendered only after they are loaded.
export function UserSettingsProvider({children}: IUserSettingsProviderProps) {
  const {user} = useBackendClients();

  const [timeZoneOffset, setTimeZoneOffset] = useState<TimeZoneOffset | undefined>();
  const [weekStart, setWeekStart] = useState<WeekStart | undefined>();
  const [error, setError] = useState<string | undefined>();

  useEffect(() => {
    let current = true;

    async function load() {
      const response = await user.getSettings();
      if (!response.ok) {
        throw new Error(response.statusText);
      }

      const body: UserSettingsResponse = await response.json();
      if (current) {
        setTimeZoneOffset(TimeZoneOffset.parse(body.timeZoneOffset));
        setWeekStart(WeekStart.parse(body.weekStart));
        setError(undefined);
      }
    }

    load().catch(error => {
      if (current) {
        setError(error instanceof Error ? error.message : "Failed to load settings");
      }
    });

    return () => {
      current = false;
    };
  }, [user]);

  const context = useMemo<IUserSettingsContext | undefined>(() => {
    if (timeZoneOffset === undefined || weekStart === undefined) {
      return undefined;
    }

    async function updateSettings(changes: IUserSettingsChanges) {
      const offsetText = changes.timeZoneOffset?.toString();
      const weekStartText = changes.weekStart?.toString();
      const request: UpdateUserSettingsRequest = {
        timeZoneOffset: offsetText === undefined ? undefined : new UpdateField(offsetText),
        weekStart: weekStartText === undefined ? undefined : new UpdateField(weekStartText)
      };
      const response = await user.updateSettings(request);
      if (!response.ok) {
        return "Failed to save settings";
      }

      if (changes.timeZoneOffset !== undefined) {
        setTimeZoneOffset(changes.timeZoneOffset);
      }
      if (changes.weekStart !== undefined) {
        setWeekStart(changes.weekStart);
      }
      return undefined;
    }

    return {timeZoneOffset, weekStart, updateSettings};
  }, [timeZoneOffset, weekStart, user]);

  if (context === undefined) {
    return error !== undefined
        ? <ValidationError error={error}/>
        : <Loading/>;
  }

  return (
      <UserSettingsContext.Provider value={context}>
        {children}
      </UserSettingsContext.Provider>
  );
}
