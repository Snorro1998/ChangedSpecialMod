using log4net;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems.ModSupport
{
    public class ModSupportFargosMutant : ModSupportData
    {
        public ModSupportFargosMutant(Mod mod, string modName) : base(mod, modName) { }

        private static readonly Dictionary<string, float> BossChecklistProgressionValues = new()
        {
            { "WhiteTail", 1.5f },
            { "WolfKing", 2.5f },
            { "Behemoth", 5.5f }
            //{ "Shark", ??? },
            //{ "SquidDog", ??? },
        };

        private void AddToMutantShop(string bossName, string summonItemName, Func<bool> downed, int price)
        {
            BossChecklistProgressionValues.TryGetValue(bossName, out float order);
            mod.Call("AddSummon", order, "ChangedSpecialMod", summonItemName, downed, price);
        }

        public override void PostSetupContent(ILog log)
        {
            if (mod is null)
                return;

            AddToMutantShop("WhiteTail", "SummonWhiteTail", () => DownedBossSystem.DownedWhiteTail, Item.buyPrice(gold: 2));
            AddToMutantShop("WolfKing", "SummonWolfKing", () => DownedBossSystem.DownedWolfKing, Item.buyPrice(gold: 2));
            AddToMutantShop("Behemoth", "SummonBehemoth", () => DownedBossSystem.DownedBehemoth, Item.buyPrice(gold: 2));
        }
    }
}
