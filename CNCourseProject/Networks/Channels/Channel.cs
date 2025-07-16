using ComputerNetworksCourseWork.Networks.Nods;

namespace ComputerNetworksCourseWork.Networks.Channels;

public class Channel
{
    public Channel(int weight, double chanceOfError, ChanelType type, Node node1, Node node2)
    {
        Weight = weight;
        ChanceOfError = chanceOfError;
        Type = type;
        Node1 = node1;
        Node2 = node2;
    }

    public Node Node1 { get; set; }

    public Node Node2 { get; set; }

    public int Flow { get; set; } = 0;

    public int Weight { get; set; }

    public double ChanceOfError { get; set; }

    public ChanelType Type { get; set; }

    public Node OppositeTo(Node node)
    {
        return Node1 == node ? Node2 : Node1;
    }
}
