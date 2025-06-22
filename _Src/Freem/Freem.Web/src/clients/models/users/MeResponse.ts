export class MeResponse {
  public readonly userId: string;
  public readonly nickname: string;

  constructor(userId: string, nickname: string) {
    this.userId = userId;
    this.nickname = nickname;
  }
}