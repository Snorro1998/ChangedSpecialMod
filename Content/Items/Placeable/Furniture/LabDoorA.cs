using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class LabDoorA : ModItem
	{
        public override string Texture => "ChangedSpecialMod/Content/Items/Placeable/Furniture/LabDoorA";

        public override void SetDefaults() 
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.LabDoorATile>());
			Item.width = 48;
			Item.height = 48;
			Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 1);
        }
	}

    public class LabDoorB : ModItem
    {
        public override string Texture => "ChangedSpecialMod/Content/Items/Placeable/Furniture/LabDoorA";

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.LabDoorBTile>());
            Item.width = 48;
            Item.height = 48;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 1);
        }
    }
}
