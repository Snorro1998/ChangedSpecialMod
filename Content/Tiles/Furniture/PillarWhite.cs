using System.Collections.Generic;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
	public class PillarWhite : ModTile
	{
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);

            TileObjectData.newTile.RandomStyleRange = 4;
            TileObjectData.newTile.StyleHorizontal = true;
            /*
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            // The following 3 lines are needed if you decide to add more styles and stack them vertically
            TileObjectData.newTile.StyleWrapLimit = 2;
            TileObjectData.newTile.StyleMultiplier = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1); // Facing right will use the second texture style
            */
            TileID.Sets.FramesOnKillWall[Type] = false;
            TileObjectData.addTile(Type);
            DustType = DustID.WoodFurniture;
        }

        // Ignore style nonsense and always drop the correct item
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<Items.Placeable.Furniture.PillarWhite>());
        }
    }
}
