import {TimeStatistics} from "./TimeStatistics.ts";

export class StatisticsPerDaysResponse {
  public readonly statistics: { [date: string]: TimeStatistics };

  public constructor(statistics: { [date: string]: TimeStatistics }) {
    this.statistics = statistics;
  }
}