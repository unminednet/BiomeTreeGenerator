# BiomeTreeGenerator

BiomeTreeGenerator is a source code generation tool __for developers__ to convert biome search tree extracted from Minecraft to a C header file for use with [cubiomes](https://github.com/Cubitect/cubiomes).

## Usage

1. Build (NET8+ SDK, "dotnet build")
2. Extract the biome search tree from Minecraft using the [BiomeTreeExtractor](https://github.com/unminednet/BiomeTreeExtractor) Fabric mod
3. Adjust biomeids.json to your needs (add missing vanilla or modded biomes with the numeric IDs used by cubiomes)
4. Run this tool to generate a C header file that contains a binary representation of the extracted biome search tree
5. Modify cubiomes to use data from the generated C header file

## License

MIT
