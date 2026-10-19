using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Content.NPCs.TownPets;
using System;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems.ModSupport
{
    public class ModSupportCensus : ModSupportData
    {
        public ModSupportCensus(Mod mod, string modName) : base(mod, modName) { }

        private void AddTownNPC(int npcType, string npcName)
        {
            mod.Call("TownNPCCondition", npcType, Language.GetText($"Mods.ChangedSpecialMod.NPCs.{npcName}.Census.SpawnCondition"));
        }

        public override void PostSetupContent(log4net.ILog log)
        {
            var changedMod = ChangedSpecialMod.Instance;

            if (changedMod == null || mod == null)
                return;

            AddTownNPC(ModContent.NPCType<Puro>(), "Puro");
            AddTownNPC(ModContent.NPCType<Prototype>(), "Prototype");
            AddTownNPC(ModContent.NPCType<Scientist>(), "Scientist");
            AddTownNPC(ModContent.NPCType<Colin>(), "Colin");
            AddTownNPC(ModContent.NPCType<DarkLatexCubTownPet>(), "DarkLatexCubTownPet");
            AddTownNPC(ModContent.NPCType<WhiteLatexCubTownPet>(), "WhiteLatexCubTownPet");
        }
    }
}
