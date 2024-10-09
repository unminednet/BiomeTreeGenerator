namespace BiomeTreeGenerator;

public class BinaryConverterResult
{
    public int Order { get; init; }
    public int Depth { get; init; }
    public required int[] Steps { get; set; }
    public required Range[] Ranges { get; set; }
    public required ulong[] Nodes { get; set; }
}