namespace BiomeTreeGenerator;

public static class TreeTools
{
    public static IEnumerable<string> GetUnknownBiomes(TreeNode node, Dictionary<string, byte> biomeIds)
    {
        if (node.SubTree != null)
        {
            foreach (var biome in node.SubTree.SelectMany(n => GetUnknownBiomes(n, biomeIds)))
                yield return biome;
        }
        else if (node.Biome != null && !biomeIds.ContainsKey(node.Biome))
        {
            yield return node.Biome;
        }
    }
}