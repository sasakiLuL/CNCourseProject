using ComputerNetworksCourseWork.Networks.Channels;
using ComputerNetworksCourseWork.Networks.Nods;

namespace ComputerNetworksCourseWork.Analyzes.Algorithms;

public class BFS
{
    public bool FindParent(Node source, Node destination, out Dictionary<Node, Channel> parent)
    {
        parent = [];

        Queue<Node> queue = [];

        HashSet<Node> visited = [];

        queue.Enqueue(source);

        visited.Add(source);

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();

            foreach (Channel channel in current.Channels)
            {
                Node nextNode = channel.OppositeTo(current);

                if (!visited.Contains(nextNode) && channel.Weight > channel.Flow)
                {
                    parent[nextNode] = channel;

                    visited.Add(nextNode);

                    if (nextNode == destination)
                    {
                        return true;
                    }

                    queue.Enqueue(nextNode);
                }
            }
        }

        return false;
    }

    public bool FindOptimalPath(Node source, Node destination, out List<Node> path)
    {
        Queue<Node> queue = new Queue<Node>();
        Dictionary<Node, Node> parent = new Dictionary<Node, Node>();
        HashSet<Node> visited = new HashSet<Node>();

        queue.Enqueue(source);
        visited.Add(source);

        while (queue.Count > 0)
        {
            Node? current = queue.Dequeue();

            if (current == destination)
            {
                path = new List<Node>();
                while (current != null)
                {
                    path.Add(current);
                    parent.TryGetValue(current, out current);
                }
                path.Reverse();
                return true;
            }

            // Додаємо сусідні вузли
            foreach (var channel in current!.Channels)
            {
                Node nextNode = channel.OppositeTo(current);
                if (!visited.Contains(nextNode))
                {
                    queue.Enqueue(nextNode);
                    visited.Add(nextNode);
                    parent[nextNode] = current;
                }
            }
        }

        path = [];
        return false;
    }
}
