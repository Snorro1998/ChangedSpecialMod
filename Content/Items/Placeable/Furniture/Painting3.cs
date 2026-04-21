using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class Painting3 : ModItem
	{
		public override void SetDefaults() 
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.Paintings.Painting3>());
			Item.width = 48;
			Item.height = 48;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 1);
		}
	}
}
