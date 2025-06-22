export interface QueryParams {
  [key: string]: any;
}

export interface IHttpMethodProps<T> {
  url?: string;
  body?: T;
  headers?: Record<string, string>;
  query?: QueryParams;
}