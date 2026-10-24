export class UpdateImportTagMappingRequest {
  // Existing tag; null removes the mapping.
  public readonly tagId: string | null;

  constructor(tagId: string | null) {
    this.tagId = tagId;
  }
}
