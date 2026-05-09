using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class Elevator : ModItem
	{
		public override void SetDefaults() 
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.Elevator>());
			Item.width = 48;
			Item.height = 48;
			Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 1);
        }
	}
}
