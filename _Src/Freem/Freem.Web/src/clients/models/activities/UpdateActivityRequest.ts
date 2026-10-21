import { UpdateField } from "../UpdateField";

export class UpdateActivityRequest {
  public readonly name?: UpdateField<string>;
  public readonly color?: UpdateField<string>;
  public readonly tags?: UpdateField<string[]>;

  constructor(name?: UpdateField<string>, color?: UpdateField<string>, tags?: UpdateField<string[]>) {
    this.name = name;
    this.color = color;
    this.tags = tags;
  }
}