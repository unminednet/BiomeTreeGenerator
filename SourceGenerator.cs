namespace BiomeTreeGenerator;

public static class SourceGenerator
{

    public static void Generate(StreamWriter writer, string postfix, BinaryConverterResult data)
    {
        writer.WriteLine("#include <inttypes.h>");
        writer.WriteLine();
        writer.WriteLine($"enum {{ btree{postfix}_order = {data.Order} }};");
        writer.WriteLine();
        writer.WriteLine(
            $"static const uint32_t btree{postfix}_steps[] = {{ {string.Join(',', data.Steps.Select(x => x.ToString()))} }};");
        writer.WriteLine();
        writer.WriteLine($"static const int32_t btree{postfix}_param[][2] =");
        writer.WriteLine("{");

        var c = 0;
        var i = 0;
        foreach (var range in data.Ranges)
        {
            if (c == 0) writer.Write("    ");

            writer.Write($"{{{range.Min.ToString(),6},{range.Max.ToString(),6}}}, ");

            c++;
            i++;
            if (c == 4)
            {
                writer.WriteLine($"// {i - 4:X2}-{i - 1:X2}");
                c = 0;
            }
        }

        if (c != 0) writer.WriteLine();

        writer.WriteLine("};");
        writer.WriteLine();
        writer.WriteLine($"static const uint64_t btree{postfix}_nodes[] =");
        writer.WriteLine("{");

        c = 0;
        foreach (var node in data.Nodes)
        {
            if (c == 0) writer.Write("    ");

            writer.Write($"0x{node:X16},");

            c++;
            if (c == 4)
            {
                writer.WriteLine();
                c = 0;
            }
        }

        if (c != 0) writer.WriteLine();

        writer.WriteLine("};");
    }
}