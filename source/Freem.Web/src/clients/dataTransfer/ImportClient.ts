import {IHttpClient} from "../http/IHttpClient.ts";
import {UploadImportRequest} from "../models/dataTransfer/import/UploadImportRequest.ts";
import {ListImportRequest} from "../models/dataTransfer/import/ListImportRequest.ts";
import {ListImportActivityRequest} from "../models/dataTransfer/import/ListImportActivityRequest.ts";
import {ListImportTagRequest} from "../models/dataTransfer/import/ListImportTagRequest.ts";
import {ListImportRowRequest} from "../models/dataTransfer/import/ListImportRowRequest.ts";
import {UpdateImportActivityMappingRequest} from "../models/dataTransfer/import/UpdateImportActivityMappingRequest.ts";
import {UpdateImportTagMappingRequest} from "../models/dataTransfer/import/UpdateImportTagMappingRequest.ts";
import {ApplyImportRequest} from "../models/dataTransfer/import/ApplyImportRequest.ts";

export class ImportClient {
  private readonly client: IHttpClient;

  constructor(client: IHttpClient) {
    this.client = client;
  }

  public async upload(request: UploadImportRequest): Promise<Response> {
    const form = new FormData();
    form.append("format", request.format);
    if (request.timeZoneOffset !== undefined) {
      form.append("timeZoneOffset", request.timeZoneOffset);
    }
    form.append("file", request.file);

    return await this.client.post({url: "api/v1/data-transfer/imports", body: form});
  }

  public async list(request: ListImportRequest): Promise<Response> {
    return await this.client.get({url: "api/v1/data-transfer/imports", query: request});
  }

  public async get(id: string): Promise<Response> {
    return await this.client.get({url: `api/v1/data-transfer/imports/${id}`});
  }

  public async listActivities(id: string, request: ListImportActivityRequest): Promise<Response> {
    return await this.client.get({url: `api/v1/data-transfer/imports/${id}/activities`, query: request});
  }

  public async listTags(id: string, request: ListImportTagRequest): Promise<Response> {
    return await this.client.get({url: `api/v1/data-transfer/imports/${id}/tags`, query: request});
  }

  public async listRows(id: string, request: ListImportRowRequest): Promise<Response> {
    return await this.client.get({url: `api/v1/data-transfer/imports/${id}/rows`, query: request});
  }

  // Import activity and tag ids are keys from the file, so they are escaped.
  public async updateActivityMapping(
      id: string,
      importActivityId: string,
      request: UpdateImportActivityMappingRequest): Promise<Response> {
    const key = encodeURIComponent(importActivityId);
    return await this.client.put({url: `api/v1/data-transfer/imports/${id}/activities/${key}`, body: request});
  }

  public async updateTagMapping(
      id: string,
      importTagId: string,
      request: UpdateImportTagMappingRequest): Promise<Response> {
    const key = encodeURIComponent(importTagId);
    return await this.client.put({url: `api/v1/data-transfer/imports/${id}/tags/${key}`, body: request});
  }

  public async apply(id: string, request: ApplyImportRequest): Promise<Response> {
    return await this.client.post({url: `api/v1/data-transfer/imports/${id}/apply`, body: request});
  }

  public async remove(id: string): Promise<Response> {
    return await this.client.delete({url: `api/v1/data-transfer/imports/${id}`});
  }
}
