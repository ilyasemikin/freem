export class ListRecordByPeriodRequest {
  public readonly period: string;
  public readonly limit?: number;

  constructor(period: string, limit?: number) {
    this.period = period;
    this.limit = limit;
  }
}