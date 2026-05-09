using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Crystals
{
	public class CrystalWhite : ModItem
	{
		public override void SetDefaults() 
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.CrystalWhite>());
			Item.width = 32;
			Item.height = 32;
			Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 0, 25);
        }
	}
}
