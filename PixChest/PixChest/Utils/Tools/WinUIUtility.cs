using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace PixChest.Utils.Tools; 
public static class WinUIUtility {
	public static Window GetParentWindow(this FrameworkElement element) {
		var parent = element.XamlRoot.Content;
		
		while (parent is FrameworkElement fe) {
			parent = fe.XamlRoot.Content;
		}

		return ((object)parent as Window)!;
	}

}