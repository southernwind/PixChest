using System.Threading.Tasks;

using PixChest.FileTypes.ViewModels.Interfaces;
using PixChest.Models.FileDetailManagers;
using PixChest.ViewModels.Files;

namespace PixChest.FileTypes.ViewModels.Base;

public abstract class BaseThumbnailPickerViewModel : IThumbnailPickerViewModel {
	public BaseThumbnailPickerViewModel(ThumbnailsManager thumbnailsManager) {
		this.RecreateThumbnailCommand.Subscribe(_ => this.RecreateThumbnail());
		this.SaveCommand.Subscribe(async _ => await this.SaveAsync());
		this.thumbnailsManager = thumbnailsManager;
	}

	protected FileViewModel? targetFileViewModel;
	protected ThumbnailsManager thumbnailsManager;

	public BindableReactiveProperty<byte[]?> OriginalThumbnail {
		get;
	} = new();

	public BindableReactiveProperty<byte[]?> CandidateThumbnail {
		get;
	} = new();

	public ReactiveCommand<Unit> RecreateThumbnailCommand {
		get;
	} = new();

	public ReactiveCommand<Unit> SaveCommand {
		get;
	} = new();

	public abstract void RecreateThumbnail();

	public virtual async Task SaveAsync() {
		if (this.targetFileViewModel is null) {
			return;
		}
		if (this.CandidateThumbnail.Value is null) {
			return;
		}
		await this.thumbnailsManager.UpdateThumbnail(this.targetFileViewModel.FileModel, this.CandidateThumbnail.Value);
	}

	public virtual async Task LoadAsync(FileViewModel fileViewModel) {
		this.targetFileViewModel = fileViewModel;
		this.CandidateThumbnail.Value = null;
		this.OriginalThumbnail.Value = await this.thumbnailsManager.LoadThumbnailAsync(fileViewModel.FileModel);
	}
}
