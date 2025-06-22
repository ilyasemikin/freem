import {ReactNode, useEffect, useState} from "react";
import {MeResponse} from "../../clients/models/users/MeResponse.ts";
import {useBackendClients} from "../clients/useBackendClients.ts";
import {AuthorizationContext} from "./AuthorizationContext.ts";
import {UserEntity} from "../../entities/UserEntity.ts";

export interface IAuthorizationProviderProps {
  children: ReactNode;
}

export function AuthorizationProvider(props: IAuthorizationProviderProps) {
  const {children} = props;

  const [user, setUser] = useState<UserEntity | undefined>(undefined);
  const [loading, setLoading] = useState(true);

  const clients = useBackendClients();

  async function me() {
    setLoading(true);

    const response = await clients.user.me();
    if (response.ok) {
      const data: MeResponse = await response.json();

      const user = new UserEntity(data.userId, data.nickname);
      setUser(user);
    } else {
      setUser(undefined);
    }

    setLoading(false);
  }

  const login = async () => {
    await me();
  }

  const logout = async () => {
    const response = await clients.authorization.logout();
    if (!response.ok) {
      throw new Error(response.statusText);
    }

    setUser(undefined);
  }

  useEffect(() => {
    me();
  }, []);

  return (
      <AuthorizationContext.Provider value={{user, loading, login, logout}}>
        {children}
      </AuthorizationContext.Provider>
  );
}