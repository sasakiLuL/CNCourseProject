using ComputerNetworksCourseWork.Analyzes.Algorithms;
using ComputerNetworksCourseWork.Graphs;
using ComputerNetworksCourseWork.Networks;
using ComputerNetworksCourseWork.Networks.Channels;
using Microsoft.Msagl.Drawing;
using System.ComponentModel;
using Node = ComputerNetworksCourseWork.Networks.Nods.Node;

namespace ComputerNetworksCourseWork.Analyzes;

public class Analysis : INotifyPropertyChanged
{
    private Node source;

    private Node sink;

    private int maxFlow = 0;

    private int step = 0;

    private bool isFinished = false;

    private Dictionary<Node, Channel> parent = [];

    private List<Node> path = [];

    public Network Network { get; set; }

    public GraphEditor GraphEditor { get; set; }

    public Node Source 
    { 
        get => source; set 
        {
            source = value;
            RaisePropertyChanged("Source");
        }
    }

    public Node Sink
    {
        get => sink; set
        {
            sink = value;
            RaisePropertyChanged("Sink");
        }
    }

    public int MaxFlow
    {
        get => maxFlow; set
        {
            maxFlow = value;
            RaisePropertyChanged("MaxFlow");
        }
    }

    public int Step
    {
        get => step; set
        {
            step = value;
            RaisePropertyChanged("Step");
        }
    }

    public bool IsFinished
    {
        get => isFinished; set
        {
            isFinished = value;
            RaisePropertyChanged("IsFinished");
        }
    }

    public Dictionary<Node, Channel> Parent
    {
        get => parent; set
        {
            parent = value;
            RaisePropertyChanged("Parent");
        }
    }

    public List<Node> Path
    {
        get => path; set
        {
            path = value;
            RaisePropertyChanged("Path");
        }
    }

    public FFABFS FFA { get; set; }

    public BFS BFS { get; set; }

    public Analysis(Network network, GraphEditor graphEditor)
    {
        FFA = new FFABFS();
        BFS = new BFS();

        Network = network;
        GraphEditor = graphEditor;

        source = Network.Nodes.First();
        sink = Network.Nodes.Last();
    }

    public bool Iterate()
    {
        if (Source is null || Sink is null)
        {
            return false;
        }

        if (!BFS.FindParent(Source, Sink,
            out Dictionary<Node, Channel> parent))
        {
            IsFinished = true;
            return true;
        }

        GraphEditor.ClearHightlight(Source, Sink, Parent);
        GraphEditor.Update();

        Parent = parent;

        MaxFlow += FFA.Iteration(Source, Sink, Parent);

        GraphEditor.AddHightlight(Source, Sink, Parent, Color.PaleGreen, Color.DarkGreen);
        GraphEditor.Update();

        Step++;

        return true;
    }

    public bool ResetFFA()
    {
        if (Source is null || Sink is null)
        {
            return false;
        }

        MaxFlow = 0;
        Step = 0;
        IsFinished = false;
        Network.ResetFlow();

        GraphEditor.ClearHightlight(Source, Sink, Parent);
        GraphEditor.Update();

        Parent.Clear();

        return true;
    }

    public bool ResetOptimalPath()
    {
        if (Source is null || Sink is null)
        {
            return false;
        }

        GraphEditor.ClearHightlight(Source, Sink, Path);
        GraphEditor.Update();

        Path.Clear();

        return true;
    }

    public bool FindOptimalPath()
    {
        if (Source is null || Sink is null)
        {
            return false;
        }

        GraphEditor.ClearHightlight(Source, Sink, Path);
        GraphEditor.Update();

        if(!BFS.FindOptimalPath(Source, Sink, out var path))
        {
            return false;
        }

        Path = path;

        GraphEditor.AddHightlight(Source, Sink, Path, Color.PaleVioletRed, Color.DarkViolet);
        GraphEditor.Update();

        return true;
    }

    public event PropertyChangedEventHandler? PropertyChanged = delegate { };

    private void RaisePropertyChanged(string propertyName)
    {
        var handlers = PropertyChanged;

        if (handlers is not null)
        {
            handlers(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
