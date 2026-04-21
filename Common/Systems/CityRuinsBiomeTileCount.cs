using ChangedSpecialMod.Content.Tiles;
using System;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems
{
	public class CityRuinsBiomeTileCount : ModSystem
	{
		public int DryDirtBlockCount;
		public int WhiteLatexBlockCount;
		public int BlackLatexBlockCount;

		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts) 
		{
			DryDirtBlockCount = tileCounts[ModContent.TileType<DryDirt>()];
			WhiteLatexBlockCount = tileCounts[ModContent.TileType<WhiteLatexTile>()];
			BlackLatexBlockCount = tileCounts[ModContent.TileType<BlackLatexTile>()];
        }
	}
}
