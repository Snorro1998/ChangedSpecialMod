using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Common.Systems.ModSupport;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Debug
{
    public class DebugItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 1;
            Item.useTime = 1;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override bool? UseItem(Player player)
        {
            {
                var dict = ModSupportSystem.NPCIdentifiers;
                var keys = dict.Keys.ToList();

                var str = "";
                foreach (var key in keys)
                {
                    var val = dict[key];
                    str += $"{key}, {val} | ";
                }

                Main.NewText(str);
            }


            var npc = Main.npc.FirstOrDefault(x => x.active);
            if (npc != null)
            {
                var changedNPC = npc.Changed();
                var keywords = changedNPC.GetChatKeyWords();

                var keys = keywords.Keys.Where(x => x.Contains("Name")).ToList();
                var str = "";
                foreach (var key in keys)
                {
                    str += $"{key}, ";
                }
                Main.NewText(str);
            }

            var anubisNPC = Main.npc.FirstOrDefault(x => x.type == 753);
            if (anubisNPC != null)
            {
                Main.NewText($"{anubisNPC.FullName}, {anubisNPC.active}");
            }



            {
                var activeNPCS = Main.npc.Where(x => x.active).ToList();
                var str = "";
                foreach (var activeNPC in activeNPCS)
                {
                    str += $"{activeNPC.type}, {activeNPC.TypeName} | ";
                }
                Main.NewText(str);
            }


            /*
            var tgMod = ModSupportSystem.externalMods.Where(x => x.modName == "terraguardians").FirstOrDefault() as ModSupportTerraGuardian;
            if (tgMod != null)
            {
                var tgs = tgMod.GetAllTerraGuardians();
                var str = "";
                foreach (var t in tgs)
                {
                    str += $"{t.name}, {t.hasBeenMet}, {t.inPlayerParty} | ";
                }
                Main.NewText(str);
            }
            */

            return true;
        }
    }
}