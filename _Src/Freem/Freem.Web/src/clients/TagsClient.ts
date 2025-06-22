import {CreateTagRequest} from "./models/tags/CreateTagRequest.ts";
import {UpdateTagRequest} from "./models/tags/UpdateTagRequest.ts";
import {ListTagRequest} from "./models/tags/ListTagRequest.ts";
import {IHttpClient} from "./http/IHttpClient.ts";

export class TagsClient {
  private readonly client: IHttpClient;

  constructor(client: IHttpClient) {
    this.client = client;
  }

  public async create(request: CreateTagRequest): Promise<Response> {
    return await this.client.post({url: "api/v1/tags", body: request});
  }

  public async update(id: string, request: UpdateTagRequest): Promise<Response> {
    return await this.client.post({url: `api/v1/tags/${id}`, body: request});
  }

  public async remove(id: string): Promise<Response> {
    return await this.client.delete({url: `api/v1/tags/${id}`});
  }

  public async get(id: string): Promise<Response> {
    return await this.client.get({url: `api/v1/tags/${id}`});
  }

  public async findByName(name: string): Promise<Response> {
    return await this.client.get({url: `api/v1/tags/by-name/${name}`});
  }

  public async list(request: ListTagRequest): Promise<Response> {
    return await this.client.get({url: "api/v1/tags", query: request});
  }
}