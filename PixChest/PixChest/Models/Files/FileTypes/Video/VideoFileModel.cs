using PixChest.Models.Files.FileTypes.Base;

namespace PixChest.Models.Files.FileTypes.Video;
public class VideoFileModel(long id, string filePath) : FileModel(id, filePath) {
}
