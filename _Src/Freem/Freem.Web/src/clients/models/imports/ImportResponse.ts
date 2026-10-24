export class ImportResponse {
  public readonly id: string;
  public readonly format: string;
  public readonly fileName: string;
  public readonly timeZoneOffset: string;
  public readonly status: string;
  public readonly rowsInFile: number;
  public readonly rowsParsed: number;

  constructor(
      id: string,
      format: string,
      fileName: string,
      timeZoneOffset: string,
      status: string,
      rowsInFile: number,
      rowsParsed: number) {
    this.id = id;
    this.format = format;
    this.fileName = fileName;
    this.timeZoneOffset = timeZoneOffset;
    this.status = status;
    this.rowsInFile = rowsInFile;
    this.rowsParsed = rowsParsed;
  }
}
