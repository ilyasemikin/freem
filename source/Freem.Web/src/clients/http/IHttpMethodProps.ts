export type QueryParams = object;

export interface IHttpMethodProps<T> {
  url?: string;
  body?: T;
  headers?: Record<string, string>;
  query?: QueryParams;
  // Do not rotate the session and repeat the request on 401.
  skipRefresh?: boolean;
}