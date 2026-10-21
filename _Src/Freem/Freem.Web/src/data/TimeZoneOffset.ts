export class TimeZoneOffset {
  public static readonly MinMinutes = -12 * 60;
  public static readonly MaxMinutes = 14 * 60;
  public static readonly StepMinutes = 15;

  public static readonly Default = new TimeZoneOffset(0);

  // Offset of the user's time zone from UTC: positive to the east (Moscow is +180)
  public readonly minutes: number;

  public constructor(minutes: number) {
    if (!Number.isInteger(minutes) || minutes < TimeZoneOffset.MinMinutes || minutes > TimeZoneOffset.MaxMinutes) {
      throw new Error(`Invalid time zone offset: ${minutes} minutes`);
    }

    this.minutes = minutes;
  }

  public get milliseconds(): number {
    return this.minutes * 60 * 1000;
  }

  // TimeSpan "c" format used by the API: "03:00:00", "-05:30:00"
  public toString(): string {
    return `${this.sign()}${this.hoursAndMinutes()}:00`;
  }

  public toDisplayString(): string {
    const sign = this.minutes < 0 ? "−" : "+";
    return `UTC${sign}${this.hoursAndMinutes()}`;
  }

  public equals(other: TimeZoneOffset): boolean {
    return this.minutes === other.minutes;
  }

  public static parse(input: string): TimeZoneOffset {
    const match = /^(-)?(\d{2}):(\d{2}):(\d{2})$/.exec(input);
    if (match === null || match[4] !== "00") {
      throw new Error(`Invalid time zone offset: "${input}"`);
    }

    const [, sign, hours, minutes] = match;
    const value = Number(hours) * 60 + Number(minutes);
    return new TimeZoneOffset(sign ? -value : value);
  }

  public static fromBrowser(date: Date = new Date()): TimeZoneOffset {
    // getTimezoneOffset is positive to the west; rounded to the step so the value is one of all()
    const minutes = -date.getTimezoneOffset();
    const rounded = Math.round(minutes / TimeZoneOffset.StepMinutes) * TimeZoneOffset.StepMinutes;
    const clamped = Math.min(Math.max(rounded, TimeZoneOffset.MinMinutes), TimeZoneOffset.MaxMinutes);
    return new TimeZoneOffset(clamped);
  }

  public static all(): TimeZoneOffset[] {
    const result: TimeZoneOffset[] = [];
    for (let minutes = TimeZoneOffset.MinMinutes; minutes <= TimeZoneOffset.MaxMinutes; minutes += TimeZoneOffset.StepMinutes) {
      result.push(new TimeZoneOffset(minutes));
    }

    return result;
  }

  private sign(): string {
    return this.minutes < 0 ? "-" : "";
  }

  private hoursAndMinutes(): string {
    const absolute = Math.abs(this.minutes);
    const hours = Math.floor(absolute / 60).toString().padStart(2, "0");
    const minutes = (absolute % 60).toString().padStart(2, "0");
    return `${hours}:${minutes}`;
  }
}
