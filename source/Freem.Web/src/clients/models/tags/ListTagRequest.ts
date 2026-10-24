export class ListTagRequest {
  public readonly offset: number;
  public readonly limit: number;

  constructor(offset: number, limit: number) {
    this.offset = offset;
    this.limit = limit;
  }
}