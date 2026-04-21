using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChangedSpecialMod.Content.Tiles;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class MusicBoxWhiteTail : ModItem
	{
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox;
        }

		public override void SetDefaults() 
        {
            Item.DefaultToMusicBox(ModContent.TileType<MusicBoxWhiteTailTile>(), 0);
            Item.value = Item.buyPrice(0, 5);
        }
	}
}
