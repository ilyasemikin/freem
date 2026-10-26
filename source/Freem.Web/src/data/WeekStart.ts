const DaysInWeek = 7;

export class WeekStart {
  public static readonly Monday = new WeekStart("Monday", 1, "Monday");
  public static readonly Sunday = new WeekStart("Sunday", 0, "Sunday");

  public static readonly Default = WeekStart.Monday;

  // Value used by the API
  public readonly value: string;
  // Number of the day as in Date.getUTCDay (Sunday is 0)
  public readonly day: number;
  public readonly label: string;

  private constructor(value: string, day: number, label: string) {
    this.value = value;
    this.day = day;
    this.label = label;
  }

  // Works with dates without time (00:00 UTC of the day), see TimeZoneHelper
  public getWeekStart(date: Date): Date {
    const diff = (date.getUTCDay() - this.day + DaysInWeek) % DaysInWeek;
    return new Date(Date.UTC(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate() - diff));
  }

  public equals(other: WeekStart): boolean {
    return this.value === other.value;
  }

  public toString(): string {
    return this.value;
  }

  public static parse(input: string): WeekStart {
    const result = WeekStart.all().find(item => item.value === input);
    if (result === undefined) {
      throw new Error(`Invalid week start: "${input}"`);
    }

    return result;
  }

  public static all(): WeekStart[] {
    return [WeekStart.Monday, WeekStart.Sunday];
  }
}
