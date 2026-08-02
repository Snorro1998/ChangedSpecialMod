using ChangedSpecialMod.Content.Items.Placeable.Latex.Black;
using ChangedSpecialMod.Content.Tiles.Latex.White;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Latex.White
{
    public class WhiteLatexIce : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<BlackLatexIce>();
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<WhiteLatexIceTile>());
            Item.width = 12;
            Item.height = 12;
        }

        public override void ExtractinatorUse(int extractinatorBlockType, ref int resultType, ref int resultStack)
        {
            resultType = ItemID.IceBlock;
            resultStack = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe(10)
                .AddIngredient(ModContent.ItemType<WhiteGoo>(), 1)
                .AddIngredient(ItemID.IceBlock, 10)
                .AddTile(TileID.Solidifier)
                .Register();
        }
    }
}