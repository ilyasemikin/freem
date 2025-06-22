import {IEntityResponse} from "../IEntityResponse.ts";

export class TagResponse implements IEntityResponse {
  public readonly id: string;
  public readonly name: string;

  constructor(id: string, name: string) {
    this.id = id;
    this.name = name;
  }
}