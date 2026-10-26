export class UserSettingsResponse {
    public readonly timeZoneOffset: string;
    public readonly weekStart: string;

    constructor(timeZoneOffset: string, weekStart: string) {
        this.timeZoneOffset = timeZoneOffset;
        this.weekStart = weekStart;
    }
}