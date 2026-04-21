using Terraria.ModLoader;

namespace ChangedSpecialMod
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class ChangedSpecialMod : Mod
	{
        internal static ChangedSpecialMod Instance => _Instance ??= ModContent.GetInstance<ChangedSpecialMod>();
        private static ChangedSpecialMod _Instance;

        public override void Unload()
        {
            _Instance = null;
            base.Unload();
        }
    }
}
