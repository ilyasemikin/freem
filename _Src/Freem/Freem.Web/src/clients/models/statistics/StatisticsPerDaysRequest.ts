export class StatisticsPerDaysRequest {
  public readonly period: string;

  public constructor(period: string) {
    this.period = period;
  }
}