using ChangedSpecialMod.Content.Items.Food;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
    // The item used to place the statue.
    public class SquidDogStatue : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ArmorStatue);
            Item.createTile = ModContent.TileType<Tiles.Furniture.SquidDogStatue>();
            Item.placeStyle = 0;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.StoneBlock, 50)
                .AddTile(TileID.HeavyWorkBench)
                .Register();
        }
    }
}
