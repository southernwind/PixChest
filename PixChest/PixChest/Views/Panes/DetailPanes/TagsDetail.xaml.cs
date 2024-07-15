namespace PixChest.Views.Panes.DetailPanes;
public sealed partial class TagsDetail : DetailPaneBase {
	public TagsDetail(){
		this.InitializeComponent();
	}

	private void TokenizingTextBox_TokenItemAdding(CommunityToolkit.WinUI.Controls.TokenizingTextBox sender, CommunityToolkit.WinUI.Controls.TokenItemAddingEventArgs args) {
		if (sender.Items.Any(x => x as string == args.TokenText)) {
			args.Cancel = true;
		}
	}
}
