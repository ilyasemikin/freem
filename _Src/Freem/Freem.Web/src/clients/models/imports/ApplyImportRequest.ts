// Exactly one of the fields is sent: all rows or the listed ones.
export class ApplyImportRequest {
  public readonly all?: boolean;
  public readonly rowIds?: string[];

  private constructor(all?: boolean, rowIds?: string[]) {
    this.all = all;
    this.rowIds = rowIds;
  }

  public static allRows(): ApplyImportRequest {
    return new ApplyImportRequest(true, undefined);
  }

  public static selectedRows(rowIds: string[]): ApplyImportRequest {
    return new ApplyImportRequest(undefined, rowIds);
  }
}
