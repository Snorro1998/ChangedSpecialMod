using ChangedSpecialMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items
{
    public class WhiteSyringe : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.useStyle = ItemUseStyleID.HiddenAnimation;
            Item.useTime = 30;
            Item.useAnimation = 30;
        }

        public override bool? UseItem(Player player)
        {
            var changedPlayer = player.ChangedPlayer();
            changedPlayer.SetOrRemoveTransfur(TransfurType.WhiteGoop);
            return true;
        }
    }
}