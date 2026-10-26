import {DateHelper} from "../helpers/DateHelper.ts";

export class TimeDuration {
  public readonly seconds: number;

  public constructor(seconds: number) {
    this.seconds = seconds;
  }

  public toString(): string {
    const sign = this.seconds < 0 ? "-" : "";
    const total = Math.floor(Math.abs(this.seconds));
    const days = Math.floor(total / 60 / 60 / 24).toString();
    const hours = DateHelper.padPart(Math.floor(total / 60 / 60) % 24);
    const minutes = DateHelper.padPart(Math.floor(total / 60) % 60);
    const seconds = DateHelper.padPart(total % 60);

    const result = `${hours}:${minutes}:${seconds}`;
    return days !== "0"
        ? `${sign}${days}.${result}`
        : `${sign}${result}`;
  }

  // Format of .NET TimeSpan: [-][d.]hh:mm:ss[.fffffff]
  public static parse(input: string): TimeDuration {
    const negative = input.trim().startsWith("-");
    const unsigned = negative ? input.trim().substring(1) : input;
    const parts = unsigned.split(":").map(s => s.trim());

    if (parts.length !== 3) {
      throw new Error(`Invalid duration "${input}"`);
    }

    let total = 0;
    if (parts[0].length !== 2) {
      const sparts = parts[0].split(".").map(s => s.trim());
      if (sparts.length !== 2) {
        throw new Error(`Invalid duration "${input}"`);
      }

      const days = +sparts[0];
      total = days * 24 * 60 * 60;

      parts[0] = sparts[1];
    }

    const hours = +parts[0];
    const minutes = +parts[1];
    const seconds = +parts[2];

    total +=
        hours * 60 * 60 +
        minutes * 60 +
        seconds;

    if (isNaN(total)) {
      throw new Error(`Invalid duration "${input}"`);
    }

    return new TimeDuration(negative ? -total : total);
  }
}