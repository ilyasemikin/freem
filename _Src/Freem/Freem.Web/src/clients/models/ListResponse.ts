export class ListResponse<T> {
  public readonly items: T[];
  public readonly count: number;
  public readonly totalCount: number;

  public constructor(items: T[], count: number, totalCount: number) {
    this.items = items;
    this.count = count;
    this.totalCount = totalCount;
  }

  // A page of a list endpoint: the body holds the total count and the items under the given property.
  public static async read<T>(response: Response, property: string): Promise<ListResponse<T>> {
    const body = await response.json();
    const items: T[] = body[property];
    return new ListResponse(items, items.length, body.totalCount);
  }
}