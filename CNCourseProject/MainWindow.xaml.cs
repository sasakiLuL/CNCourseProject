using ComputerNetworksCourseWork.Analyzes;
using ComputerNetworksCourseWork.Graphs;
using ComputerNetworksCourseWork.Graphs.Controls;
using ComputerNetworksCourseWork.Networks;
using ComputerNetworksCourseWork.TraficSimulations.Simulators;
using System.Windows;
using System.Windows.Input;

namespace ComputerNetworksCourseWork;

public partial class MainWindow : Window
{
    public GraphEditor GraphEditor { get; set; }

    public Network Network { get; set; }

    public TraficSimulator TraficSimulator { get; set; }

    public Analysis Analysis { get; set; }

    public MainWindow()
    {
        Network = new Network();

        Network.Generate();

        InitializeComponent();

        GraphEditor = new GraphEditor()
        {
            GViewer = GraphControl.GViewer,
            Network = Network
        };

        GraphEditor.BuildGraph();
        GraphEditor.Update();

        TraficSimulator = new TraficSimulator(Network);

        Analysis = new Analysis(Network, GraphEditor);

        Network.GraphEditor = GraphEditor;

        NetworkControl.Network = Network;

        TraficSimulatorControl.TraficSimulator = TraficSimulator;

        AnalysisControl.Analysis = Analysis;
    }

    public void OnKeyDownHandler(object sender, KeyEventArgs e)
    {
        GraphControl.OnKeyDownHandler(sender, e);
    }

    public void OnKeyUpHandler(object sender, KeyEventArgs e)
    {
        GraphControl.OnKeyUpHandler(sender, e);
    }
}
