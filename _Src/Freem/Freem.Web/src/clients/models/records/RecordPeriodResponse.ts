export class RecordPeriodResponse {
  public readonly id: string;
  public readonly period: string;
  public readonly description?: string;

  constructor(id: string, period: string, description?: string) {
    this.id = id;
    this.period = period;
    this.description = description;
  }
}
