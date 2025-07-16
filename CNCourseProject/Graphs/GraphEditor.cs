using ComputerNetworksCourseWork.Networks.Channels;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Layout.MDS;
using Node = ComputerNetworksCourseWork.Networks.Nods.Node;
using MsaglStyle = Microsoft.Msagl.Drawing.Style;
using Color = Microsoft.Msagl.Drawing.Color;
using ComputerNetworksCourseWork.Networks;
namespace ComputerNetworksCourseWork.Graphs;

public class GraphEditor
{
    public required GViewer GViewer { get; set; }

    public required Network Network { get; set; }

    public void BuildGraph()
    {
        var graph = new Graph("Graph");

        graph.LayoutAlgorithmSettings = new MdsLayoutSettings();

        foreach (var node in Network.Nodes)
        {
            var nodeSprite = graph.AddNode(node.Id.ToString());
            nodeSprite.Attr.Shape = Shape.Circle;
            nodeSprite.Attr.XRadius = 20;
            nodeSprite.Attr.YRadius = 20;
            nodeSprite.Attr.LabelMargin = 10;
            nodeSprite.Label.FontSize = 26;
            nodeSprite.Attr.FillColor = Color.Azure;
        }

        foreach (var channel in Network.Channels)
        {
            var edge = graph.AddEdge(
                channel.Node1.Id.ToString(),
                $"{channel.Flow.ToString()}/{channel.Weight.ToString()}",
                channel.Node2.Id.ToString());

            edge.Attr.Color = GetEdgeColor(channel.Type);
            edge.Attr.LineWidth = 5;
            edge.Attr.AddStyle(GetEdgeStyles(channel.Type));

            edge.Attr.ArrowheadAtSource = ArrowStyle.Normal;
            edge.Attr.ArrowheadAtTarget = ArrowStyle.Normal;

            edge.Label.FontColor = GetEdgeColor(channel.Type);
            edge.Label.FontSize = 26;
        }

        GViewer.Graph = graph;
    }

    public void AddHightlight(Node source, Node dest, Dictionary<Node, Channel> path, Color color, Color labelColor)
    {
        var sourceSprite = GViewer.Graph.FindNode(source.Id.ToString());

        var destSprite = GViewer.Graph.FindNode(dest.Id.ToString());

        sourceSprite.Attr.FillColor = color;
        destSprite.Attr.FillColor = color;

        foreach (var elem in path)
        {
            var node = GViewer.Graph.FindNode(elem.Key.Id.ToString());

            var connection = GViewer.Graph.Edges
                .Where(e => e.Source == elem.Value.Node1.Id.ToString() &&
                            e.Target == elem.Value.Node2.Id.ToString())
                .First();
            node.Attr.FillColor = color;

            connection.Attr.Color = color;
            connection.Attr.LineWidth = 8;
            connection.Label.FontColor = labelColor;
            connection.Label.Text = $"{elem.Value.Flow.ToString()}/{elem.Value.Weight.ToString()}";
        }
    }

    public void AddHightlight(Node source, Node dest, List<Node> path, Color color, Color labelColor)
    {
        var sourceSprite = GViewer.Graph.FindNode(source.Id.ToString());

        var destSprite = GViewer.Graph.FindNode(dest.Id.ToString());

        foreach (var elem in path)
        {
            var sprite = GViewer.Graph.FindNode(elem.Id.ToString());

            sprite.Attr.FillColor = color;
        }

        List<Channel> channels = [];

        for (var i = 0; i < path.Count - 1; i++)
        {
            channels.Add(path[i].Channels.Where(c => c.Node1 == path[i + 1] || c.Node2 == path[i + 1]).First());
        }

        foreach (var chanel in channels)
        {
            var connection = GViewer.Graph.Edges
                .Where(e => e.Source == chanel.Node1.Id.ToString() &&
                            e.Target == chanel.Node2.Id.ToString())
                .First();

            connection.Attr.Color = color;
            connection.Attr.LineWidth = 8;
            connection.Label.FontColor = labelColor;
        }
    }

    public void ClearHightlight(Node source, Node dest, Dictionary<Node, Channel> path)
    {
        var sourceSprite = GViewer.Graph.FindNode(source.Id.ToString());

        var destSprite = GViewer.Graph.FindNode(dest.Id.ToString());

        sourceSprite.Attr.FillColor = Color.Azure;
        destSprite.Attr.FillColor = Color.Azure;

        foreach (var elem in path)
        {
            var node = GViewer.Graph.FindNode(elem.Key.Id.ToString());

            var connection = GViewer.Graph.Edges
                .Where(e => e.Source == elem.Value.Node1.Id.ToString() &&
                            e.Target == elem.Value.Node2.Id.ToString())
                .First();

            node.Attr.FillColor = Color.Azure;

            connection.Attr.Color = GetEdgeColor(elem.Value.Type);
            connection.Attr.AddStyle(GetEdgeStyles(elem.Value.Type));
            connection.Attr.LineWidth = 5;
            connection.Label.FontColor = GetEdgeColor(elem.Value.Type);

            connection.Label.Text = $"{elem.Value.Flow.ToString()}/{elem.Value.Weight.ToString()}";
        }
    }

    public void ClearHightlight(Node source, Node dest, List<Node> path)
    {
        var sourceSprite = GViewer.Graph.FindNode(source.Id.ToString());

        var destSprite = GViewer.Graph.FindNode(dest.Id.ToString());

        sourceSprite.Attr.FillColor = Color.Azure;
        destSprite.Attr.FillColor = Color.Azure;

        foreach (var elem in path)
        {
            var sprite = GViewer.Graph.FindNode(elem.Id.ToString());

            sprite.Attr.FillColor = Color.Azure;
        }

        List<Channel> channels = [];

        for (var i = 0; i < path.Count - 1; i++)
        {
            channels.Add(path[i].Channels.Where(c => c.Node1 == path[i + 1] || c.Node2 == path[i + 1]).First());
        }

        foreach (var chanel in channels)
        {
            var connection = GViewer.Graph.Edges
                .Where(e => e.Source == chanel.Node1.Id.ToString() &&
                            e.Target == chanel.Node2.Id.ToString())
                .First();

            connection.Attr.Color = GetEdgeColor(chanel.Type);
            connection.Attr.AddStyle(GetEdgeStyles(chanel.Type));
            connection.Attr.LineWidth = 5;
            connection.Label.FontColor = GetEdgeColor(chanel.Type);

            connection.Label.Text = $"{chanel.Flow.ToString()}/{chanel.Weight.ToString()}";
        }
    }

    public void Update()
    {
        GViewer.Graph = GViewer.Graph;
    }

    private static MsaglStyle GetEdgeStyles(ChanelType channelType)
        => channelType switch
        {
            ChanelType.Duplex => MsaglStyle.Solid,
            ChanelType.HalfDuplex => MsaglStyle.Solid,
            ChanelType.Satellite => MsaglStyle.Dotted,
            _ => MsaglStyle.Solid,
        };

    private static Color GetEdgeColor(ChanelType channelType)
        => channelType switch
        {
            ChanelType.Duplex => Color.Black,
            ChanelType.HalfDuplex => Color.DarkGray,
            ChanelType.Satellite => Color.DarkCyan,
            _ => Color.Black,
        };
}
