using PixChest.Database.Tables;

namespace PixChest.Utils.Notifications;
public static class FileNotifications {
	public static Subject<MediaFile> FileRegistered {
		get;
	} = new();
}
