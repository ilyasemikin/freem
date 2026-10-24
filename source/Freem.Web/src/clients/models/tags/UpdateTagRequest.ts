import { UpdateField } from "../UpdateField";

export class UpdateTagRequest {
  public readonly name?: UpdateField<string>;

  constructor(name: UpdateField<string>) {
    this.name = name;
  }
}