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
            $"static const uint32_t btree{postfix}_steps[] = {{ {string.Join(", ", data.Steps.Select(x => x.ToString()))} }};");
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
        writer.WriteLine(
            """
                // Binary encoded biome parameter search tree
                //
                //   +-------------- If the top byte equals 0xFF, the node is a leaf and the
                //   |               second byte is the biome id, otherwise the two bytes
                //   |               are a short index to the first child node.
                //   |
                //   | +------------ Biome parameter index for 5 (weirdness)
                //   | | +---------- Biome parameter index for 4 (depth)
                //   | | | +-------- Biome parameter index for 3 (erosion)
                //   | | | | +------ Biome parameter index for 2 (continentalness)
                //   | | | | | +---- Biome parameter index for 1 (humidity)
                //   | | | | | | +-- Biome parameter index for 0 (temperature)
                //   | | | | | | |
                //   v v v v v v v
            """);


        c = 0;
        var n = 1;
        foreach (var node in data.Nodes)
        {
            if (c == 0) writer.Write("    ");

            writer.Write($"0x{node:X16},");

            c++;
            if (c == n)
            {
                writer.WriteLine();
                c = 0;
                n = 4;
            }
        }

        if (c != 0) writer.WriteLine();

        writer.WriteLine("};");
    }
}