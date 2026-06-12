using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Content.NPCs;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Scenes
{
    public class WhiteTailMusicScene : BaseMusicSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override int NPCType => ModContent.NPCType<WhiteTail>();

        public override string SceneMusic => Sounds.MusicWhiteTailChase2;
    }
}
