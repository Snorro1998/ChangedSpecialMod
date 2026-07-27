using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ChangedSpecialMod.Utilities.ChangedUtils;

namespace ChangedSpecialMod.Content.Items.Debug
{
    public class DebugTeleportToHallow : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if (Main.netMode == NetmodeID.SinglePlayer || (Main.netMode == NetmodeID.MultiplayerClient && player == Main.LocalPlayer))
                    SoundEngine.PlaySound(SoundID.Unlock, player.Center);
                player.inventory[player.selectedItem].SetDefaults(ModContent.ItemType<DebugTeleportToLatex>());
                return true;
            }

            var position = ChangedUtils.GetInfectionBlockPosition(BiomeType.Hallow);
            if (position != new Vector2(-1, -1))
                player.Center = position;

            return true;
        }
    }
}