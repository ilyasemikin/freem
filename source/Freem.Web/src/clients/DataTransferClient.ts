import {IHttpClient} from "./http/IHttpClient.ts";

export class DataTransferClient {
  private readonly client: IHttpClient;

  constructor(client: IHttpClient) {
    this.client = client;
  }

  public async listFormats(): Promise<Response> {
    return await this.client.get({url: "api/v1/formats"});
  }

  public async export(format: string): Promise<Response> {
    const key = encodeURIComponent(format);
    return await this.client.get({url: `api/v1/export/${key}`});
  }
}
