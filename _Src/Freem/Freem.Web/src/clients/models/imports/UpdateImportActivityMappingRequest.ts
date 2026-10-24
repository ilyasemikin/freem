export class UpdateImportActivityMappingRequest {
  // Existing activity; null removes the mapping.
  public readonly activityId: string | null;

  constructor(activityId: string | null) {
    this.activityId = activityId;
  }
}
