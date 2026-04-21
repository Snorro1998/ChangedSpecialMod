
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class IrisScanner : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.IrisScanner>());
            Item.value = Item.buyPrice(0, 0, 1, 50);
        }
	}
}