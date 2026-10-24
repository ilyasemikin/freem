import {ReactNode, useEffect, useState} from "react";
import {AuthContext, AuthStatus, IAuthContext} from "./AuthContext.ts";
import {useBackendClients} from "../../clients/context/useBackendClients.ts";
import {UserClient} from "../../clients/UserClient.ts";
import {CredentialsRequest} from "../../clients/models/auth/CredentialsRequest.ts";

// The session is valid here, so 401 from "me" means the Freem user has not been registered yet.
async function detectStatus(user: UserClient): Promise<AuthStatus> {
  const response = await user.me(true);
  if (response.ok) {
    return "authenticated";
  }

  return response.status === 401 ? "unregistered" : "anonymous";
}

interface IAuthProviderProps {
  children: ReactNode;
}

export function AuthProvider({children}: IAuthProviderProps) {
  const {auth, user, session} = useBackendClients();

  const [status, setStatus] = useState<AuthStatus>("loading");

  useEffect(() => {
    const unsubscribe = session.onSignedOut(() => setStatus("anonymous"));

    async function restore() {
      if (!await session.refresh()) {
        setStatus("anonymous");
        return;
      }

      setStatus(await detectStatus(user));
    }

    restore().catch(() => setStatus("anonymous"));

    return unsubscribe;
  }, [session, user]);

  async function startSession(username: string, password: string): Promise<string | undefined> {
    const response = await auth.login(new CredentialsRequest(username, password));
    if (!response.ok) {
      return response.status === 401 ? "Invalid username or password" : "Login failed";
    }

    const body = await response.json();
    session.setAccessToken(body.accessToken);
    return undefined;
  }

  async function login(username: string, password: string) {
    const error = await startSession(username, password);
    if (error !== undefined) {
      return error;
    }

    setStatus(await detectStatus(user));
    return undefined;
  }

  async function completeRegistration(nickname: string) {
    const response = await user.register({nickname: nickname});
    if (!response.ok) {
      return response.status === 409 ? "User is already registered" : "Registration failed";
    }

    setStatus("authenticated");
    return undefined;
  }

  async function register(username: string, password: string, nickname: string) {
    const response = await auth.register(new CredentialsRequest(username, password));
    if (!response.ok) {
      return "Registration failed (the username may be taken or the password is too weak)";
    }

    const error = await startSession(username, password);
    if (error !== undefined) {
      return error;
    }

    return await completeRegistration(nickname);
  }

  async function logout() {
    await auth.logout();
    session.clear();
    setStatus("anonymous");
  }

  const value: IAuthContext = {status, login, register, completeRegistration, logout};

  return (
      <AuthContext value={value}>
        {children}
      </AuthContext>
  );
}
