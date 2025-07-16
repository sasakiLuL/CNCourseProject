using System.Windows;
using System.Windows.Controls;

namespace ComputerNetworksCourseWork.Networks.Controls;

public partial class NetworkControl : UserControl
{
    public required Network Network { get; set; }

    public NetworkControl()
    {
        InitializeComponent();
        DataContext = this;
    }

    public void OnGenerateButtonClickedHandler(object sender, RoutedEventArgs e)
    {
        Network.Generate();
        Network.GraphEditor?.BuildGraph();
    }
}
