export class ImportTagResponse {
  public readonly id: string;
  public readonly name: string;
  public readonly mappedTagId: string | null;

  constructor(id: string, name: string, mappedTagId: string | null) {
    this.id = id;
    this.name = name;
    this.mappedTagId = mappedTagId;
  }
}
