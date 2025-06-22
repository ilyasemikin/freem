import {IHttpClient} from "./IHttpClient.ts";
import {IHttpMethodProps} from "./IHttpMethodProps.ts";

export class AuthorizedHttpClient implements IHttpClient {
  private readonly client: IHttpClient;
  private readonly refresh: () => Promise<Response>;

  public constructor(client: IHttpClient, refresh: () => Promise<Response>) {
    this.client = client;
    this.refresh = refresh;
  }

  public async post<TBody>(props?: IHttpMethodProps<TBody>): Promise<Response> {
    return await this.send(this.client.post.bind(this.client), props);
  }

  public async put<TBody>(props?: IHttpMethodProps<TBody>): Promise<Response> {
    return await this.send(this.client.put.bind(this.client), props);
  }

  public async delete<TBody>(props?: IHttpMethodProps<TBody>): Promise<Response> {
    return await this.send(this.client.delete.bind(this.client), props);
  }

  public async get<TBody>(props?: IHttpMethodProps<TBody>): Promise<Response> {
    return await this.send(this.client.get.bind(this.client), props);
  }

  private async send<TBody>(action: (props?: IHttpMethodProps<TBody>) => Promise<Response>, props?: IHttpMethodProps<TBody>): Promise<Response> {
    let response = await action(props);

    if (response.status !== 401) {
      return response;
    }

    const refreshResponse = await this.refresh();
    if (!refreshResponse.ok) {
      return response;
    }

    response = await action(props);
    return response;
  }
}