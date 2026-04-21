using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items
{
    public class DiscoCrystal : ModItem
    {
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.ChangedSpecialMod.hjson file.

        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CrystalRed>(), 3)
                .AddIngredient(ModContent.ItemType<CrystalGreen>(), 3)
                .AddIngredient(ModContent.ItemType<CrystalWhite>(), 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}