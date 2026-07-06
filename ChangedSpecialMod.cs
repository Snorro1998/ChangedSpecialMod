using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ChangedSpecialMod
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public partial class ChangedSpecialMod : Mod
	{
        internal static ChangedSpecialMod Instance => _Instance ??= ModContent.GetInstance<ChangedSpecialMod>();
        private static ChangedSpecialMod _Instance;

        public override void Unload()
        {
            _Instance = null;
            base.Unload();
        }

        public override object Call(params object[] args)
        {
            if (args ==  null || args.Length == 0)
                return null;

            int argsLength = args.Length;

            try
            {
                if (args[0] is not string)
                    return new ArgumentNullException("ERROR: First argument must be a string.");

                string methodName = args[0] as string;
                methodName = methodName.ToLower();

                switch (methodName)
                {
                    default: break;
                    case "getbossdowned":
                    case "bossdowned":
                        if (argsLength < 2)
                            return new ArgumentNullException("ERROR: Must specify a boss or event name as a string.");
                        if (args[1] is not string)
                            return new ArgumentException("ERROR: The argument to \"Downed\" must be a string.");
                        return GetBossDowned(args[1].ToString());
                    case "orang":
                    case "orange":
                    case "oranges":
                        ChangedUtils.SpawnOrangesOnAllPlayers();
                        return true;
                    case "spawnall":
                    case "spawnallnpcs":
                        ChangedUtils.SpawnAllNPCs(null);
                        return true;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Call Error: {e.StackTrace} {e.Message}");
            }

            return new ArgumentException("ERROR: Invalid method name.");
        }

        private bool GetBossDowned(string bossName)
        {
            bossName = bossName.ToLower();

            switch(bossName)
            {
                case "whitetail":
                    return DownedBossSystem.DownedWhiteTail;
                case "wolfking":
                    return DownedBossSystem.DownedWolfKing;
                case "behemoth":
                    return DownedBossSystem.DownedBehemoth;
            }

            return false;
        }
    }
}
