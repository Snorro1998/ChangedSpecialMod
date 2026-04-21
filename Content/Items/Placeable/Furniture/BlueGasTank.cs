using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class BlueGasTank : ModItem
    {
		public override void SetDefaults() 
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.BlueGasTank>());
			Item.width = 32;
			Item.height = 32;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 1);
		}
		
		public override void AddRecipes() 
		{
			CreateRecipe()
				.AddIngredient(ItemID.IronBar, 4)
				.AddTile(TileID.Anvils)
				.Register();
            CreateRecipe()
				.AddIngredient(ItemID.LeadBar, 4)
				.AddTile(TileID.Anvils)
				.Register();
        }
	}
}
