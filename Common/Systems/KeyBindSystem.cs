using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems
{
    public class KeybindSystem : ModSystem
    {
        public static ModKeybind TransfurEvolveKeybind { get; private set; }
        public static ModKeybind TransfurDevolveKeybind { get; private set; }
        public static ModKeybind TransfurAttackKeybind { get; private set; }

        public override void Load()
        {
            TransfurEvolveKeybind = KeybindLoader.RegisterKeybind(Mod, "TransfurEvolve", "P");
            TransfurDevolveKeybind = KeybindLoader.RegisterKeybind(Mod, "TransfurDevolve", "O");
            TransfurAttackKeybind = KeybindLoader.RegisterKeybind(Mod, "TransfurAttack", "Mouse2");//Mouse3
        }

        public override void Unload()
        {
            TransfurEvolveKeybind = null;
            TransfurDevolveKeybind = null;
            TransfurAttackKeybind = null;
        }
    }
}
