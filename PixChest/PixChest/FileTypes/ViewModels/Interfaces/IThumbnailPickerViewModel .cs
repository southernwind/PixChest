using System.Threading.Tasks;

using PixChest.ViewModels.Files;

namespace PixChest.FileTypes.ViewModels.Interfaces;

public interface IThumbnailPickerViewModel {
	public BindableReactiveProperty<byte[]?> OriginalThumbnail {
		get;
	}

	public BindableReactiveProperty<byte[]?> CandidateThumbnail {
		get;
	}

	public ReactiveCommand<Unit> RecreateThumbnailCommand {
		get;
	}

	public ReactiveCommand<Unit> SaveCommand {
		get;
	}

	public void RecreateThumbnail();

	public Task SaveAsync();

	public Task LoadAsync(FileViewModel fileViewModel);
}
