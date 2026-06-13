using System.IO;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;

namespace MediaDeck.Views.Resources.Converters;

/// <summary>
/// バイナリ形式の画像データを <see cref="BitmapImage"/> に変換します。
/// </summary>
public class BinaryToImageSourceConverter : IValueConverter {
	/// <summary>
	/// バイナリ形式の画像データから <see cref="BitmapImage"/> を生成します。
	/// </summary>
	public object? Convert(object value, Type targetType, object parameter, string language) {
		if (value is not byte[] binary) {
			return null;
		}

		var image = new BitmapImage();
		using var stream = new MemoryStream(binary);
		using var randomAccessStream = stream.AsRandomAccessStream();
		image.SetSource(randomAccessStream);
		return image;
	}

	/// <summary>
	/// 逆変換はサポートしていません。
	/// </summary>
	public object ConvertBack(object value, Type targetType, object parameter, string language) {
		throw new NotImplementedException();
	}
}
