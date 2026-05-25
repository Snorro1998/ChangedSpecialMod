using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Content.NPCs;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Scenes
{
    public class Experiment009MusicScene : BaseMusicSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override int NPCType => ModContent.NPCType<Experiment009>();
        public override string SceneMusic => Sounds.Music30;
        //public override string SceneMusic => Sounds.MusicMeaninglessStrafe;
    }
}
