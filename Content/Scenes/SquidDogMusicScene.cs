using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Content.NPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Scenes
{
    public class SquidDogMusicScene : BaseMusicSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override int NPCType => ModContent.NPCType<SquidDogBoss>();

        public override string SceneMusic => Sounds.MusicSquidDog;
    }
}
