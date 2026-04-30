using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class StorageBox : ModItem
	{
		public override void SetDefaults() 
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.StorageBox>());
			Item.width = 26;
			Item.height = 22;
			Item.value = 500;
		}		
	}

	public class StorageBoxKey : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Item.ResearchUnlockCount = 3;
		}

		public override void SetDefaults() 
		{
			Item.CloneDefaults(ItemID.GoldenKey);
		}
	}
}
