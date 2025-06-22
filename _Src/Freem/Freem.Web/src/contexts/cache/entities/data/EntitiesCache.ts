import {IEntityResponse} from "../../../../clients/models/IEntityResponse.ts";

export interface IEntitiesCacheListResult<T> {
  founded: T[];
  others: string[];
}

export class EntitiesCache<T extends IEntityResponse> {
  private readonly values: Record<string, T>;
  private readonly commit: (values: Record<string, T>) => void;

  public constructor(values: Record<string, T>, commit: (values: Record<string, T>) => void) {
    this.values = values;
    this.commit = commit;
  }

  public contains(id: string): boolean {
    return id in this.values;
  }

  public get(id: string): T {
    return this.values[id];
  }

  public list(ids: string[]): IEntitiesCacheListResult<T> {
    return {
      founded: ids.filter(id => this.contains(id)).map(id => this.get(id)),
      others: ids.filter(id => !this.contains(id))
    }
  }

  public append(values: T[]) {
    for (const value of values) {
      this.values[value.id] = value;
    }

    this.commit(this.values);
  }
}