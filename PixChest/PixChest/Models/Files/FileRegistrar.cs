using System.ComponentModel;
using System.IO;
using System.Reflection;

using ObservableCollections;

using PixChest.Database;
using PixChest.Models.Files.FileTypes.Interfaces;
using PixChest.Models.Preferences;
using PixChest.Utils.Enums;

namespace PixChest.Models.Files;

[AddSingleton]
public class FileRegistrar {
	private readonly PixChestDbContext _db;
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
		_fileOperators =
			Assembly
				.GetExecutingAssembly()
				.GetTypes()
				.Where(x =>
					x.GetInterfaces()
					.Any(t => t == typeof(IFileOperator)))
				.Where(x => x.IsAbstract == false)
				.Select(x => Activator.CreateInstance(x) as IFileOperator)
				.ToArray()!;
	}

	public FileRegistrar(PixChestDbContext dbContext, Config config) {
		this._db = dbContext;
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
		while (this.RegistrationQueue.TryDequeue(out var filepath)) {
			try {
				var type = filepath.GetMediaType();
				var fileOperator = _fileOperators.First(x => x.TargetMediaType == type);
				fileOperator.RegisterFile(filepath);
			} catch (Exception e) {
				Console.WriteLine(e);
				throw;
			}
		}
	}
}
