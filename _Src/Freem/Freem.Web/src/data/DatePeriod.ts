export class DatePeriod {
  readonly start: Date;
  readonly end: Date;

  public constructor(start: Date, end: Date) {
    const normalizedStart = DatePeriod.stripTime(start);
    const normalizedEnd = DatePeriod.stripTime(end);

    if (normalizedStart > normalizedEnd) {
      throw new Error("Start date must be before or equal to end date.");
    }

    this.start = normalizedStart;
    this.end = normalizedEnd;
  }

  public toString(): string {
    return `${this.formatDate(this.start)}, ${this.formatDate(this.end)}`;
  }

  public getDurationInDays(): number {
    const msPerDay = 1000 * 60 * 60 * 24;
    return (this.end.getTime() - this.start.getTime()) / msPerDay;
  }

  public static parse(input: string): DatePeriod {
    const parts = input.split(',').map(s => s.trim());
    if (parts.length !== 2) {
      throw new Error(`Invalid format. Expected "YYYY-MM-DD, YYYY-MM-DD"`);
    }

    const [startStr, endStr] = parts;
    const start = new Date(startStr);
    const end = new Date(endStr);

    if (isNaN(start.getTime()) || isNaN(end.getTime())) {
      throw new Error("Invalid date(s). Must be in YYYY-MM-DD format.");
    }

    return new DatePeriod(start, end);
  }

  private static stripTime(date: Date): Date {
    return new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()));
  }

  private formatDate(date: Date): string {
    return date.toISOString().split('T')[0]; // YYYY-MM-DD
  }
}