using ChangedSpecialMod.Content.Items.Weapons;
using ChangedSpecialMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items
{
    public class SquidDogSyringe : ModItem
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

        public override bool? UseItem(Player player)
        {
            var changedPlayer = player.ChangedPlayer();
            changedPlayer.SetTransfurType(NPCs.GooType.None);
            return true;
        }
    }
}