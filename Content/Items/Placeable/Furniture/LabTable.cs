using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class LabTable : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.LabTable>());
			Item.width = 38;
			Item.height = 24;
            Item.value = Item.buyPrice(0, 0, 1, 50);
        }
	}
}
