using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class DocumentPaper : ModItem
	{
		public override void SetDefaults() 
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.DocumentPaper>());
			Item.width = 32;
			Item.height = 32;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 1);
		}
		
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Paper>(), 1)
				.AddIngredient(ItemID.BlackInk, 1)
				.Register();
		}
	}
}
