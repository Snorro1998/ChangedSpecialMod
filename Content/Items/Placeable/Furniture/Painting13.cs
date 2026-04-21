using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class Painting13 : ModItem
	{
		public override void SetDefaults() {
			// Vanilla has many useful methods like these, use them! This substitutes setting Item.createTile and Item.placeStyle as well as setting a few values that are common across all placeable items
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.Paintings.Painting13>());

			Item.width = 96;
			Item.height = 64;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 1);
		}
	}
}
