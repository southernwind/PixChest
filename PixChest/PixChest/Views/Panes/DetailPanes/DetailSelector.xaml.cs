using PixChest.Composition.Bases;
using PixChest.ViewModels.Panes.DetailPanes;

namespace PixChest.Views.Panes.DetailPanes;

public sealed partial class DetailSelector : DetailSelectorUserControl {
    public DetailSelector()
    {
        this.InitializeComponent();
    }
}

public abstract class DetailSelectorUserControl : UserControlBase<DetailSelectorViewModel>;
