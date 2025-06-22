import {IEntityResponse} from "../IEntityResponse.ts";

export class ActivityResponse implements IEntityResponse {
  public readonly id: string;
  public readonly name: string;
  public readonly status: string;
  public readonly tags?: string[];

  constructor(id: string, name: string, status: string, tags?: string[]) {
    this.id = id;
    this.name = name;
    this.status = status;
    this.tags = tags;
  }
}