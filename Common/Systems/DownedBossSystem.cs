using System.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ChangedSpecialMod.Common.Systems
{
    public class DownedBossSystem : ModSystem
    {
        public static bool DownedWhiteTail = false;
        public static bool DownedBehemoth = false;
        public static bool DownedWolfKing = false;
        public static bool DownedShark = false;
        public static bool ColinCanSpawn = false;
        public static int ExploredLabVersion = -1;

        public override void ClearWorld()
        {
            DownedWhiteTail = false;
            DownedBehemoth = false;
            DownedWolfKing = false;
            DownedShark = false;
            ColinCanSpawn = false;
            ExploredLabVersion = -1;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            // Bosses killed
            if (DownedWhiteTail)
                tag[nameof(DownedWhiteTail)] = true;
            if (DownedBehemoth)
                tag[nameof(DownedBehemoth)] = true;
            if (DownedWolfKing)
                tag[nameof(DownedWolfKing)] = true;
            if (DownedShark)
                tag[nameof(DownedShark)] = true;
            // Others
            if (ColinCanSpawn)
                tag[nameof(ColinCanSpawn)] = true;
            tag[nameof(ExploredLabVersion)] = ExploredLabVersion;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            DownedWhiteTail = tag.ContainsKey(nameof(DownedWhiteTail));
            DownedBehemoth = tag.ContainsKey(nameof(DownedBehemoth));
            DownedWolfKing = tag.ContainsKey(nameof(DownedWolfKing));
            DownedShark = tag.ContainsKey(nameof(DownedShark));
            ColinCanSpawn = tag.ContainsKey(nameof(ColinCanSpawn));
            tag.TryGet(nameof(ExploredLabVersion), out ExploredLabVersion);
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.WriteFlags(DownedWhiteTail, DownedBehemoth, DownedWolfKing, DownedShark);
            writer.WriteFlags(ColinCanSpawn);
            writer.Write7BitEncodedInt(ExploredLabVersion);
        }

        public override void NetReceive(BinaryReader reader)
        {
            reader.ReadFlags(out DownedWhiteTail, out DownedBehemoth, out DownedWolfKing, out DownedShark);
            reader.ReadFlags(out ColinCanSpawn);
            ExploredLabVersion = reader.Read7BitEncodedInt();
        }

        public static void SetExploredLab()
        {
            var versionString = ChangedSpecialMod.Instance.Version.ToString().Replace(".", "");
            var success = int.TryParse(versionString, out int versionNumber);
            if (success)
            {
                if (versionNumber < 1000)
                    versionNumber *= 10;
                ExploredLabVersion = versionNumber;
            }
        }
    }
}