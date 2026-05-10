using System.Threading.Tasks;
using MediaDeck.Composition.Stores.Config.Model;
using MediaDeck.MediaItemTypes.Base.Models;
using MediaDeck.MediaItemTypes.Base.ViewModels;
using MediaDeck.MediaItemTypes.Pdf.Models;
using Microsoft.Extensions.Logging;

namespace MediaDeck.MediaItemTypes.Pdf.ViewModels;

[Inject(InjectServiceLifetime.Transient)]
public class PdfThumbnailPickerViewModel(BaseThumbnailPickerModel thumbnailPickerModel, IPdfDocumentOperator pdfDocumentOperator, ILogger<PdfThumbnailPickerViewModel> logger, IFilePickerService filePickerService, ConfigModel config) : BaseThumbnailPickerViewModel<BaseThumbnailPickerModel>(thumbnailPickerModel, filePickerService, config) {
	private readonly IPdfDocumentOperator _pdfDocumentOperator = pdfDocumentOperator;
	private readonly ILogger<PdfThumbnailPickerViewModel> _logger = logger;

	public BindableReactiveProperty<int> PageNumber {
		get;
	} = new(1);

	public override async Task RecreateThumbnailAsync() {
		if (this.targetFileViewModel is null) {
			return;
		}
		try {
			this.CandidateThumbnail.Value = await this._pdfDocumentOperator.CreateThumbnailAsync(this.targetFileViewModel.FilePath, this._config.ThumbnailConfig.ThumbnailSize.Value, this._config.ThumbnailConfig.ThumbnailSize.Value, this.PageNumber.Value);
		} catch (Exception ex) {
			this._logger.LogError(ex, "Failed to recreate pdf thumbnail for file {FilePath} at page {PageNumber}", this.targetFileViewModel.FilePath, this.PageNumber.Value);
			this.CandidateThumbnail.Value = null;
		}
	}
}