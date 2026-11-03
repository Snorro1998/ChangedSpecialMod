using Terraria;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Buffs
{
    [LegacyName("FlyingDarkLatexMountBuff")]
    public class YufengCubMountBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(ModContent.MountType<Mounts.YufengCubMount>(), player);
            player.buffTime[buffIndex] = 10;
        }
    }
}
