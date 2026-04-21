using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChangedSpecialMod.Content.Tiles;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
    // The item used to place the statue.
    public class OrangeStatue : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ArmorStatue);
            Item.createTile = ModContent.TileType<Tiles.Furniture.OrangeStatue>();
            Item.placeStyle = 0;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Orange>()
                .AddIngredient(ItemID.StoneBlock, 50)
                .AddTile(TileID.HeavyWorkBench)
                .Register();
        }
    }
}
