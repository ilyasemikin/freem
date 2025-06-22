export class StopRunningRecordRequest {
  public readonly endAt?: string;

  constructor(endAt?: string) {
    this.endAt = endAt;
  }
}