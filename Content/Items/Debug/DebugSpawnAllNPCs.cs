using ChangedSpecialMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Debug
{
    public class DebugSpawnAllNPCs : ModItem
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
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return true;

            ChangedUtils.SpawnAllNPCs(player);
            return true;
        }
    }
}