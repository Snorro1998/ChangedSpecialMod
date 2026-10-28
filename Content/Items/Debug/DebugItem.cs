using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Common.Systems.ModSupport;
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

            //BossProgressionSystem.PrintDefeatedBosses();
            //var bookName = BookQuestSystem.GetBookName();
            //Main.NewText(bookName);
            return true;
        }
    }
}