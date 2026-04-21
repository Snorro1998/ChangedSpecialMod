using ChangedSpecialMod.Content.Tiles.Furniture;
using Terraria;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class LabDoor : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<LabDoorClosed>());
			Item.width = 14;
			Item.height = 28;
            Item.value = Item.buyPrice(0, 0, 1, 50);
        }

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		
	}
}