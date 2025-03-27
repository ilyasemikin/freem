export class RegisterPasswordCredentialsRequest {
  public readonly nickname: string;
  public readonly login: string;
  public readonly password: string;

  constructor(nickname: string, login: string, password: string) {
    this.nickname = nickname;
    this.login = login;
    this.password = password;
  }
}