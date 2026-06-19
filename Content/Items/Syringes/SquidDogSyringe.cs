using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Syringes
{
    public class SquidDogSyringe : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 5, 0, 0);
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
                TransfurSystem.UntransfurPlayer(player.whoAmI);
            else
                TransfurSystem.SetTransfurFromNPCType(player.whoAmI, ModContent.NPCType<SquidDog>());
            return true;
        }
    }
}