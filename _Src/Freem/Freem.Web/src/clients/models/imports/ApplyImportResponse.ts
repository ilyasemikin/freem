export class ApplyImportResponse {
  public readonly recordsCreated: number;
  public readonly skippedRowIds: string[];

  constructor(recordsCreated: number, skippedRowIds: string[]) {
    this.recordsCreated = recordsCreated;
    this.skippedRowIds = skippedRowIds;
  }
}
