import {IHttpClient} from "./http/IHttpClient.ts";
import {CredentialsRequest} from "./models/auth/CredentialsRequest.ts";

export class AuthClient {
  private readonly client: IHttpClient;

  public constructor(client: IHttpClient) {
    this.client = client;
  }

  public async register(request: CredentialsRequest): Promise<Response> {
    return await this.client.post({url: "api/v1/auth/register", body: request});
  }

  public async login(request: CredentialsRequest): Promise<Response> {
    return await this.client.post({url: "api/v1/auth/login", body: request});
  }

  public async logout(): Promise<Response> {
    return await this.client.post({url: "api/v1/auth/logout"});
  }
}
