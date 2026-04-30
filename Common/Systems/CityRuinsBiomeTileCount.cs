using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Content.Tiles.Latex;
using System;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems
{
	public class CityRuinsBiomeTileCount : ModSystem
	{
		public int NBlocksNeeded = 800;
		public int DryDirtBlockCount;
		public int WhiteLatexBlockCount;
		public int BlackLatexBlockCount;

		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts) 
		{
			DryDirtBlockCount = tileCounts[ModContent.TileType<DryDirt>()];
			WhiteLatexBlockCount = tileCounts[ModContent.TileType<WhiteLatexTile>()] + tileCounts[ModContent.TileType<WhiteLatexSandTile>()];
            BlackLatexBlockCount = tileCounts[ModContent.TileType<BlackLatexTile>()] + tileCounts[ModContent.TileType<BlackLatexSandTile>()];
        }
	}
}
