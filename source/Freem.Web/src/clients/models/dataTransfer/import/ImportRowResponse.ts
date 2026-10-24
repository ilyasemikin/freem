import {ImportRowPeriodResponse} from "./ImportRowPeriodResponse.ts";

export class ImportRowResponse {
  public readonly id: string;
  public readonly recordId: string;
  public readonly name: string | null;
  public readonly description: string | null;
  public readonly activityIds: string[];
  public readonly tagIds: string[];
  public readonly periods: ImportRowPeriodResponse[];

  constructor(
      id: string,
      recordId: string,
      name: string | null,
      description: string | null,
      activityIds: string[],
      tagIds: string[],
      periods: ImportRowPeriodResponse[]) {
    this.id = id;
    this.recordId = recordId;
    this.name = name;
    this.description = description;
    this.activityIds = activityIds;
    this.tagIds = tagIds;
    this.periods = periods;
  }
}
