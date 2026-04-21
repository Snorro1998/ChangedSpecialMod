using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
	public class WhiteLatexBookcases : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileFrameImportant[Type] = true;
            Main.tileSolidTop[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.IgnoredByNpcStepUp[Type] = true; // This line makes NPCs not try to step up this tile during their movement. Only use this for furniture with solid tops.
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.addTile(Type);
		}
	}
}
