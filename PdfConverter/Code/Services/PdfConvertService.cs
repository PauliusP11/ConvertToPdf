using NLog;
using Xceed.Words.NET;

namespace PdfConverter;

public class PdfConvertService {
    private readonly Logger _log = LogManager.GetLogger("PdfConvert.PdfConvertService");

    private static readonly object Locker = new();

    public (bool, byte[]) GetPdfBytes(byte[] docxBytes) {
        var resultStream = new MemoryStream();
        var docxStream = new MemoryStream(docxBytes);

        using var document = DocX.Load(docxStream);
        DocX.ConvertToPdf(document, resultStream);

        var result = resultStream.ToArray();

        return (true, result);
    }
}