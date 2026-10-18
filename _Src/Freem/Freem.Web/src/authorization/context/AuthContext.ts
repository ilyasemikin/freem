import {createContext} from "react";

export type AuthStatus = "loading" | "anonymous" | "unregistered" | "authenticated";

export interface IAuthContext {
  status: AuthStatus;

  // Each action returns an error message, or undefined on success.
  login: (username: string, password: string) => Promise<string | undefined>;
  register: (username: string, password: string, nickname: string) => Promise<string | undefined>;
  completeRegistration: (nickname: string) => Promise<string | undefined>;
  logout: () => Promise<void>;
}

export const AuthContext = createContext<IAuthContext>(undefined!);
