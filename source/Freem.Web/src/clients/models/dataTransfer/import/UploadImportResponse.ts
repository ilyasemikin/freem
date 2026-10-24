import {ImportResponse} from "./ImportResponse.ts";
import {ImportWarningResponse} from "./ImportWarningResponse.ts";

export class UploadImportResponse {
  public readonly import: ImportResponse;
  public readonly warnings: ImportWarningResponse[];

  constructor(value: ImportResponse, warnings: ImportWarningResponse[]) {
    this.import = value;
    this.warnings = warnings;
  }
}
