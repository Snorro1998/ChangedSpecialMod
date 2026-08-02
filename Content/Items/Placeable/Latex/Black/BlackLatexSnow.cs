using ChangedSpecialMod.Content.Items.Placeable.Latex.White;
using ChangedSpecialMod.Content.Tiles.Latex.Black;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Latex.Black
{
    public class BlackLatexSnow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<WhiteLatexSnow>();
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<BlackLatexSnowTile>());
            Item.width = 12;
            Item.height = 12;
        }

        public override void ExtractinatorUse(int extractinatorBlockType, ref int resultType, ref int resultStack)
        {
            resultType = ItemID.SnowBlock;
            resultStack = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe(10)
                .AddIngredient(ModContent.ItemType<BlackGoo>(), 1)
                .AddIngredient(ItemID.SnowBlock, 10)
                .AddTile(TileID.Solidifier)
                .Register();
        }
    }
}