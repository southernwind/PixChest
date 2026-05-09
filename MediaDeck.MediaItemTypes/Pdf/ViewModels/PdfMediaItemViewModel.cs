using MediaDeck.Composition.Enum;
using MediaDeck.Composition.Interfaces;
using MediaDeck.Composition.Interfaces.MediaItemTypes.Models;
using MediaDeck.MediaItemTypes.Base.ViewModels;
using MediaDeck.MediaItemTypes.Pdf.Models;

namespace MediaDeck.MediaItemTypes.Pdf.ViewModels;

[Inject(InjectServiceLifetime.Transient)]
public class PdfMediaItemViewModel(IStringProvider stringProvider) : BaseMediaItemViewModel(MediaType.Pdf) {
	private readonly IStringProvider _stringProvider = stringProvider;

	public int? FileCount {
		get;
		private set;
	}

	public string FileCountText {
		get {
			return this.FileCount is { } c ? this._stringProvider.GetString("Pdf_PagesCountFormat", c) : string.Empty;
		}
	}

	public override void Initialize(IMediaItemModel fileModel) {
		base.Initialize(fileModel);
		if (fileModel is PdfMediaItemModel pdfModel) {
			this.FileCount = pdfModel.FileCount;
		}
	}
}