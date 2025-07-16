using ComputerNetworksCourseWork.Networks.Channels;
using ComputerNetworksCourseWork.Networks.Nods;
using ComputerNetworksCourseWork.TraficSimulations;

namespace ComputerNetworksCourseWork.Analyzes.Algorithms;

public class TraficSimulationAlgorithm
{
    protected Random _random = new Random();

    public Simulation? Execute(Node source, Node destination, Protocol protocol, int servicePacketLenght, int infoPacketLenght, int infoPacketsCount)
    {
        var bfs = new BFS();

        if (!bfs.FindOptimalPath(source, destination, out var path))
        {
            return null;
        }

        List<Channel> channels = [];

        for (var i = 0; i < path.Count - 1; i++)
        {
            channels.Add(path[i].Channels.Where(c => c.Node1 == path[i + 1] || c.Node2 == path[i + 1]).First());
        }

        var time = 0;

        var servicePacketsSentCount = 0;

        if (protocol is Protocol.TCP)
        {
            // Step 1 handshake
            SendServicePackents(servicePacketLenght, channels, ref time, ref servicePacketsSentCount);

            // Step 2 handshake
            SendServicePackents(servicePacketLenght, channels, ref time, ref servicePacketsSentCount);

            // Step 3 handshake
            SendServicePackents(servicePacketLenght, channels, ref time, ref servicePacketsSentCount);

            // Send data packets
            SendInfoPacketsWithRetry(infoPacketLenght, infoPacketsCount, channels, ref time, ref servicePacketsSentCount);

            // ACK
            SendServicePackents(servicePacketLenght, channels, ref time, ref servicePacketsSentCount);

            // FIN
            SendServicePackents(servicePacketLenght, channels, ref time, ref servicePacketsSentCount);

            // ACK
            SendServicePackents(servicePacketLenght, channels, ref time, ref servicePacketsSentCount);

            // FIN
            SendServicePackents(servicePacketLenght, channels, ref time, ref servicePacketsSentCount);

            // ACK
            SendServicePackents(servicePacketLenght, channels, ref time, ref servicePacketsSentCount);
        }
        else
        {
            SendInfoPacketsWithoutRetry(infoPacketLenght, infoPacketsCount, channels, ref time, ref servicePacketsSentCount);
        }

        return new Simulation(
            protocol,
            source,
            destination,
            servicePacketLenght,
            infoPacketLenght,
            infoPacketsCount,
            (infoPacketLenght * infoPacketsCount) + (servicePacketLenght * servicePacketsSentCount),
            time);
    }

    private void SendInfoPacketsWithRetry(int infoPacketLenght, int infoPacketsCount, List<Channel> channels,  ref int time, ref int servicePacketsSentCount)
    {
        for (int i = 0; i < infoPacketsCount; i++)
        {
            // DATA
            foreach (var channel in channels)
            {
                var result = SendPacketWithRetry(channel, infoPacketsCount);

                time += result.time;
                servicePacketsSentCount += result.sendCount;
            }
        }
    }

    private void SendInfoPacketsWithoutRetry(int infoPacketLenght, int infoPacketsCount, List<Channel> channels, ref int time, ref int servicePacketsSentCount)
    {
        for (int i = 0; i < infoPacketsCount; i++)
        {
            // DATA
            foreach (var channel in channels)
            {
                var result = SendPacketWithoutRetry(channel, infoPacketsCount);

                time += result.time;
                servicePacketsSentCount += result.sendCount;
            }
        }
    }

    private void SendServicePackents(int servicePacketLenght, List<Channel> channels, ref int time, ref int servicePacketsSentCount)
    {
        foreach (var channel in channels)
        {
            var result = SendPacketWithRetry(channel, servicePacketLenght);

            time += result.time;
            servicePacketsSentCount += result.sendCount;
        }
    }

    private (int time, int sendCount) SendPacketWithRetry(Channel channel, int packetLenght, int time = 0, int sendCount = 1)
    {
        time += channel.Weight + (packetLenght / channel.Weight);

        bool isDelivered = _random.NextDouble() > channel.ChanceOfError;

        if (!isDelivered)
        {
            return SendPacketWithRetry(channel, packetLenght, time, sendCount + 1);
        }

        if (channel.Type == ChanelType.Duplex)
        {
            time /= 2;
        }

        return (time + channel.Weight + (packetLenght / channel.Weight), sendCount);
    }

    private (int time, int sendCount) SendPacketWithoutRetry(Channel channel, int packetLenght, int time = 0, int sendCount = 1)
    {
        time += channel.Weight + (packetLenght / channel.Weight);

        bool isDelivered = _random.NextDouble() > channel.ChanceOfError;

        if (!isDelivered)
        {
            return (time + channel.Weight + (packetLenght / channel.Weight), 0);
        }

        if (channel.Type == ChanelType.Duplex)
        {
            time /= 2;
        }

        return (time + channel.Weight + (packetLenght / channel.Weight), sendCount);
    }
}
