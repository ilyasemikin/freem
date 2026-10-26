export class ErrorTypes {
  public static readonly ImportUnmappedEntities = "freem:import:unmapped_entities";
  public static readonly ImportTooManyRows = "freem:import:too_many_rows";
  public static readonly ActivityNameAlreadyExists = "freem:activity:name_already_exists";
  public static readonly TagNameAlreadyExists = "freem:tag:name_already_exists";
}

export class ErrorProperties {
  public static readonly ImportActivityId = "freem:import_activity_id";
  public static readonly ImportTagId = "freem:import_tag_id";
  public static readonly RowsLimit = "freem:rows_limit";
}
