export type QueryParams = object;

export interface IHttpMethodProps<T> {
  url?: string;
  body?: T;
  headers?: Record<string, string>;
  query?: QueryParams;
}