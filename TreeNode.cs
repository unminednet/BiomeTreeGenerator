namespace BiomeTreeGenerator;

public class TreeNode
{
    public Range[] Parameters { get; set; }
    public TreeNode[]? SubTree { get; set; }
    public string? Biome { get; set; }
}