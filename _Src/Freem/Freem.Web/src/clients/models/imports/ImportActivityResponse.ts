export class ImportActivityResponse {
  public readonly id: string;
  public readonly name: string;
  public readonly color: string | null;
  public readonly status: string;
  public readonly tagIds: string[];
  public readonly mappedActivityId: string | null;

  constructor(
      id: string,
      name: string,
      color: string | null,
      status: string,
      tagIds: string[],
      mappedActivityId: string | null) {
    this.id = id;
    this.name = name;
    this.status = status;
    this.tagIds = tagIds;
    this.color = color;
    this.mappedActivityId = mappedActivityId;
  }
}
