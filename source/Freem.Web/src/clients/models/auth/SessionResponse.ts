export class SessionResponse {
  public readonly accessToken: string;
  public readonly accessTokenExpiresAt: string;

  constructor(accessToken: string, accessTokenExpiresAt: string) {
    this.accessToken = accessToken;
    this.accessTokenExpiresAt = accessTokenExpiresAt;
  }
}
