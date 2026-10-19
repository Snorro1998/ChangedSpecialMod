using ChangedSpecialMod.Content.Items.Weapons;
using log4net;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems.ModSupport
{
    public class ModSupportRedemption : ModSupportData
    {
        public ModSupportRedemption(Mod mod, string modName) : base(mod, modName) { }

        public override void Init()
        {
            bossProgressionList = new DownedBossObjectList()
            {
                mod = this.mod,
                modName = "Redemption",
                reflectionPath = "Redemption.Globals.RedeBossDowned",
                bosses = new List<DownedBossObject>()
                {
                    new DownedBossObject("Janitor", 11.1f, null, "downedJanitor"),
                    new DownedBossObject("IrradiatedBehemoth", 11.1f, null, "downedBehemoth"),
                    new DownedBossObject("PatientZero", 19.1f, null, "downedPZ")
                }
            };
        }

        public override void PostSetupContent(ILog log)
        {
            if (mod == null)
                return;

            base.PostSetupContent(log);
            SetupWeaponElementTypes();
        }

        public override void AddTownNPCs()
        {
            AddTownNPC("TBot", "RedemptionTBot");
        }

        public override void AddItemsFruit()
        {
            AddItemFruit("Avocado");
            AddItemFruit("Olives");
        }

        public void SetupWeaponElementTypes()
        {
            // Arcane
            mod.Call("addElementItem", 1, ModContent.ItemType<Encyclopedia>());
            mod.Call("addElementItem", 1, ModContent.ItemType<Literature>());
            mod.Call("addElementItem", 1, ModContent.ItemType<BookBarrage>());

            // Water
            mod.Call("addElementItem", 3, ModContent.ItemType<Mollash>());
            mod.Call("addElementItem", 3, ModContent.ItemType<Twintacle>());
            mod.Call("addElementItem", 3, ModContent.ItemType<Tentatrio>());
            mod.Call("addElementItem", 3, ModContent.ItemType<WateringCanWeapon>());

            // Ice
            mod.Call("addElementItem", 4, ModContent.ItemType<Dreadhorn>());
            mod.Call("addElementItem", 4, ModContent.ItemType<Bashskewer>());
            mod.Call("addElementItem", 4, ModContent.ItemType<Daemonspike>());

            // Blood
            mod.Call("addElementItem", 12, ModContent.ItemType<TigerPaw>());
            mod.Call("addElementItem", 12, ModContent.ItemType<Shredder>());
            mod.Call("addElementItem", 12, ModContent.ItemType<MegaMauler>());
        }
    }
}
