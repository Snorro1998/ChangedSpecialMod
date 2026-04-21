using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Buffs
{
    public class FlyingDarkLatexMountBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true; // The time remaining won't display on this buff
            Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(ModContent.MountType<Mounts.FlyingDarkLatexMount>(), player);
            player.buffTime[buffIndex] = 10; // reset buff time
        }
    }
}
