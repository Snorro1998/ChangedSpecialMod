using Terraria;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Buffs
{
    public class FlyingDarkLatexMountBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(ModContent.MountType<Mounts.FlyingDarkLatexMount>(), player);
            player.buffTime[buffIndex] = 10;
        }
    }
}
