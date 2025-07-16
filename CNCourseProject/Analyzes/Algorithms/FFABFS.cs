using ComputerNetworksCourseWork.Networks.Channels;
using ComputerNetworksCourseWork.Networks.Nods;

namespace ComputerNetworksCourseWork.Analyzes.Algorithms;

public class FFABFS
{
    public int Iteration(Node source, Node sink, Dictionary<Node, Channel> path)
    {
        int maxFlow = 0;

        double pathFlow = double.PositiveInfinity;
        Node currentNode = sink;

        while (currentNode != source)
        {
            Channel channel = path[currentNode];
            pathFlow = Math.Min(pathFlow, channel.Weight - channel.Flow);
            currentNode = channel.OppositeTo(currentNode);
        }

        maxFlow += (int)pathFlow;
        currentNode = sink;

        while (currentNode != source)
        {
            Channel channel = path[currentNode];
            channel.Flow += (int)pathFlow;
            currentNode = channel.OppositeTo(currentNode);
        }

        return maxFlow;
    }
}
