using System.Linq;

namespace Counters.Views;

public partial class AllCountersPage : ContentPage
{
    public AllCountersPage()
    {
        InitializeComponent();
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        countersCollection.SelectedItem = null;
    }
}