import {FormatResponse} from "./FormatResponse.ts";

export class ListFormatsResponse {
  public readonly exportFormats: FormatResponse[];
  public readonly importFormats: FormatResponse[];

  constructor(exportFormats: FormatResponse[], importFormats: FormatResponse[]) {
    this.exportFormats = exportFormats;
    this.importFormats = importFormats;
  }
}
