using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items
{
    public class Paper : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BambooBlock, 1)
                .AddTile(TileID.WorkBenches)
                .Register();

            // Doesn't make much sense, but this way we can also make leather in crimson worlds
            var recipe = Recipe.Create(ItemID.Leather);
            recipe.AddIngredient(ItemID.Vertebrae, 5);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();

            recipe = Recipe.Create(ItemID.Book);
            recipe.AddIngredient(ItemID.Leather);
            recipe.AddIngredient(ModContent.ItemType<Paper>(), 3);
            recipe.AddIngredient(ItemID.BlackInk);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}