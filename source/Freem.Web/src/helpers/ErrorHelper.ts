import {ApiError} from "../clients/http/ApiError.ts";
import {ErrorTypes} from "../clients/models/ErrorTypes.ts";

const messages: Record<string, string> = {
  [ErrorTypes.ActivityNameAlreadyExists]: "An activity with this name already exists",
  [ErrorTypes.TagNameAlreadyExists]: "A tag with this name already exists"
};

export class ErrorHelper {
  // Text for the user: known API errors get a readable message, an empty message falls back
  public static getMessage(error: unknown, fallback: string): string {
    if (error instanceof ApiError && error.type !== undefined && messages[error.type] !== undefined) {
      return messages[error.type];
    }

    if (error instanceof Error && error.message !== "") {
      return error.message;
    }

    return fallback;
  }

  public static async throwIfFailed(response: Response): Promise<void> {
    if (!response.ok) {
      throw await ApiError.fromResponse(response);
    }
  }
}
