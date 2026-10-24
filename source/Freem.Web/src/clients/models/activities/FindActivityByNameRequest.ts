export class FindActivityByNameRequest {
  public readonly searchText: string;

  constructor(searchText: string) {
    this.searchText = searchText;
  }
}