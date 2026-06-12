using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Utilities;
using ChangedSpecialMod.Utilities.UI.TransfurUI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Syringes
{
    public class SuperSyringe : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override bool? UseItem(Player player)
        {
            var changedPlayer = player.ChangedPlayer();

            if (changedPlayer.TransfurTypeCurrent != null)
                ChangedUtils.UntransfurPlayer(player.whoAmI);
            else if (player.whoAmI == Main.myPlayer)
                ModContent.GetInstance<TransfurUISystem>().ToggleUI(0);

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BlackSyringe>())
                .AddIngredient(ModContent.ItemType<WhiteSyringe>())
                .AddIngredient(ModContent.ItemType<SquidDogSyringe>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}