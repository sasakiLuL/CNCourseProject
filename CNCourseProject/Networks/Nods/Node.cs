using ComputerNetworksCourseWork.Networks.Channels;

namespace ComputerNetworksCourseWork.Networks.Nods;

public class Node
{
    private readonly List<Channel> _connections = [];

    public Node(int id, List<Channel> connections)
    {
        Id = id;
        _connections = connections;
    }

    public int Id { get; init; }

    public IReadOnlyList<Channel> Channels => _connections.AsReadOnly();

    public void AddConnection(Channel connection)
    {
        _connections.Add(connection);
    }

    public void RemoveConnection(Channel connection)
    {
        _connections.Remove(connection);
    }
}
