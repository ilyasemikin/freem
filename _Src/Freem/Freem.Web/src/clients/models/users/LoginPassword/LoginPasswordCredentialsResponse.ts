import { UserTokens } from "../UserTokens";

export class LoginPasswordCredentialsResponse {
  public readonly userTokens: UserTokens;

  constructor(userTokens: UserTokens) {
    this.userTokens = userTokens;
  }
}