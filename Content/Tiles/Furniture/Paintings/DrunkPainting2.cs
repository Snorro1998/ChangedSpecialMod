using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture.Paintings
{
	public class DrunkPainting2 : ModTile
	{
        public override string Texture => "ChangedSpecialMod/Content/Tiles/Furniture/Paintings/DrunkPainting2";
        public override void SetStaticDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.AnchorWall = true;
			TileObjectData.addTile(Type);
			DustType = DustID.WoodFurniture;
		}
    }
}
