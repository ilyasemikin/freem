export class UpdatePasswordCredentialsRequest {
  public readonly oldPassword: string;
  public readonly newPassword: string;

  constructor(oldPassword: string, newPassword: string) {
    this.oldPassword = oldPassword;
    this.newPassword = newPassword;
  }
}