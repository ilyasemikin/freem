export class FormatResponse {
  public readonly key: string;
  public readonly name: string;
  public readonly fileExtension: string;
  public readonly mediaType: string;

  constructor(key: string, name: string, fileExtension: string, mediaType: string) {
    this.key = key;
    this.name = name;
    this.fileExtension = fileExtension;
    this.mediaType = mediaType;
  }
}
