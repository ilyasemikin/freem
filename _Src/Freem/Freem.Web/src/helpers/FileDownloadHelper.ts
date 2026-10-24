export class FileDownloadHelper {
  // File name from a Content-Disposition header: the RFC 5987 "filename*" when present, otherwise "filename".
  public static getFileName(contentDisposition: string | null): string | undefined {
    if (contentDisposition === null) {
      return undefined;
    }

    const extended = /filename\*\s*=\s*([^']*)''([^;]+)/i.exec(contentDisposition);
    if (extended !== null) {
      return decodeURIComponent(extended[2].trim());
    }

    const plain = /filename\s*=\s*("([^"]*)"|[^;]+)/i.exec(contentDisposition);
    if (plain !== null) {
      return (plain[2] ?? plain[1]).trim();
    }

    return undefined;
  }

  // Hands the content to the browser as a downloaded file.
  public static save(blob: Blob, fileName: string) {
    const url = URL.createObjectURL(blob);
    const link = document.createElement("a");
    link.href = url;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    link.remove();
    URL.revokeObjectURL(url);
  }
}
