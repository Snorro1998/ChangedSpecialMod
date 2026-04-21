using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Content.NPCs;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Scenes
{
    public class BehemothHandMusicScene : BaseMusicSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override int NPCType => ModContent.NPCType<BehemothHand>();

        public override string SceneMusic => Sounds.MusicBehemoth;
    }
}
