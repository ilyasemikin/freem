import { UserTokens } from "../UserTokens";

export class RefreshTokensResponse {
  public readonly tokens: UserTokens;

  constructor(tokens: UserTokens) {
    this.tokens = tokens;
  }
}