using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChangedSpecialMod.Content.Tiles;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
	public class MusicBoxCrystalZone : ModItem
	{
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox;
            //MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, Assets.Sounds.MusicBlackLatexZone), ModContent.ItemType<MusicBoxCrystalZone>(), ModContent.TileType<MusicBoxCrystalZoneTile>());
        }

		public override void SetDefaults()
        {
            Item.DefaultToMusicBox(ModContent.TileType<MusicBoxCrystalZoneTile>(), 0);
            Item.value = Item.buyPrice(0, 5);
        }
	}
}
