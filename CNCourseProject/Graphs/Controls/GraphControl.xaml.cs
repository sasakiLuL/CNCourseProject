using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Msagl.GraphViewerGdi;
using System.Drawing;

namespace ComputerNetworksCourseWork.Graphs.Controls;

public partial class GraphControl : UserControl
{
    public required GraphEditor GraphEditor { get; set; }

    public GraphControl()
    {
        InitializeComponent();
        DataContext = this;
        GViewer.OutsideAreaBrush = new SolidBrush(System.Drawing.Color.White);
    }

    public void OnKeyDownHandler(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Space)
        {
            GViewer.PanButtonPressed = true;
        }
    }

    public void OnKeyUpHandler(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Space)
        {
            GViewer.PanButtonPressed = false;
        }
    }
}
