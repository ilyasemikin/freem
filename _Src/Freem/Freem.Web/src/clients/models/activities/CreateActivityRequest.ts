export class CreateActivityRequest {
  public readonly name: string;
  public readonly tags?: string[];

  constructor(name: string, tags?: string[]) {
    this.name = name;
    this.tags = tags;
  }
}