export class CreateActivityRequest {
  public readonly name: string;
  public readonly color: string;
  public readonly tags?: string[];

  constructor(name: string, color: string, tags?: string[]) {
    this.name = name;
    this.color = color;
    this.tags = tags;
  }
}