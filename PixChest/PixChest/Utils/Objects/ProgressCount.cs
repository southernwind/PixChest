using System.Reactive.Linq;

namespace PixChest.Utils.Objects; 
public class ProgressCount {

	public ReactivePropertySlim<long> Current {
		get;
	} = new(0);

	public ReactivePropertySlim<long?> Total {
		get;
	} = new(null);

	public IReadOnlyReactiveProperty<double> Progress {
		get;
	}

	public ReactivePropertySlim<bool> InProgress {
		get;
	} = new(false);

	public IReadOnlyReactiveProperty<bool> IsIndeterminate {
		get;
	}

	public ProgressCount() {
		this.Progress = this.Current.CombineLatest(this.Total, (current, total) => {
			if (total is not {} ltotal || ltotal == 0) {
				return 0;
			}
			return 100d * current / ltotal;
		}).ToReadOnlyReactivePropertySlim();

		this.IsIndeterminate = this.Total.Select(x => x == null).ToReadOnlyReactivePropertySlim();

		this.Progress.Where(x => x == 100).Subscribe(_ => {
			this.InProgress.Value = false;
		});
	}
}
