using ChangedSpecialMod.Common.Systems;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
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
            var npcIDs = new List<int>
            {
                NPCID.Guide,
                NPCID.Merchant,
                NPCID.Nurse
            };

            var npcs = Main.npc.Where(x => x.active && npcIDs.Contains(x.type)).ToList();
            if (npcs.Any())
            {
                foreach (var npc in npcs)
                {
                    npc.ai[0] = 2f;
                    npc.ai[1] = 30 * Main.rand.Next(1, 4);
                    npc.netUpdate = true;
                }

            }

            /*
            Main.NewText(Language.GetTextValue("Mods.ChangedSpecialMod.Messages.BossProgressionReset"));
            DownedBossSystem.DownedWolfKing = false;
            DownedBossSystem.DownedWhiteTail = false;
            DownedBossSystem.DownedBehemoth = false;
            */
            return true;
        }
    }
}