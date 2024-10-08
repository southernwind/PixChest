namespace PixChest.Utils.Objects;
/// <summary>
/// 場所、座標クラス
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="latitude">緯度</param>
/// <param name="longitude">経度</param>
/// <param name="altitude">高度</param>
public class GpsLocation(double latitude, double longitude, double? altitude = null) : IComparable<GpsLocation>, IComparable {
	/// <summary>
	/// 緯度
	/// </summary>
	public double Latitude {
		get;
	} = latitude;

	/// <summary>
	/// 経度
	/// </summary>
	public double Longitude {
		get;
	} = longitude;

	/// <summary>
	/// 高度
	/// </summary>
	public double? Altitude {
		get;
	} = altitude;

	public int CompareTo(GpsLocation? other) {
		if (other is null) {
			return -1;
		}
		var c = this.Latitude.CompareTo(other.Latitude);
		if (c != 0) {
			return c;
		}
		c = this.Longitude.CompareTo(other.Longitude);
		if (c != 0) {
			return c;
		}
		if (this.Altitude is { } alt) {
			c = alt.CompareTo(other.Altitude);
			if (c != 0) {
				return c;
			}
		} else if (other.Altitude is { } alt2) {
			c = alt2.CompareTo(this.Altitude);
			if (c != 0) {
				return c;
			}
		}

		return 0;
	}

	public int CompareTo(object? obj) {
		if (obj is GpsLocation gl) {
			return this.CompareTo(gl);
		} else {
			return -1;
		}
	}

	public static bool operator ==(GpsLocation? gl, GpsLocation? gl2) {
		if (gl is null && gl2 is null) {
			return true;
		}
		if (gl is null || gl2 is null) {
			return false;
		}
		return gl.CompareTo(gl2) == 0;
	}

	public static bool operator !=(GpsLocation? gl, GpsLocation? gl2) {
		return !(gl == gl2);
	}

	public static bool operator <(GpsLocation? gl, GpsLocation? gl2) {
		if (gl == gl2) {
			return false;
		}
		if (gl == null) {
			return gl2!.CompareTo(gl) > 0;
		}
		return gl.CompareTo(gl2) < 0;
	}

	public static bool operator >(GpsLocation? gl, GpsLocation? gl2) {
		if (gl == gl2) {
			return false;
		}
		if (gl == null) {
			return gl2!.CompareTo(gl) < 0;
		}
		return gl.CompareTo(gl2) > 0;
	}

	public static bool operator <=(GpsLocation? gl, GpsLocation? gl2) {
		if (gl == gl2) {
			return true;
		}
		return gl < gl2;
	}

	public static bool operator >=(GpsLocation? gl, GpsLocation? gl2) {
		if (gl == gl2) {
			return true;
		}
		return gl > gl2;
	}

	public override string ToString() {
		return $"{this.Latitude} {this.Longitude} {this.Altitude}";
	}

	public override bool Equals(object? obj) {
		if (obj is not GpsLocation loc) {
			return false;
		}
		return this.Latitude == loc.Latitude &&
			this.Longitude == loc.Longitude &&
			this.Altitude == loc.Altitude;
	}

	public override int GetHashCode() {
		return this.Latitude.GetHashCode() ^ this.Longitude.GetHashCode() ^ this.Altitude.GetHashCode();
	}
}
