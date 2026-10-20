using log4net;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems.ModSupport
{
    public class ModSupportPuroSummon : ModSupportData
    {
        private int puroSummonProjectileId = -1;

        public ModSupportPuroSummon(Mod mod, string modName) : base(mod, modName) { }

        public override void PostSetupContent(ILog log)
        {
            if (mod == null)
                return;

            if (mod.TryFind("PuroMinion", out ModProjectile modProjectile))
                puroSummonProjectileId = modProjectile.Type;
        }

        public bool PlayerHasPuroSummon(Player player)
        {
            if (puroSummonProjectileId == -1)
                return false;

            return Main.projectile.Any(x => x.active && x.type == puroSummonProjectileId && x.owner == player.whoAmI);
        }
    }
}
