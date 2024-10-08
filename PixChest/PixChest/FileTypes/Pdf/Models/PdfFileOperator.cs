using PixChest.Database.Tables;
using System.IO;
using PixChest.Utils.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Enums;
using System.Drawing.Imaging;
using PixChest.FileTypes.Base.Models;

namespace PixChest.FileTypes.Pdf.Models;
[AddTransient]
public partial class PdfFileOperator : BaseFileOperator {

	public override MediaType TargetMediaType {
		get;
	} = MediaType.Pdf;

	public override void RegisterFile(string filePath) {
		using var transaction = this._db.Database.BeginTransaction();
		var isExists = this._db.MediaFiles.Any(x => x.FilePath == filePath);
		if (isExists) {
			return;
		}

		var thumbPath = FilePathUtility.GetThumbnailRelativeFilePath(filePath);
		try {
			var image = this.CreateThumbnail(filePath, 300, 300, 1);
			new FileInfo(thumbPath).Directory?.Create();
			File.WriteAllBytes(thumbPath, image);
		} catch (Exception) {
			thumbPath = null;
		}

		var pdfDocument = PdfDocument.Load(filePath);
		var fileInfo = new FileInfo(filePath);

		var mf = new MediaFile {
			DirectoryPath = Path.GetDirectoryName(filePath)!,
			FilePath = filePath,
			ThumbnailFileName = thumbPath,
			Rate = -1,
			Description = "",
			IsAutoGeneratedThumbnail = true,
			FileSize = fileInfo.Length,
			CreationTime = fileInfo.CreationTime,
			ModifiedTime = fileInfo.LastWriteTime,
			LastAccessTime = fileInfo.LastAccessTime,
			Width = (int)pdfDocument.Pages[0].Width,
			Height = (int)pdfDocument.Pages[1].Height,
			Container = new() {
				PageCount = pdfDocument.Pages.Count
			}
		};

		this._db.MediaFiles.Add(mf);
		this._db.SaveChanges();
		transaction.Commit();
	}

	/// <summary>
	/// サムネイル作成
	/// </summary>
	/// <param name="filePath">動画ファイルパス</param>
	/// <param name="width">サムネイル幅</param>
	/// <param name="height">サムネイル高さ</param>
	/// <param name="pageNumber">サムネイルにするページ番号</param>
	/// <returns>作成されたサムネイルファイル名</returns>
	public byte[] CreateThumbnail(string filePath, int width, int height, int pageNumber = 1) {

		var pdfDoc = PdfDocument.Load(filePath);
		var page = pdfDoc.Pages[pageNumber - 1];
		using var pdfBitmap = new PdfBitmap(width, height, true);
		page.Render(pdfBitmap, 0, 0, width, height, PageRotate.Normal, RenderFlags.FPDF_NONE);
		using var ms = new MemoryStream();

		pdfBitmap.GetImage().Save(ms, ImageFormat.Jpeg);
		return ms.ToArray();
	}
}
