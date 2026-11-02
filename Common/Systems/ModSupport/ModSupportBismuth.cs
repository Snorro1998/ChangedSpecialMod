using ChangedSpecialMod.Content.Items.Weapons;
using log4net;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems.ModSupport
{
    public class ModSupportBismuth : ModSupportData
    {
        public ModSupportBismuth(Mod mod, string modName) : base(mod, modName) { }

        public override void AddTiles()
        {
            AddTileType("SwampMud", true);
        }
    }
}
