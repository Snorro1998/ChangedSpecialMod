using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Content.NPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Scenes
{
    public class DarkLatexCubOfDoomScene : BaseMusicSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override int NPCType => ModContent.NPCType<DarkLatexCubOfDoom>();

        public override string SceneMusic => Sounds.MusicRun;
    }
}
