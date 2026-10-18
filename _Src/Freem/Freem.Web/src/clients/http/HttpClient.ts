import {IHttpMethodProps} from "./IHttpMethodProps.ts";
import {IHttpClient} from "./IHttpClient.ts";
import {AuthSession} from "../AuthSession.ts";

export class HttpClient implements IHttpClient{
  private readonly baseUrl?: string;

  private readonly session?: AuthSession;

  constructor(baseUrl?: string, session?: AuthSession) {
    this.baseUrl = baseUrl;
    this.session = session;
  }

  public async post<TBody>(props?: IHttpMethodProps<TBody>): Promise<Response> {
    return this.send("POST", props);
  }

  public async put<TBody>(props?: IHttpMethodProps<TBody>): Promise<Response> {
    return this.send("PUT", props);
  }

  public async delete<TBody>(props?: IHttpMethodProps<TBody>): Promise<Response> {
    return this.send("DELETE", props);
  }

  public async get<TBody>(props?: IHttpMethodProps<TBody>): Promise<Response> {
    return this.send("GET", props);
  }

  private buildUrl<TBody>(props?: IHttpMethodProps<TBody>): string {
    const {url, query} = props || {};

    if (!query) {
      return `${this.baseUrl}${url === undefined ? "" : `/${url}`}`;
    }

    const params = new URLSearchParams();
    const values = query as Record<string, unknown>;
    for (const key of Object.keys(values)) {
      const item = values[key];
      if (Array.isArray(item)) {
        for (const value of item) {
          params.append(key, String(value));
        }
      } else if (item != null) {
        params.append(key, String(item));
      }
    }

    return `${this.baseUrl}${url === undefined ? "" : `/${url}`}?${params}`;
  }

  private getRequestInfo<TBody>(method: string, props?: IHttpMethodProps<TBody>): RequestInit {
    const {body} = props || {};
    const headers: Record<string, string> = {...props?.headers};

    const accessToken = this.session?.getAccessToken();
    if (accessToken !== undefined) {
      headers["Authorization"] = `Bearer ${accessToken}`;
    }

    if (body !== undefined) {
      return {
        method: method,
        headers: {
          ...headers,
          "Content-Type": "application/json"
        },
        body: JSON.stringify(body),
        credentials: "include"
      }
    }

    return {
      method: method,
      headers: headers,
      credentials: "include"
    }
  }

  private async send<TBody>(method: string, props?: IHttpMethodProps<TBody>): Promise<Response> {
    const url = this.buildUrl(props);
    const response = await fetch(url, this.getRequestInfo(method, props));

    const isAuthRequest = props?.url?.startsWith("api/v1/auth/") ?? false;
    if (response.status !== 401 || this.session === undefined || isAuthRequest || props?.skipRefresh) {
      return response;
    }

    // The access token has expired: rotate the session and repeat the request once.
    if (!await this.session.refresh()) {
      this.session.onSignedOut?.();
      return response;
    }

    return await fetch(url, this.getRequestInfo(method, props));
  }
}