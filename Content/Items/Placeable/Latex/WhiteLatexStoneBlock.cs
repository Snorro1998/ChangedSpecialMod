using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Content.Tiles.Latex;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Latex
{
    public class WhiteLatexStoneBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<BlackLatexBlock>();
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<WhiteLatexStoneTile>());
            Item.width = 12;
            Item.height = 12;
        }

        public override void ExtractinatorUse(int extractinatorBlockType, ref int resultType, ref int resultStack)
        {
            resultType = ItemID.StoneBlock;
            resultStack = 1;
        }
    }
}