import {ErrorBody, ErrorResponse} from "../models/ErrorResponse.ts";

// A failed response with the error from its body, when the body is an ErrorResponse.
export class ApiError extends Error {
  public readonly status: number;
  public readonly body?: ErrorBody;

  constructor(status: number, message: string, body?: ErrorBody) {
    super(message);
    this.status = status;
    this.body = body;
  }

  public get type(): string | undefined {
    return this.body?.type;
  }

  public static async fromResponse(response: Response): Promise<ApiError> {
    try {
      const value: ErrorResponse = await response.json();
      if (value?.error?.type !== undefined) {
        return new ApiError(response.status, value.error.message, value.error);
      }
    } catch {
      // The body is not JSON: fall back to the status text.
    }

    return new ApiError(response.status, response.statusText);
  }
}
