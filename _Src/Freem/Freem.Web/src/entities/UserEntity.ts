export class UserEntity {
  public readonly id: string;
  public readonly nickname: string;

  public constructor(id: string, nickname: string) {
    this.id = id;
    this.nickname = nickname;
  }
}