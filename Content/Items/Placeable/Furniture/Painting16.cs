using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class Painting16 : ModItem
	{
		public override void SetDefaults() 
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.Paintings.Painting16>());
			Item.width = 32;
			Item.height = 32;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 1);
		}
	}
}
