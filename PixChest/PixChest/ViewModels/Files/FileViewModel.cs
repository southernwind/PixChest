using System.Diagnostics;

using PixChest.Models.FileEditors;
using PixChest.Models.Files;
using PixChest.Utils.Objects;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace PixChest.ViewModels.Files;

[AddTransient]
public class FileViewModel {
	public FileViewModel(FileModel fileModel, TagsManager tagsManager) {
		this.FilePath = fileModel.FilePath;
		this.ThumbnailFilePath = fileModel.ThumbnailFilePath;
		this.Properties = fileModel.Properties;
		this.Tags.AddRange(fileModel.Tags);
		this.Tags.ObserveAddChanged().Subscribe(async x => {
			await tagsManager.AddTag(fileModel, x);
		});
		this.Tags.ObserveRemoveChanged().Subscribe(async x => {
			await tagsManager.RemoveTag(fileModel, x);
		});
	}
	public string FilePath {
		get;
	}

	public string? ThumbnailFilePath {
		get;
	}

	/// <summary>
	/// プロパティ
	/// </summary>
	public Attributes<string> Properties {
		get;
	}

	public ReactiveCollection<string> Tags {
		get;
	} = [];
}
