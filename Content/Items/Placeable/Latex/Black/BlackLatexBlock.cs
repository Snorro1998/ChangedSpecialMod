using ChangedSpecialMod.Content.Items.Placeable.Latex.White;
using ChangedSpecialMod.Content.Tiles.Latex.Black;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Latex.Black
{
    public class BlackLatexBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<WhiteLatexBlock>();
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<BlackLatexTile>());
            Item.width = 12;
            Item.height = 12;
        }

        public override void ExtractinatorUse(int extractinatorBlockType, ref int resultType, ref int resultStack)
        {
            resultType = ItemID.DirtBlock;
            resultStack = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe(10)
                .AddIngredient(ModContent.ItemType<BlackGoo>(), 1)
                .AddIngredient(ItemID.DirtBlock, 10)
                .AddTile(TileID.Solidifier)
                .Register();
        }
    }
}