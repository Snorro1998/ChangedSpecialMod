using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class Pictures6 : ModItem
	{
        public override string Texture => "ChangedSpecialMod/Content/Items/Placeable/Furniture/Pictures1";

        public override void SetDefaults() 
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.Paintings.Pictures6>());
			Item.width = 48;
			Item.height = 48;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 1);
		}
	}
}
