using ComputerNetworksCourseWork.Graphs;
using ComputerNetworksCourseWork.Networks.Channels;
using ComputerNetworksCourseWork.Networks.Nods;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ComputerNetworksCourseWork.Networks;

public class Network : INotifyPropertyChanged
{
    private HashSet<int> channelWeights = [2, 3, 5, 7, 10, 12, 15, 20, 21, 25];

    private HashSet<double> chancesOfError = [0.01, 0.02, 0.03];

    private uint nodeCount = 26;

    private uint averageChannelsPerNodeCount = 3;

    private uint satelliteChannelsCount = 3;

    private ObservableCollection<Node> nodes = [];

    private ObservableCollection<Channel> channels = [];

    public HashSet<int> ChannelWeights
    {
        get => channelWeights; 
        set
        {
            channelWeights = value;
            RaisePropertyChanged("ChannelWeights");
        }
    }

    public HashSet<double> ChancesOfError
    {
        get => chancesOfError; 
        set
        {
            chancesOfError = value;
            RaisePropertyChanged("ChancesOfError");
        }
    }

    public uint NodeCount
    {
        get => nodeCount; 
        set
        {
            nodeCount = value;
            RaisePropertyChanged("NodeCount");
        }
    }

    public uint AverageChannelsPerNodeCount
    {
        get => averageChannelsPerNodeCount; 
        set
        {
            averageChannelsPerNodeCount = value;
            RaisePropertyChanged("AverageChannelsPerNodeCount");
        }
    }

    public uint SatelliteChannelsCount
    {
        get => satelliteChannelsCount; 
        set
        {
            satelliteChannelsCount = value;
            RaisePropertyChanged("SatelliteChannelsCount");
        }
    }

    public ObservableCollection<Node> Nodes 
    { 
        get => nodes; 
        set
        {
            nodes = value;
            RaisePropertyChanged("Nodes");
        }
    }

    public ObservableCollection<Channel> Channels
    {
        get => channels;
        set
        {
            channels = value;
            RaisePropertyChanged("Channels");
        }
    }

    public GraphEditor? GraphEditor { get; set; } = null;

    public void Generate()
    {
        Random random = new Random();

        Channels.Clear();

        CreateNodes();

        foreach (var node in Nodes)
        {
            var potentialDestinationNodes = Nodes.Where(n => n != node).ToList();

            for (int i = 0; i < AverageChannelsPerNodeCount; i++)
            {
                potentialDestinationNodes = FindPotentialDestinationNodes(node, potentialDestinationNodes).ToList();

                if (!potentialDestinationNodes.Any())
                {
                    break;
                }

                var destinationNode = PickRandom(potentialDestinationNodes);

                potentialDestinationNodes.Remove(destinationNode);

                int weight = PickRandom(ChannelWeights);

                double error = PickRandom(ChancesOfError);

                ChanelType channelType = PickRandom([ChanelType.Duplex, ChanelType.HalfDuplex]);

                var channel = new Channel(
                    weight,
                    error,
                    channelType,
                    node,
                    destinationNode);

                Channels.Add(channel);

                node.AddConnection(channel);

                destinationNode.AddConnection(channel);
            }
        }

        for (int i = 0; i < SatelliteChannelsCount; i++)
        {
            var randomConnection = PickRandom(PickRandom(Nodes).Channels.ToList());

            randomConnection.Type = ChanelType.Satellite;
        }
    }

    public void ResetFlow()
    {
        foreach (var chanel in Channels)
        {
            chanel.Flow = 0;
        }
    }

    private void CreateNodes()
    {
        Nodes.Clear();

        for (int i = 1; i <= NodeCount; i++)
        {
            Nodes.Add(new(i, []));
        }
    }

    private static TType PickRandom<TType>(ICollection<TType> collection)
    {
        var random = new Random();

        return collection.ElementAt(random.Next(collection.Count()));
    }

    private static IEnumerable<Node> FindPotentialDestinationNodes(Node node, IEnumerable<Node> nodes)
    {
        return nodes
            .Where(n => !n.Channels.Any(c => c.Node1 == node || c.Node2 == node) && n.Channels.Count < 2);
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
