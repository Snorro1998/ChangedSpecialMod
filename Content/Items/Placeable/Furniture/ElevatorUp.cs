using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	// This is a dummy item. It is only used to display as an icon
	public class ElevatorUp : ModItem
	{
		public override void SetDefaults() 
		{
			Item.width = 48;
			Item.height = 48;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 1);
		}
	}
}
