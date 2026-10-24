import {TagEntity} from "./TagEntity.ts";

export class ActivityEntity {
  public readonly id: string;
  public readonly name: string;
  public readonly color: string;
  public readonly status: string;
  public readonly tags: TagEntity[];

  constructor(id: string, name: string, color: string, status: string, tags?: TagEntity[]) {
    this.id = id;
    this.name = name;
    this.color = color;
    this.status = status;
    this.tags = tags || [];
  }

  public get archived(): boolean {
    return this.status === "Archived";
  }
}