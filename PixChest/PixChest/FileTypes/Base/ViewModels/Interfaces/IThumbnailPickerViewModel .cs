using System.Threading.Tasks;

namespace PixChest.FileTypes.Base.ViewModels.Interfaces;

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

	public Task LoadAsync(IFileViewModel fileViewModel);
}
