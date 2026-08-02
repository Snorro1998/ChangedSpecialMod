using ChangedSpecialMod.Content.Tiles;
using Terraria;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class CelebrationCake : ModItem
	{
		public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<CelebrationCakeTile>());
            Item.value = Item.buyPrice(0, 5);
        }
	}
}
