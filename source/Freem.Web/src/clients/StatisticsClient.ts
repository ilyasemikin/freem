import {IHttpClient} from "./http/IHttpClient.ts";
import {StatisticsPerPeriodRequest} from "./models/statistics/StatisticsPerPeriodRequest.ts";

export class StatisticsClient {
  private readonly client: IHttpClient;

  public constructor(client: IHttpClient) {
    this.client = client;
  }

  public async perPeriod(request: StatisticsPerPeriodRequest): Promise<Response> {
    return await this.client.get({url: "api/v1/statistics/per-period", query: request.period});
  }
}