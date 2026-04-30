using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class SharkPlush : ModItem, ILocalizedModType
    {
		public override void SetDefaults() 
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.SharkPlush>());
			Item.width = 32;
			Item.height = 32;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 1);
		}
		
		public override void AddRecipes() 
		{
			CreateRecipe()
				.AddIngredient(ItemID.FlinxFur, 2)
				.AddTile(TileID.Loom)
				.Register();
		}
	}
}
