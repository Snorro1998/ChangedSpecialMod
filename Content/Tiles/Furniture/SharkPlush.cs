using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
	public class SharkPlush : ModTile
	{
		public override void SetStaticDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            //TileID.Sets.FramesOnKillWall[Type] = true;
            //TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            //TileObjectData.addTile(Type);
            //DustType = DustID.WoodFurniture;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);

            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            // The following 3 lines are needed if you decide to add more styles and stack them vertically
            TileObjectData.newTile.StyleWrapLimit = 2;
            TileObjectData.newTile.StyleMultiplier = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1); // Facing right will use the second texture style

            TileID.Sets.FramesOnKillWall[Type] = false;
            TileObjectData.addTile(Type);
            DustType = DustID.WoodFurniture;
        }
    }
}
