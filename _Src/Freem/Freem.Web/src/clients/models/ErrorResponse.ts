export class ErrorResponse {
  public readonly type: string;
  public readonly message?: string;
  public readonly properties?: { [key: string]: string };

  constructor(type: string, message?: string, properties?: { [key: string]: string }) {
    this.type = type;
    this.message = message;
    this.properties = properties;
  }
}