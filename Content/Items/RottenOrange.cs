using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items
{
    public class RottenOrange : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Orange>(), 1);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}