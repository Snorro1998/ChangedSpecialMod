using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ChangedSpecialMod.Common.Configs
{
    public sealed class ChangedSpecialModClientConfig : ModConfig
    {
        public static ChangedSpecialModClientConfig Instance;
        public override void OnLoaded()
        {
            Instance = this;
        }

        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        public bool TransfurSound;
    }
}
