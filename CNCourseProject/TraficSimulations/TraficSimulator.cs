using ComputerNetworksCourseWork.Analyzes.Algorithms;
using ComputerNetworksCourseWork.Networks;
using ComputerNetworksCourseWork.Networks.Nods;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ComputerNetworksCourseWork.TraficSimulations.Simulators;

public class TraficSimulator : INotifyPropertyChanged
{
    private Node source;

    private Node destination;

    private Protocol protocol = Protocol.TCP;

    private uint servicePacketLenght = 20;

    private uint infoPacketLenght = 80;

    private uint infoPacketCount = 100;

    private uint messageCount = 10;

    private ObservableCollection<Simulation> simulations = [];

    public Node Source
    {
        get => source;
        set
        {
            source = value;
            RaisePropertyChanged("Source");
        }
    }

    public Node Destination
    {
        get => destination;
        set
        {
            destination = value;
            RaisePropertyChanged("Destination");
        }
    }

    public Protocol Protocol
    {
        get => protocol;
        set
        {
            protocol = value;
            RaisePropertyChanged("Protocol");
        }
    }

    public uint ServicePacketLenght
    {
        get => servicePacketLenght;
        set
        {
            servicePacketLenght = value;
            RaisePropertyChanged("ServicePacketLenght");
        }
    }

    public uint InfoPacketLenght
    {
        get => infoPacketLenght;
        set
        {
            infoPacketLenght = value;
            RaisePropertyChanged("InfoPacketLenght");
        }
    }

    public uint InfoPacketCount
    {
        get => infoPacketCount;
        set
        {
            infoPacketCount = value;
            RaisePropertyChanged("InfoPacketCount");
        }
    }

    public uint MessageCount
    {
        get => messageCount;
        set
        {
            messageCount = value;
            RaisePropertyChanged("MessageCount");
        }
    }

    public ObservableCollection<Simulation> Simulations
    {
        get => simulations;
        set
        {
            simulations = value;
            RaisePropertyChanged("Simulations");
        }
    }

    public Network Network { get; set; }

    public TraficSimulator(Network network)
    {
        Network = network;

        source = Network.Nodes.First();
        destination = Network.Nodes.Last();
    }

    public bool GenerateTrafic()
    {
        if (Source is null)
        {
            return false;
        }

        if (Destination is null)
        {
            return false;
        }

        Simulations.Clear();

        var simulationAlgorithm = new TraficSimulationAlgorithm();

        for (int i = 0; i < MessageCount; i++)
        {
            var simulation = simulationAlgorithm.Execute(
                Source,
                Destination,
                Protocol,
                (int) ServicePacketLenght,
                (int) InfoPacketLenght,
                (int) InfoPacketCount);

            if (simulation is not null)
            {
                Simulations.Add(simulation);
            }
        }

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
