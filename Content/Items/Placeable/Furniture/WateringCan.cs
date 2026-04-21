using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class WateringCan : ModItem, ILocalizedModType
    {
		public override void SetDefaults() 
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.WateringCan>());
			Item.width = 32;
			Item.height = 32;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 1);
		}
		
		public override void AddRecipes() 
		{
			CreateRecipe()
				.AddIngredient(ItemID.LeadBar, 2)
				.AddTile(TileID.Anvils)
				.Register();

            CreateRecipe()
				.AddIngredient(ItemID.IronBar, 2)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
