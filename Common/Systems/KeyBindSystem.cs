using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems
{
    public class KeybindSystem : ModSystem
    {
        public static ModKeybind TransfurAttackKeybind { get; private set; }

        public override void Load()
        {
            TransfurAttackKeybind = KeybindLoader.RegisterKeybind(Mod, "TransfurAttack", "Mouse2");
        }

        public override void Unload()
        {
            TransfurAttackKeybind = null;
        }
    }
}
