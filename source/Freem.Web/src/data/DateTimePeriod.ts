export class DateTimePeriod {
  public readonly startAt: Date;
  public readonly endAt: Date;

  public duration: number;

  public constructor(startAt: Date, endAt: Date) {
    if (startAt > endAt) {
      throw new Error("Start date must be before end date.");
    }

    this.startAt = startAt;
    this.endAt = endAt;

    this.duration = Math.floor((+this.endAt - +this.startAt) / 1000);
  }

  public toString(): string {
    return `${this.startAt.toISOString()}, ${this.endAt.toISOString()}`;
  }

  public getDurationFormatted(): string {
    function pad(x: number) {
      return x.toString().padStart(2, "0");
    }

    const days = Math.floor(this.duration / 60 / 60 / 24).toString();
    const hours = pad(Math.floor(this.duration / 60 / 60) % 24);
    const minutes = pad(Math.floor(this.duration / 60) % 60);
    const seconds = pad(this.duration % 60);

    const result = `${hours}:${minutes}:${seconds}`;
    return days !== "0"
        ? `${days}.${result}`
        : result;
  }

  public static parse(input: string) {
    const parts = input.split(',').map(s => s.trim());
    if (parts.length !== 2) {
      throw new Error(`Invalid input format. Expected "Date, Date", got: "${input}"`);
    }

    const [startStr, endStr] = parts;
    const start = new Date(startStr);
    const end = new Date(endStr);

    if (isNaN(start.getTime()) || isNaN(end.getTime())) {
      throw new Error("Invalid date(s) provided. Dates must be in ISO 8601 format.");
    }

    return new DateTimePeriod(start, end);
  }
}