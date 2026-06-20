using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

namespace ChangedSpecialMod.Content.Items.Syringes
{
    public class GermanShepherdSyringe : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var descriptionTip = tooltips.FirstOrDefault(x => x.Name == "Tooltip0");
            if (descriptionTip != null)
                descriptionTip.Text = TransfurSystem.GetDescription(ModContent.NPCType<GermanShepherd>(), true);
        }

        public override bool? UseItem(Player player)
        {
            var changedPlayer = player.ChangedPlayer();
            if (changedPlayer.TransfurTypeCurrent != null)
                TransfurSystem.UntransfurPlayer(player.whoAmI);
            else
                TransfurSystem.SetTransfurFromNPCType(player.whoAmI, ModContent.NPCType<GermanShepherd>());
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Syringe>())
                .AddIngredient(ItemID.DirtBlock, 5)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}