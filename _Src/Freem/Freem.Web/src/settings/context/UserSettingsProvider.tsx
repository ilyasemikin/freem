import {ReactNode, useEffect, useMemo, useState} from "react";
import {IUserSettingsContext, UserSettingsContext} from "./UserSettingsContext.ts";
import {useBackendClients} from "../../clients/context/useBackendClients.ts";
import {TimeZoneOffset} from "../../data/TimeZoneOffset.ts";
import {UserSettingsResponse} from "../../clients/models/users/Settings/UserSettingsResponse.ts";
import {UpdateUserSettingsRequest} from "../../clients/models/users/Settings/UpdateUserSettingsRequest.ts";
import {UpdateField} from "../../clients/models/UpdateField.ts";
import {Loading} from "../../components/Loading.tsx";
import {ValidationError} from "../../components/ValidationError.tsx";

interface IUserSettingsProviderProps {
  children: ReactNode;
}

// Everything that shows or reads time depends on the offset, so children are rendered only after it is loaded.
export function UserSettingsProvider({children}: IUserSettingsProviderProps) {
  const {user} = useBackendClients();

  const [timeZoneOffset, setTimeZoneOffset] = useState<TimeZoneOffset | undefined>();
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
    if (timeZoneOffset === undefined) {
      return undefined;
    }

    async function updateTimeZoneOffset(value: TimeZoneOffset) {
      const text = value.toString();
      const field = new UpdateField(text);
      const request: UpdateUserSettingsRequest = {timeZoneOffset: field};
      const response = await user.updateSettings(request);
      if (!response.ok) {
        return "Failed to save settings";
      }

      setTimeZoneOffset(value);
      return undefined;
    }

    return {timeZoneOffset, updateTimeZoneOffset};
  }, [timeZoneOffset, user]);

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
