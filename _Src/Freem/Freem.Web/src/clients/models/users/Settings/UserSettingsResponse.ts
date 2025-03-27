export class UserSettingsResponse {
    public readonly dayUtcOffset: string;

    constructor(dayUtcOffset: string) {
        this.dayUtcOffset = dayUtcOffset;
    }
}