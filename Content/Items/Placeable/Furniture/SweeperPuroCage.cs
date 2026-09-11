using ChangedSpecialMod.Content.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
    public class SweeperPuroCage : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.SquirrelCage);
            Item.createTile = ModContent.TileType<Tiles.Furniture.SweeperPuroCage>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Terrarium)
                .AddIngredient(ModContent.ItemType<SweeperPuroItem>())
                //.SortAfterFirstRecipesOf(ItemID.FrogCage) // places the recipe right after vanilla frog cage recipe.
                .Register();
        }
    }
}
