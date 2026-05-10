using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using MediaDeck.MediaItemTypes.Pdf.Models;
using Windows.Data.Pdf;
using Windows.Storage;
using Windows.Storage.Streams;

namespace MediaDeck.MediaItemTypes.UI.Pdf;

[Inject(InjectServiceLifetime.Transient, typeof(IPdfDocumentOperator))]
public class PdfDocumentOperator : IPdfDocumentOperator {
	/// <summary>
	/// サムネイル作成
	/// </summary>
	/// <param name="filePath">ファイルパス</param>
	/// <param name="width">サムネイル幅</param>
	/// <param name="height">サムネイル高さ</param>
	/// <param name="pageNumber">サムネイルにするページ番号</param>
	/// <returns>作成されたサムネイルのバイト配列</returns>
	public async Task<byte[]> CreateThumbnailAsync(string filePath, int width, int height, int pageNumber = 1) {
		var file = await StorageFile.GetFileFromPathAsync(filePath);
		var pdfDoc = await PdfDocument.LoadFromFileAsync(file);

		if (pdfDoc.PageCount < pageNumber || pageNumber < 1) {
			throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number is out of range.");
		}

		using var page = pdfDoc.GetPage((uint)pageNumber - 1);
		using var stream = new InMemoryRandomAccessStream();

		var options = new PdfPageRenderOptions {
			DestinationWidth = (uint)width,
			DestinationHeight = (uint)height
		};

		await page.RenderToStreamAsync(stream, options);

		var buffer = new byte[stream.Size];
		await stream.ReadAsync(buffer.AsBuffer(), (uint)stream.Size, InputStreamOptions.None);
		return buffer;
	}

	/// <summary>
	/// PDFのプロパティ取得
	/// </summary>
	/// <param name="filePath">ファイルパス</param>
	/// <returns>PDFプロパティ</returns>
	public async Task<PdfProperties> GetPdfPropertiesAsync(string filePath) {
		var file = await StorageFile.GetFileFromPathAsync(filePath);
		var pdfDoc = await PdfDocument.LoadFromFileAsync(file);

		var properties = new PdfProperties {
			PageCount = (int)pdfDoc.PageCount
		};

		if (pdfDoc.PageCount > 0) {
			using var page = pdfDoc.GetPage(0);
			properties.Width = page.Size.Width;
			properties.Height = page.Size.Height;
		}

		return properties;
	}
}