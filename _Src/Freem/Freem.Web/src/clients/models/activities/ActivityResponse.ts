import {IEntityResponse} from "../IEntityResponse.ts";

export class ActivityResponse implements IEntityResponse {
  public readonly id: string;
  public readonly name: string;
  public readonly color: string;
  public readonly status: string;
  public readonly tags?: string[];

  constructor(id: string, name: string, color: string, status: string, tags?: string[]) {
    this.id = id;
    this.name = name;
    this.color = color;
    this.status = status;
    this.tags = tags;
  }
}