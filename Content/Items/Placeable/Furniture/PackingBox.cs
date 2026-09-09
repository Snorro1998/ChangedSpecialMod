using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class PackingBox : ModItem
	{
		public override void SetDefaults() 
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.PackingBox>());
			Item.width = 26;
			Item.height = 22;
			Item.value = 500;
		}	
	}

	public class PackingBoxKey : ModItem
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
