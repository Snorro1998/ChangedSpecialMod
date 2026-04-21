using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class StackOfBoxes : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.StackOfBoxes>());
			Item.width = 32;
			Item.height = 32;
			Item.value = 150;
		}
	}
}
