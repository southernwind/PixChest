using MediaDeck.Core.Models.Maps;
using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace MediaDeck.Views.Resources.Converters;

public class PinStateToBrushConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, string language) {
		if (value is not PinState state) {
			return new SolidColorBrush(ColorHelper.FromArgb(255, 224, 224, 224));
		}

		return state switch {
			PinState.Selected => new SolidColorBrush(Colors.DodgerBlue),
			PinState.Indeterminate => new SolidColorBrush(Colors.Orange),
			_ => new SolidColorBrush(ColorHelper.FromArgb(255, 224, 224, 224))
		};
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language) {
		throw new NotImplementedException();
	}
}