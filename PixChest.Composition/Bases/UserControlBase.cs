using Microsoft.UI.Xaml.Controls;

namespace PixChest.Composition.Bases;
public abstract class UserControlBase<T>:UserControl where T:class {
	public T? ViewModel {
		get; set;
	}

	protected UserControlBase() {
		this.DataContextChanged += (s, e) => {
			this.ViewModel = this.DataContext as T;
		};
	}
}
