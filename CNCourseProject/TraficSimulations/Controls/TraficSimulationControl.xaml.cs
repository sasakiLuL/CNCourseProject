using ComputerNetworksCourseWork.TraficSimulations.Simulators;
using System.Windows;
using System.Windows.Controls;

namespace ComputerNetworksCourseWork.TraficSimulations.Controls;

public partial class TraficSimulationControl : UserControl
{
    public TraficSimulationControl()
    {
        InitializeComponent();
        DataContext = this;
    }

    public required TraficSimulator TraficSimulator { get; set; }

    public string[] ProtocolItems { get; } = ["TCP", "UDP"];

    public void OnGenerateTraficClickedHandler(object sender, EventArgs e)
    {
        if (!TraficSimulator.GenerateTrafic())
        {
            MessageBox.Show("The error has been appeared. Check all of the fields");
        }
    }
}
