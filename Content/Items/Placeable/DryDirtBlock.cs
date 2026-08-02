using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable
{
    public class DryDirtBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.DryDirt>());
            Item.width = 12;
            Item.height = 12;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DirtBlock, 1)
                .AddTile(TileID.Furnaces)
                .Register();

            var recipe = Recipe.Create(ItemID.DirtBlock);
            recipe.AddIngredient(ModContent.ItemType<DryDirtBlock>(), 1);
            recipe.AddCondition(Condition.NearWater);
            recipe.Register();
        }
    }
}