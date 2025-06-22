import { UpdateField } from "../UpdateField";

export class UpdateActivityRequest {
  public readonly name?: UpdateField<string>;
  public readonly tags?: UpdateField<string[]>;

  constructor(name?: UpdateField<string>, tags?: UpdateField<string[]>) {
    this.name = name;
    this.tags = tags;
  }
}