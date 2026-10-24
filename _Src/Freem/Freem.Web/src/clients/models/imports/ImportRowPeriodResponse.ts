export class ImportRowPeriodResponse {
  public readonly period: string;
  public readonly timeZoneOffset: string;
  public readonly description: string | null;

  constructor(period: string, timeZoneOffset: string, description: string | null) {
    this.period = period;
    this.timeZoneOffset = timeZoneOffset;
    this.description = description;
  }
}
