export class AuthSession {
  private readonly baseUrl?: string;
  private accessToken?: string;
  private refreshing?: Promise<boolean>;

  private readonly signedOutListeners = new Set<() => void>();

  // Returns the function that removes the listener.
  public onSignedOut(listener: () => void): () => void {
    this.signedOutListeners.add(listener);

    return () => {
      this.signedOutListeners.delete(listener);
    };
  }

  public notifySignedOut() {
    for (const listener of this.signedOutListeners) {
      listener();
    }
  }

  constructor(baseUrl?: string) {
    this.baseUrl = baseUrl;
  }

  public getAccessToken(): string | undefined {
    return this.accessToken;
  }

  public setAccessToken(accessToken: string) {
    this.accessToken = accessToken;
  }

  public clear() {
    this.accessToken = undefined;
  }

  // Rotates the session using the HTTP-only refresh cookie; concurrent calls share one request.
  public refresh(): Promise<boolean> {
    if (this.refreshing === undefined) {
      this.refreshing = this.refreshInner().finally(() => {
        this.refreshing = undefined;
      });
    }

    return this.refreshing;
  }

  private async refreshInner(): Promise<boolean> {
    const response = await fetch(`${this.baseUrl}/api/v1/auth/refresh`, {
      method: "POST",
      credentials: "include"
    });

    if (!response.ok) {
      this.clear();
      return false;
    }

    const session = await response.json();
    this.accessToken = session.accessToken;
    return true;
  }
}
