import {TimeStatistics} from "./TimeStatistics.ts";

export class StatisticsPerPeriodResponse {
  public readonly statistics: TimeStatistics;

  public constructor(statistics: TimeStatistics) {
    this.statistics = statistics;
  }
}