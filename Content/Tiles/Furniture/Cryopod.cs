using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
	public class Cryopod : ModTile
	{
		public override void SetStaticDefaults() 
		{
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileFrameImportant[Type] = true;
            Main.tileSolidTop[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.IgnoredByNpcStepUp[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.addTile(Type);
		}
	}
}
