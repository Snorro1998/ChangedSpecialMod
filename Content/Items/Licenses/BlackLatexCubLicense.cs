using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.NPCs.TownPets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Licenses
{
    public class BlackLatexCubLicense : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.UseSound = SoundID.Item92;
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = Item.CommonMaxStack;
            Item.SetShopValues(ItemRarityColor.Green2, Item.buyPrice(0, 5));
        }

        public override bool? UseItem(Player player)
        {
            int npcType = ModContent.NPCType<DarkLatexCubTownPet>();
            if (player.ItemAnimationJustStarted && (!TownPetSystem.boughtBlackLatexCubPet || NPC.AnyNPCs(npcType)))
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    UnlockPet(ref TownPetSystem.boughtBlackLatexCubPet, npcType, this.GetLocalizationKey("UseLicense"));
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// <br>The vanilla method NPC.UnlockOrExchangePet will not work for our modded Town Pets because the NetMessage only works with vanilla NPCs.</br>
        /// <br>This version uses a ModPacket for that instead.</br>
        /// </summary>
        /// <param name="petBoughtFlag">The bool that determines if the License has been used once. It doesn't really have anything to do with buying.</param>
        /// <param name="npcType">The NPC Type for the Town Pet.</param>
        /// <param name="textKeyForLicense">The localization path for when the License has been used for the first time.</param>
        public static void UnlockPet(ref bool petBoughtFlag, int npcType, string textKeyForLicense)
        {
            Color color = new(50, 255, 130);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                if (!petBoughtFlag || NPC.AnyNPCs(npcType))
                {
                    ModPacket packet = ModContent.GetInstance<ChangedSpecialMod>().GetPacket();
                    packet.Write((byte)ChangedSpecialMod.MessageType.BlackLatexCubTownPetUnlock);
                    packet.Send();
                }
            }
            else if (!petBoughtFlag)
            {
                petBoughtFlag = true;
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(textKeyForLicense), color);
                NetMessage.TrySendData(MessageID.WorldData);
            }
        }
    }
}
