const palette = [
  "#6366f1",
  "#10b981",
  "#f59e0b",
  "#ef4444",
  "#0ea5e9",
  "#8b5cf6",
  "#14b8a6",
  "#f97316",
  "#ec4899",
  "#84cc16"
];

export class ActivityColor {
  public static readonly palette: readonly string[] = palette;

  public static random(): string {
    const index = Math.floor(Math.random() * palette.length);
    return palette[index];
  }

  public static isFromPalette(color: string): boolean {
    return palette.includes(color.toLowerCase());
  }
}
