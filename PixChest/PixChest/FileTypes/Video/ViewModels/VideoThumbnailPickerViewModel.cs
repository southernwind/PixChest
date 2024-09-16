using System.Threading.Tasks;

using PixChest.FileTypes.Base.ViewModels;
using PixChest.FileTypes.Base.ViewModels.Interfaces;
using PixChest.FileTypes.Video.Models;
using PixChest.Models.FileDetailManagers;

namespace PixChest.FileTypes.Video.ViewModels;

[AddTransient]
public class VideoThumbnailPickerViewModel : BaseThumbnailPickerViewModel {
	public VideoThumbnailPickerViewModel(
		ThumbnailsManager thumbnailsManager,
		VideoFileOperator imageFileOperator) : base(thumbnailsManager) {
		this._pdfFileOperator = imageFileOperator;
		this._updateTimeSubject.ObserveOnCurrentSynchronizationContext().Subscribe(x => this.Time.Value = x);
	}
	private readonly VideoFileOperator _pdfFileOperator;

	private readonly Subject<TimeSpan> _updateTimeSubject = new();
	public BindableReactiveProperty<TimeSpan> Time {
		get;
	} = new();

	public BindableReactiveProperty<string> VideoFilePath {
		get;
	} = new();

	public override void RecreateThumbnail() {
		if (this.targetFileViewModel is null) {
			return;
		}
		try {
			this.CandidateThumbnail.Value = this._pdfFileOperator.CreateThumbnail(this.targetFileViewModel.FileModel, 300, 300, this.Time.Value);
		} catch (Exception) {
			this.CandidateThumbnail.Value = null;
		}
	}

	public override async Task LoadAsync(IFileViewModel fileViewModel) {
		await base.LoadAsync(fileViewModel);
		this.VideoFilePath.Value = fileViewModel.FilePath;
	}

	public void UpdateTime(TimeSpan time) {
		this._updateTimeSubject.OnNext(time);
	}
}
