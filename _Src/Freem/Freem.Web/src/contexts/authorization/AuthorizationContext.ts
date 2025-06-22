import {createContext} from "react";
import {UserEntity} from "../../entities/UserEntity.ts";

export interface IAuthorizationContext {
  user?: UserEntity;
  loading: boolean;
  login: () => Promise<void>;
  logout: () => Promise<void>;
}

export const AuthorizationContext = createContext<IAuthorizationContext>({
  loading: true,
  login: () => Promise.resolve(),
  logout: () => Promise.resolve(),
});
