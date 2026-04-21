using ChangedSpecialMod.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture.Paintings
{
	public class Pictures6 : ModTile
	{
        public override string Texture => "ChangedSpecialMod/Content/Tiles/Furniture/Pictures/Pictures1";
        public override void SetStaticDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.AnchorWall = true;
            TileObjectData.addTile(Type);
			DustType = DustID.WoodFurniture;
		}

        public override bool RightClick(int i, int j)
        {
            return PictureSystem.OpenPictureViewer(i, j, PictureSystem.ImagesColinPuro);
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<Items.Placeable.Furniture.Pictures1>();
        }
    }
}
