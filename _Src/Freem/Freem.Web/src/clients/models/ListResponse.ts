export class ListResponse<T> {
  public readonly items: T[];
  public readonly count: number;
  public readonly totalCount: number;

  public constructor(items: T[], count: number, totalCount: number) {
    this.items = items;
    this.count = count;
    this.totalCount = totalCount;
  }

  // A page of a list endpoint: items in the body, counts in the X-ITEMS-COUNT and X-TOTAL-ITEMS-COUNT headers.
  public static async read<T>(response: Response): Promise<ListResponse<T>> {
    const items: T[] = await response.json();
    const count = Number(response.headers.get("X-ITEMS-COUNT") ?? items.length);
    const totalCount = Number(response.headers.get("X-TOTAL-ITEMS-COUNT") ?? items.length);
    return new ListResponse(items, count, totalCount);
  }
}