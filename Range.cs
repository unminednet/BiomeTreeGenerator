namespace BiomeTreeGenerator;

public record Range(int Min, int Max) : IComparable<Range>
{
    public int Min { get; set; } = Min;
    public int Max { get; set; } = Max;

    public int CompareTo(Range? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        var minComparison = Min.CompareTo(other.Min);
        if (minComparison != 0) return minComparison;
        return Max.CompareTo(other.Max);
    }
}