using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable
{
    public class LabTileBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.Lab_TileTile>());
            Item.width = 12;
            Item.height = 12;
            Item.value = Item.buyPrice(0, 0, 0, 10);
        }
    }
}