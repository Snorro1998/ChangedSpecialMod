using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles
{
	// This file contains 3 classes and shows off using inheritance to share code between classes.
	// Terraria has many tiles that are purely decorative and do not drop items when broken.
	// These tiles go by many names, such as ambient tiles, background tiles, piles, detritus, and rubble. We will use the term rubble because of the recently added Rubblemaker item. 
	// The Rubblemaker (https://terraria.wiki.gg/wiki/Rubblemaker) is a special item that can place these decorative tiles. The tile placed by the Rubblemaker looks the same as the original rubble tile but behaves slightly differently.

	// Example1x1RubbleBase is an abstract class, it is not an actual tile, but the other 2 classes in this file will reuse the Texture and SetStaticDefaults code shown here because they inherit from it. 

	public class WhiteLatexGrass : ModTile
	{
		// We want both tiles to use the same texture
		public override string Texture => "ChangedSpecialMod/Content/Tiles/WhiteLatexGrass";

		public override void SetStaticDefaults() 
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileObsidianKill[Type] = true;

			DustType = DustID.Stone;

            Main.tileCut[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileNoAttach[Type] = true;
            Main.tileNoFail[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.ReplaceTileBreakUp[Type] = true;
            TileID.Sets.SwaysInWindBasic[Type] = true;
            TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);

            HitSound = SoundID.Grass;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);

			AddMapEntry(new Color(152, 171, 198));
		}
	}
}