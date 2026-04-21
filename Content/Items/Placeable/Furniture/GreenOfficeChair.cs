using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class GreenOfficeChair : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.GreenOfficeChair>());
			Item.width = 12;
			Item.height = 30;
            Item.value = Item.buyPrice(0, 0, 1, 50);
        }

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.IronBar, 2)
                .AddIngredient(ItemID.Silk)
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.LeadBar, 2)
                .AddIngredient(ItemID.Silk)
                .AddTile(TileID.Anvils)
                .Register();
        }
	}
}
