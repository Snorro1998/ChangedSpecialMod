using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
	public class Blocks : ModTile
	{
		public override void SetStaticDefaults() 
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.RandomStyleRange = 4;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.addTile(Type);
			DustType = DustID.WoodFurniture;
		}

        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<Items.Weapons.ToyBlock>(), 50);
        }
    }
}
