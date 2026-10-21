export class UserSettingsResponse {
    public readonly timeZoneOffset: string;

    constructor(timeZoneOffset: string) {
        this.timeZoneOffset = timeZoneOffset;
    }
}