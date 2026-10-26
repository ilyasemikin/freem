import { UpdateField } from "../../UpdateField";

export class UpdateUserSettingsRequest {
	public readonly timeZoneOffset?: UpdateField<string>;
	public readonly weekStart?: UpdateField<string>;
}