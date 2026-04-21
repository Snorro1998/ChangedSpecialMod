using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class PuroPlush : ModItem, ILocalizedModType
    {
		public override void SetDefaults() {
			// Vanilla has many useful methods like these, use them! This substitutes setting Item.createTile and Item.placeStyle as well as setting a few values that are common across all placeable items
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.PuroPlush>());

			Item.width = 32;
			Item.height = 32;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 1);
		}
		
		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.FlinxFur, 2)
				.AddTile(TileID.Loom)
				.Register();
		}
	}
}
