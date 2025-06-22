export class ListResponse<T> {
  public readonly items: T[];
  public readonly count: number;
  public readonly totalCount: number;

  public constructor(items: T[], count: number, totalCount: number) {
    this.items = items;
    this.count = count;
    this.totalCount = totalCount;
  }
}