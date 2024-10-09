using System.Text;

namespace BiomeTreeGenerator;

/// <summary>
///     Converts a biome search tree to binary format for use with cubiomes
/// </summary>
public class BinaryConverter(Dictionary<string, byte> biomeIds)
{
    public BinaryConverterResult Convert(TreeNode rootNode)
    {
        if (rootNode.SubTree == null || rootNode.SubTree.Length == 0)
            throw new ArgumentException("The root node has no child nodes.");

        var order = rootNode.SubTree.Length;
        var depth = GetNodeDepth(rootNode);
        var steps = new int[depth];

        steps[depth - 1] = 0;
        steps[depth - 2] = 1;
        var f = 6;
        for (var i = depth - 3; i >= 0; i--)
        {
            steps[i] = steps[i + 1] + f;
            f *= 6;
        }

        var ranges = new List<Range>();
        var nodes = new List<ulong>();

        DumpNode(rootNode, ranges, nodes);

        return new BinaryConverterResult
        {
            Order = order,
            Depth = depth,
            Steps = steps,
            Nodes = nodes.ToArray(),
            Ranges = ranges.ToArray()
        };
    }

    private static int GetNodeDepth(TreeNode node)
    {
        if (node.SubTree is { Length: > 0 })
            return 1 + node.SubTree.Select(GetNodeDepth).Max();

        return 1;
    }

    private void DumpNode(TreeNode node, List<Range> ranges, List<ulong> nodes)
    {
        var value = 0ul;

        // Minecraft has 7 parameters, we use 6 (skip "offset")
        foreach (var p in node.Parameters.Take(6).Reverse())
        {
            var i = ranges.IndexOf(p);
            if (i < 0)
            {
                ranges.Add(p);
                i = ranges.Count - 1;
            }

            value = (value << 8) + (ulong)i;
        }

        if (node.SubTree != null)
        {
            var firstChildIndex = (ulong)(nodes.Count + 1);
            value = (firstChildIndex << 48) + value;
            nodes.Add(value);
            foreach (var n in node.SubTree)
                DumpNode(n, ranges, nodes);
        }
        else
        {
            if (!biomeIds.TryGetValue(node.Biome, out var biomeId))
                throw new KeyNotFoundException($"No ID mapping found for biome \"{node.Biome}\"");

            value = ((0xff00 + (ulong)biomeId) << 48) + value;
            nodes.Add(value);
        }
    }


    private static bool HasOrder(TreeNode node, int order)
    {
        if (node.SubTree == null) return true;

        if (node.SubTree.Length != order)
            return false;

        if (node.SubTree.Any(n => !HasOrder(n, order))) return false;

        return true;
    }
}