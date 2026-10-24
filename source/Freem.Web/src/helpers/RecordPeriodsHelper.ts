import {DateTimePeriod} from "../data/DateTimePeriod.ts";

export interface IPeriodRowState {
  key: string;
  id?: string;
  startAt?: Date;
  endAt?: Date;
  description: string;
}

export interface IPeriodRowData {
  period: DateTimePeriod;
  description?: string;
}

export class RecordPeriodsHelper {
  private static counter = 0;

  public static createRow(startAt?: Date): IPeriodRowState {
    RecordPeriodsHelper.counter++;
    return {key: `row-${RecordPeriodsHelper.counter}`, startAt: startAt, description: ""};
  }

  public static toRowData(row: IPeriodRowState): IPeriodRowData {
    if (row.startAt === undefined || row.endAt === undefined) {
      throw new Error("Period row is incomplete");
    }

    const period = new DateTimePeriod(row.startAt, row.endAt);
    const description = row.description !== "" ? row.description : undefined;
    return {period: period, description: description};
  }

  // Periods of a record must not overlap; touching is allowed
  public static validateRow(row: IPeriodRowState, others: IPeriodRowState[]): string | undefined {
    if (row.startAt === undefined) {
      return "Start at is required";
    }

    if (row.endAt === undefined) {
      return "End at is required";
    }

    if (row.endAt <= row.startAt) {
      return "End at must be after start at";
    }

    for (const other of others) {
      if (other.key === row.key || other.startAt === undefined || other.endAt === undefined) {
        continue;
      }

      if (row.startAt < other.endAt && other.startAt < row.endAt) {
        return "Period overlaps another period";
      }
    }

    return undefined;
  }

  public static validateRows(rows: IPeriodRowState[]): Record<string, string> {
    const errors: Record<string, string> = {};
    for (const row of rows) {
      const error = RecordPeriodsHelper.validateRow(row, rows);
      if (error !== undefined) {
        errors[row.key] = error;
      }
    }

    return errors;
  }
}
