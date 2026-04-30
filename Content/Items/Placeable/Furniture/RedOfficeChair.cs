using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class RedOfficeChair : ModItem
	{
		public override void SetDefaults() 
        {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.RedOfficeChair>());
			Item.width = 12;
			Item.height = 30;
            Item.value = Item.buyPrice(0, 0, 1, 50);
        }

		public override void AddRecipes() 
        {
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
