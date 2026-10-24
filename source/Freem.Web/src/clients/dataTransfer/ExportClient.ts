import {IHttpClient} from "../http/IHttpClient.ts";

export class ExportClient {
  private readonly client: IHttpClient;

  constructor(client: IHttpClient) {
    this.client = client;
  }

  public async export(format: string): Promise<Response> {
    const key = encodeURIComponent(format);
    return await this.client.get({url: `api/v1/data-transfer/exports/${key}`});
  }
}
