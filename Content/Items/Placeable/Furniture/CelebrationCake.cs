using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Content.Tiles.Latex.Black;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class CelebrationCake : ModItem
	{
        public override void SetStaticDefaults()
        {
            //ItemID.Sets.CanGetPrefixes[Type] = false;
            //ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox;
        }

		public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<CelebrationCakeTile>());
            Item.value = Item.buyPrice(0, 5);
        }
	}
}
