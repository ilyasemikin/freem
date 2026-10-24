import {IHttpClient} from "./http/IHttpClient.ts";
import {UpdateUserSettingsRequest} from "./models/users/Settings/UpdateUserSettingsRequest.ts";

export class UserClient {
  private readonly client: IHttpClient;

  public constructor(client: IHttpClient) {
    this.client = client;
  }

  public async getSettings(): Promise<Response> {
    return await this.client.get({url: "api/v1/user/settings"})
  }

  public async updateSettings(request: UpdateUserSettingsRequest): Promise<Response> {
    return await this.client.put({url: "api/v1/user/settings", body: request});
  }

  public async me(skipRefresh?: boolean): Promise<Response> {
    return await this.client.get({url: "api/v1/user/me", skipRefresh: skipRefresh});
  }

  public async register(request: {nickname: string}): Promise<Response> {
    return await this.client.post({url: "api/v1/user/register", body: request});
  }
}