export class RecordPeriodResponse {
  public readonly id: string;
  public readonly period: string;
  public readonly timeZoneOffset: string;
  public readonly description?: string;

  constructor(id: string, period: string, timeZoneOffset: string, description?: string) {
    this.id = id;
    this.period = period;
    this.timeZoneOffset = timeZoneOffset;
    this.description = description;
  }
}
