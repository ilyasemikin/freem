export class RecordPeriodRequest {
  public readonly period: string;
  public readonly description?: string;

  constructor(period: string, description?: string) {
    this.period = period;
    this.description = description;
  }
}
