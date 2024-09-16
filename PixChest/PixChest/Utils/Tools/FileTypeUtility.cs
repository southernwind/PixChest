using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using PixChest.Database.Tables;
using PixChest.FileTypes.Base;
using PixChest.FileTypes.Base.Models.Interfaces;
using PixChest.FileTypes.Base.ViewModels.Interfaces;
using PixChest.FileTypes.Base.Views;
using PixChest.ViewModels.Files;

namespace PixChest.Utils.Tools;
public static class FileTypeUtility {
	static FileTypeUtility() {
		_fileTypes = Ioc.Default.GetServices<IFileType>().ToArray();
	}
	private static readonly IFileType[] _fileTypes;
	public static IFileModel CreateFileModelFromRecord(MediaFile mediaFile) {
		return GetFileType(mediaFile).CreateFileModelFromRecord(mediaFile);
	}

	public static IThumbnailPickerViewModel CreateThumbnailPickerViewModel(FileViewModel fileViewModel) {
		return GetFileType(fileViewModel).CreateThumbnailPickerViewModel();
	}

	public static IThumbnailPickerView CreateThumbnailPickerView(FileViewModel fileViewModel) {
		return GetFileType(fileViewModel).CreateThumbnailPickerView();
	}

	private static IFileType GetFileType(MediaFile mediaFile) {
		return _fileTypes.First(x => x.MediaType == mediaFile.FilePath.GetMediaType());
	}
	private static IFileType GetFileType(FileViewModel fileViewModel) {
		return _fileTypes.First(x => x.MediaType == fileViewModel.MediaType);
	}
}
