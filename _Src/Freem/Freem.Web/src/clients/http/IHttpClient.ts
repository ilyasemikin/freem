import {IHttpMethodProps} from "./IHttpMethodProps.ts";

export interface IHttpClient {
  post<TBody>(props?: IHttpMethodProps<TBody>): Promise<Response>;
  put<TBody>(props?: IHttpMethodProps<TBody>): Promise<Response>;
  delete<TBody>(props?: IHttpMethodProps<TBody>): Promise<Response>;
  get<TBody>(props?: IHttpMethodProps<TBody>): Promise<Response>;
}