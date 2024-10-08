namespace PixChest.Database.Tables; 
public class PositionNameDetail {
	private string? _desc;
	private string? _name;
	private Position? _position;

	/// <summary>
	/// 緯度
	/// </summary>
	public double Latitude {
		get;
		set;
	}

	/// <summary>
	/// 経度
	/// </summary>
	public double Longitude {
		get;
		set;
	}

	/// <summary>
	/// 名前の種類
	/// </summary>
	public string Desc {
		get {
			return this._desc ?? throw new InvalidOperationException();
		}
		set {
			this._desc = value;
		}
	}

	/// <summary>
	/// 名前
	/// </summary>
	public string Name {
		get {
			return this._name ?? throw new InvalidOperationException();
		}
		set {
			this._name = value;
		}
	}

	/// <summary>
	/// 位置情報
	/// </summary>
	public Position Position {
		get {
			return this._position ?? throw new InvalidOperationException();
		}
		set {
			this._position = value;
		}
	}
}
