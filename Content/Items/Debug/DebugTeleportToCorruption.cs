using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ChangedSpecialMod.Utilities.ChangedUtils;

namespace ChangedSpecialMod.Content.Items.Debug
{
    public class DebugTeleportToCorruption : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override bool? UseItem(Player player)
        {
            var position = ChangedUtils.GetInfectionBlockPosition(BiomeType.Corrupt);
            if (position != new Vector2(-1, -1))
                player.Center = position;

            return true;
        }
    }
}