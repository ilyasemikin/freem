import {IHttpMethodProps} from "./IHttpMethodProps.ts";
import {IHttpClient} from "./IHttpClient.ts";

export class HttpClient implements IHttpClient{
  private readonly baseUrl?: string;

  constructor(baseUrl?: string) {
    this.baseUrl = baseUrl;
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
    for (const key of Object.keys(query)) {
      const item = query[key];
      if (Array.isArray(item)) {
        for (const value of item) {
          params.append(key, value);
        }
      } else if (item != null) {
        params.append(key, item);
      }
    }

    return `${this.baseUrl}${url === undefined ? "" : `/${url}`}?${params}`;
  }

  private getRequestInfo<TBody>(method: string, props?: IHttpMethodProps<TBody>): RequestInit {
    const {body, headers} = props || {};

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
    const requestInfo = this.getRequestInfo(method, props);
    return await fetch(url, requestInfo);
  }
}