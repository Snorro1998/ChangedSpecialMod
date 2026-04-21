using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
	public class DocumentPaper : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileSign[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.addTile(Type);
			DustType = 7;
		}
	}
}
