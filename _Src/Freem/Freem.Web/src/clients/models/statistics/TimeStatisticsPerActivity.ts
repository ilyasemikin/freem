export class TimeStatisticsPerActivity {
  public period: string;

  public id: string;
  public recordedTime: string;

  public constructor(period: string, activityId: string, recordedTime: string) {
    this.period = period;

    this.id = activityId;
    this.recordedTime = recordedTime;
  }
}