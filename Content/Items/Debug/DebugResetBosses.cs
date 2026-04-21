using ChangedSpecialMod.Common.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Debug
{
    public class DebugResetBosses : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool? UseItem(Player player)
        {
            Main.NewText("Changed boss progression reset!");
            DownedBossSystem.DownedWolfKing = false;
            DownedBossSystem.DownedWhiteTail = false;
            DownedBossSystem.DownedBehemoth = false;
            return true;
        }
    }
}