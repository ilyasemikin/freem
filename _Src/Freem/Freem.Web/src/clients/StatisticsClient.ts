import {IHttpClient} from "./http/IHttpClient.ts";
import {StatisticsPerDaysRequest} from "./models/statistics/StatisticsPerDaysRequest.ts";
import {StatisticsPerPeriodRequest} from "./models/statistics/StatisticsPerPeriodRequest.ts";

export class StatisticsClient {
  private readonly client: IHttpClient;

  public constructor(client: IHttpClient) {
    this.client = client;
  }

  public async perDays(request: StatisticsPerDaysRequest): Promise<Response> {
    return await this.client.get({url: "api/v1/statistics/per-days", query: request});
  }

  public async perPeriod(request: StatisticsPerPeriodRequest): Promise<Response> {
    return await this.client.get({url: "api/v1/statistics/per-period", query: request.period});
  }
}