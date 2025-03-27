import { LoginPasswordCredentialsRequest } from "./models/users/LoginPassword/LoginPasswordCredentialsRequest";
import { LoginPasswordCredentialsResponse } from "./models/users/LoginPassword/LoginPasswordCredentialsResponse";
import { RegisterPasswordCredentialsRequest } from "./models/users/LoginPassword/RegisterPasswordCredentialsRequest";
import { UpdatePasswordCredentialsRequest } from "./models/users/LoginPassword/UpdatePasswordCredentialsRequest";
import { UpdateUserSettingsRequest } from "./models/users/Settings/UpdateUserSettingsRequest";
import { UserSettingsResponse } from "./models/users/Settings/UserSettingsResponse";
import { RefreshTokensRequest } from "./models/users/Tokens/RefreshTokensRequest";
import { RefreshTokensResponse } from "./models/users/Tokens/RefreshTokensResponse";

export class UserClient {
  private readonly address: string;

  constructor(address: string) {
    this.address = address;
  }

  public async register(request: RegisterPasswordCredentialsRequest) {
    const body = JSON.stringify(request);

    const response = await fetch(`${this.address}/api/v1/user/password-credentials/register`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: body
    });

    if (!response.ok)
      throw new Error();
  }

  public async login(request: LoginPasswordCredentialsRequest): Promise<LoginPasswordCredentialsResponse> {
    const body = JSON.stringify(request);

    const response = await fetch(`${this.address}/api/v1/user/tokens/refresh`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: body
    });

    if (!response.ok)
      throw new Error();

    return await response.json() as LoginPasswordCredentialsResponse;
   }

   public async updatePasswordCredentials(request: UpdatePasswordCredentialsRequest) {
    const body = JSON.stringify(request);

    const response = await fetch(`${this.address}/api/v1/user/password-credentials`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json"
      },
      body: body
    });

    if (!response.ok)
      throw new Error();
   }

   public async updateSettings(request: UpdateUserSettingsRequest) {
    const body = JSON.stringify(request);

    const response = await fetch(`${this.address}/api/v1/user/settings`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json"
      },
      body: body
    })

    if (!response.ok)
      throw new Error();
   }

   public async getSettings(): Promise<UserSettingsResponse> {
    const response = await fetch(`${this.address}/api/v1/user/settings`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json"
      }
    });

    if (!response.ok)
      throw new Error();

    return await response.json() as UserSettingsResponse;
   }

   public async refreshTokens(request: RefreshTokensRequest): Promise<RefreshTokensResponse> {
    const body = JSON.stringify(request);

    const response = await fetch(`${this.address}/api/v1/user/tokens/refresh`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: body
    });

    if (!response.ok)
      throw new Error();

    return await response.json() as RefreshTokensResponse;
  }
}