using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class DocumentPaper : ModItem
	{
		public override void SetDefaults() {
			// Vanilla has many useful methods like these, use them! This substitutes setting Item.createTile and Item.placeStyle as well as setting a few values that are common across all placeable items
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.DocumentPaper>());

			Item.width = 32;
			Item.height = 32;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 1);
		}
		
		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Wood, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
