using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using PixChest.Database;
using PixChest.FileTypes.Base.Models.Interfaces;
using PixChest.Models.Preferences;

namespace PixChest.Models.Files;

[AddSingleton]
public class FileRegistrar {
	private static readonly IFileOperator[] _fileOperators;
	public ObservableQueue<string> RegistrationQueue {
		get;
	} = [];

	public ReactiveProperty<long> QueueCount {
		get;
	} = new();

	public Config Config {
		get;
	}

	static FileRegistrar() {
		_fileOperators = Ioc.Default.GetServices<IFileOperator>().ToArray();
	}

	public FileRegistrar(Config config) {
		this.Config = config;
		this.RegistrationQueue
			.ObserveAdd()
			.Synchronize()
			.Subscribe(_ => {
				this.RegisterFiles();
			});

		this.RegistrationQueue.ObserveCountChanged(true).ThrottleLast(TimeSpan.FromSeconds(0.1)).Subscribe(x => {
			this.QueueCount.Value = x;
		});
	}

	public void RegisterFiles() {
		while (this.RegistrationQueue.TryDequeue(out var filePath)) {
			try {
				var type = filePath.GetMediaType();
				var fileOperator = _fileOperators.First(x => x.TargetMediaType == type);
				fileOperator.RegisterFile(filePath);
			} catch (Exception e) {
				Console.WriteLine(e);
			}
		}
	}
}
