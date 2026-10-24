import {ImportWarningResponse} from "../../clients/models/imports/ImportWarningResponse.ts";

// Passed from the upload to the import page: warnings are not stored with the import.
export interface IImportPageState {
  warnings?: ImportWarningResponse[];
}
