namespace PixChest.ViewModels.Files; 
public class FileViewModel {
	public ReactiveProperty<string> FilePath {
		get;
	} = new();
}
