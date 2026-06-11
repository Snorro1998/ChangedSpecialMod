using ChangedSpecialMod.Content.Items.Placeable.Crystals;
using ChangedSpecialMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Summons
{
    public class SummonWolfKing : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override bool? UseItem(Player player)
        {
            ChangedUtils.PlayerSpawnsWolfKing(player.whoAmI);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 5)
                .AddIngredient(ModContent.ItemType<CrystalRed>(), 2)
                .AddIngredient(ModContent.ItemType<CrystalGreen>(), 2)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}