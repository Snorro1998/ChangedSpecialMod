using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class WhiteLatexBookcases : ModItem
	{
		public override void SetDefaults() 
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.WhiteLatexBookcases>());
			Item.width = 32;
			Item.height = 32;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 1);
		}
		
		public override void AddRecipes() 
		{
			CreateRecipe()
				.AddIngredient(ItemID.Book, 10)
				.AddIngredient(ModContent.ItemType<WhiteLatexBlock>(), 20)
				.AddTile(TileID.Sawmill)
				.Register();
		}
	}
}
