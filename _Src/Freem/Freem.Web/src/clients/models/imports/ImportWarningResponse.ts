export class ImportWarningResponse {
  public readonly lineNumber: number;
  public readonly message: string;

  constructor(lineNumber: number, message: string) {
    this.lineNumber = lineNumber;
    this.message = message;
  }
}
