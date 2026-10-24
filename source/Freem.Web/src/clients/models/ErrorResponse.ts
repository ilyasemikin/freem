export class ErrorBody {
  public readonly type: string;
  public readonly message: string;
  public readonly properties: Record<string, string> | null;
  public readonly details: ErrorBody[] | null;

  constructor(type: string, message: string, properties: Record<string, string> | null, details: ErrorBody[] | null) {
    this.type = type;
    this.message = message;
    this.properties = properties;
    this.details = details;
  }
}

export class ErrorResponse {
  public readonly error: ErrorBody;

  constructor(error: ErrorBody) {
    this.error = error;
  }
}
