import {IHttpClient} from "./http/IHttpClient.ts";
import {RegisterPasswordCredentialsRequest} from "./models/users/LoginPassword/RegisterPasswordCredentialsRequest.ts";
import {LoginPasswordCredentialsRequest} from "./models/users/LoginPassword/LoginPasswordCredentialsRequest.ts";

export class AuthorizationClient {
  private readonly client: IHttpClient;

  public constructor(client: IHttpClient) {
    this.client = client;
  }

  public async register(request: RegisterPasswordCredentialsRequest): Promise<Response> {
    return await this.client.post({url: "api/v1/user/password-credentials/register", body: request});
  }

  public async login(request: LoginPasswordCredentialsRequest): Promise<Response> {
    return await this.client.post({url: "api/v1/user/password-credentials/login/cookie", body: request});
  }

  public async refresh(): Promise<Response> {
    return await this.client.post({url: "api/v1/user/cookie-tokens/refresh"});
  }

  public async logout(): Promise<Response> {
    return await this.client.delete({url: "api/v1/user/cookie-tokens"});
  }
}