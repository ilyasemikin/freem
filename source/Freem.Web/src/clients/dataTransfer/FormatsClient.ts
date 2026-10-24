import {IHttpClient} from "../http/IHttpClient.ts";

export class FormatsClient {
  private readonly client: IHttpClient;

  constructor(client: IHttpClient) {
    this.client = client;
  }

  public async list(): Promise<Response> {
    return await this.client.get({url: "api/v1/data-transfer/formats"});
  }
}
