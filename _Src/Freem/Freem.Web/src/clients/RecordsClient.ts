import {IHttpClient} from "./http/IHttpClient.ts";
import {CreateRecordRequest} from "./models/records/CreateRecordRequest.ts";
import {UpdateRecordRequest} from "./models/records/UpdateRecordRequest.ts";
import {StartRunningRecordRequest} from "./models/records/running/StartRunningRecordRequest.ts";
import {StopRunningRecordRequest} from "./models/records/running/StopRunningRecordRequest.ts";
import {UpdateRunningRecordRequest} from "./models/records/running/UpdateRunningRecordRequest.ts";
import {ListRecordRequest} from "./models/records/ListRecordRequest.ts";
import {ListRecordByPeriodRequest} from "./models/records/ListRecordByPeriodRequest.ts";

export class RecordsClient {
  private readonly client: IHttpClient;

  constructor(client: IHttpClient) {
    this.client = client;
  }

  public async create(request: CreateRecordRequest): Promise<Response> {
    return await this.client.post({url: "api/v1/records", body: request});
  }

  public async update(id: string, request: UpdateRecordRequest): Promise<Response> {
    return await this.client.put({url: `api/v1/records/${id}`, body: request});
  }

  public async remove(id: string): Promise<Response> {
    return await this.client.delete({url: `api/v1/records/${id}`});
  }

  public async start(request: StartRunningRecordRequest): Promise<Response> {
    return await this.client.post({url: "api/v1/records/running/start", body: request});
  }

  public async stop(request: StopRunningRecordRequest): Promise<Response> {
    return await this.client.post({url: "api/v1/records/running/stop", body: request});
  }

  public async updateRunning(request: UpdateRunningRecordRequest): Promise<Response> {
    return await this.client.put({url: "api/v1/records/running", body: request});
  }

  public async removeRunning(): Promise<Response> {
    return await this.client.delete({url: "api/v1/records/running"});
  }

  public async getRunning(): Promise<Response> {
    return await this.client.get({url: "api/v1/records/running"});
  }

  public async get(id: string): Promise<Response> {
    return await this.client.get({url: `api/v1/records/${id}`});
  }

  public async list(request: ListRecordRequest): Promise<Response> {
    return await this.client.get({url: "api/v1/records", query: request});
  }

  public async listByPeriod(request: ListRecordByPeriodRequest): Promise<Response> {
    return await this.client.get({url: "api/v1/records/by-period", query: request});
  }
}