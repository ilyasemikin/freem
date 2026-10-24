import {CreateActivityRequest} from "./models/activities/CreateActivityRequest.ts";
import {UpdateActivityRequest} from "./models/activities/UpdateActivityRequest.ts";
import {FindActivityByNameRequest} from "./models/activities/FindActivityByNameRequest.ts";
import {ListActivityRequest} from "./models/activities/ListActivityRequest.ts";
import {IHttpClient} from "./http/IHttpClient.ts";

export class ActivitiesClient {
  private readonly client: IHttpClient;

  constructor(client: IHttpClient) {
    this.client = client;
  }

  public async create(request: CreateActivityRequest): Promise<Response> {
    return await this.client.post({url: "api/v1/activities", body: request});
  }

  public async update(id: string, request: UpdateActivityRequest): Promise<Response> {
    return await this.client.put({url: `api/v1/activities/${id}`, body: request});
  }

  public async remove(id: string): Promise<Response> {
    return await this.client.delete({url: `api/v1/activities/${id}`});
  }

  public async archive(id: string): Promise<Response> {
    return await this.client.post({url: `api/v1/activities/${id}/archive`})
  }

  public async unarchive(id: string): Promise<Response> {
    return await this.client.post({url: `api/v1/activities/${id}/unarchive`});
  }

  public async get(id: string): Promise<Response> {
    return await this.client.get({url: `api/v1/activities/${id}`});
  }

  public async findByName(request: FindActivityByNameRequest): Promise<Response> {
    return await this.client.get({url: "api/v1/activities/by-name", query: request});
  }

  public async list(request: ListActivityRequest): Promise<Response> {
    return await this.client.get({url: "api/v1/activities", query: request});
  }
}