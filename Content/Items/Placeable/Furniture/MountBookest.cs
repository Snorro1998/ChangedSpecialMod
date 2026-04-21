using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class MountBookest : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.MountBookest>());
			Item.width = 38;
			Item.height = 24;
			Item.value = 150;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Book, 10)
				.Register();
		}
	}
}
