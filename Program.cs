using System.Text.Json;

namespace BiomeTreeGenerator;

internal class Program
{
    private static void Main(string[] args)
    {
        if (args.Length != 3)
        {
            PrintUsage();
            return;
        }

        var inputFileName = args[0];
        var outputFileName = args[1];
        var postfix = args[2];

        Console.WriteLine("Loading biome id mapping from biomeids.json");
        var biomeIds = JsonSerializer.Deserialize<Dictionary<string, byte>>(File.ReadAllText("biomeids.json"));
        if (biomeIds == null || biomeIds.Count == 0)
            throw new InvalidDataException("Error loading biome ID mapping.");


        Console.WriteLine($"Loading biome search tree from {inputFileName}");
        var tree = JsonSerializer.Deserialize<TreeNode>(File.ReadAllText(inputFileName), new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var unknownBiomes = TreeTools.GetUnknownBiomes(tree, biomeIds).Distinct().OrderBy(x => x).ToList();
        if (unknownBiomes.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine("Missing biome ID mappings:");
            Console.WriteLine("--------------------------");
            Console.WriteLine();
            foreach (var b in unknownBiomes)
                Console.WriteLine(b);
            Console.WriteLine();
            Console.WriteLine("Add these biomes to biomeids.json to make this work.");

            Environment.Exit(-1);
        }


        Console.WriteLine("Converting biome tree to binary format");

        // set root node parameters to zero
        //TODO find out why cubiomes does not work correctly without this
        tree.Parameters = tree.Parameters.Select(x => new Range(0, 0)).ToArray();

        var converter = new BinaryConverter(biomeIds);
        var result = converter.Convert(tree);

        Console.WriteLine("Generating C header");
        using (var writer = new StreamWriter(outputFileName))
        {
            SourceGenerator.Generate(writer, postfix, result);
        }

        Console.WriteLine("Done");
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine();
        Console.WriteLine("  BiomeTreeGenerator.exe inputfile outputfile postfix");
        Console.WriteLine();
        Console.WriteLine("Arguments:");
        Console.WriteLine();
        Console.WriteLine(
            "  inputfile         Name of the JSON file obtained by the BiomeTreeExtractor Fabric mod.");
        Console.WriteLine("  outputfile        Name of the C header file to be generated for use with cubiomes.");
        Console.WriteLine("  postfix           A postfix to be added to identifiers in the generated C header.");
        Console.WriteLine();
        Console.WriteLine("Example:");
        Console.WriteLine();
        Console.WriteLine(
            "  BiomeTreeGenerator.exe biometreeextract_minecraft_overworld.json btree24w40a.h 24w40a");
        Console.WriteLine();
    }
}