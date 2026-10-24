export class UploadImportRequest {
  public readonly format: string;
  public readonly file: File;
  public readonly timeZoneOffset?: string;

  constructor(format: string, file: File, timeZoneOffset?: string) {
    this.format = format;
    this.file = file;
    this.timeZoneOffset = timeZoneOffset;
  }
}
