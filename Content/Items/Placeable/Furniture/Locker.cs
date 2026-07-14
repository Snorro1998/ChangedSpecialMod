using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class Locker : ModItem
	{
		public override void SetDefaults() 
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.Locker>());
			Item.width = 32;
			Item.height = 32;
            Item.value = Item.buyPrice(0, 0, 1, 50);
        }
	}
}
