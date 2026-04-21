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

        public static bool HasBlackCubTF = false;
        public static bool HasBlackAdultTF = false;
        public static bool HasBlackFastTF = false;
        public static bool HasBlackStrongTF = false;

        public static bool HasWhiteCubTF = false;
        public static bool HasWhiteAdultTF = false;
        public static bool HasWhiteFastTF = false;

        public override void ClearWorld()
        {
            DownedWhiteTail = false;
            DownedBehemoth = false;
            DownedWolfKing = false;
            DownedShark = false;

            HasBlackCubTF = false;
            HasBlackAdultTF = false;
            HasBlackFastTF = false;
            HasBlackStrongTF = false;

            HasWhiteCubTF = false;
            HasWhiteAdultTF = false;
            HasWhiteFastTF = false;
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

            // Black TFs
            if (HasBlackCubTF)
                tag[nameof(HasBlackCubTF)] = true;
            if (HasBlackAdultTF)
                tag[nameof(HasBlackAdultTF)] = true;
            if (HasBlackFastTF)
                tag[nameof(HasBlackFastTF)] = true;
            if (HasBlackStrongTF)
                tag[nameof(HasBlackStrongTF)] = true;

            // White TFs
            if (HasWhiteCubTF)
                tag[nameof(HasWhiteCubTF)] = true;
            if (HasWhiteAdultTF)
                tag[nameof(HasWhiteAdultTF)] = true;
            if (HasWhiteFastTF)
                tag[nameof(HasWhiteFastTF)] = true;

        }

        public override void LoadWorldData(TagCompound tag)
        {
            DownedWhiteTail = tag.ContainsKey(nameof(DownedWhiteTail));
            DownedBehemoth = tag.ContainsKey(nameof(DownedBehemoth));
            DownedWolfKing = tag.ContainsKey(nameof(DownedWolfKing));
            DownedShark = tag.ContainsKey(nameof(DownedShark));

            HasBlackCubTF = tag.ContainsKey(nameof(HasBlackCubTF));
            HasBlackAdultTF = tag.ContainsKey(nameof(HasBlackAdultTF));
            HasBlackFastTF = tag.ContainsKey(nameof(HasBlackFastTF));
            HasBlackStrongTF = tag.ContainsKey(nameof(HasBlackStrongTF));

            HasWhiteCubTF = tag.ContainsKey(nameof(HasWhiteCubTF));
            HasWhiteAdultTF = tag.ContainsKey(nameof(HasWhiteAdultTF));
            HasWhiteFastTF = tag.ContainsKey(nameof(HasWhiteFastTF));
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.WriteFlags(DownedWhiteTail, DownedBehemoth, DownedWolfKing, DownedShark);
            writer.WriteFlags(HasBlackCubTF, HasBlackAdultTF, HasBlackFastTF, HasBlackStrongTF);
            writer.WriteFlags(HasWhiteCubTF, HasWhiteAdultTF, HasWhiteFastTF);
        }

        public override void NetReceive(BinaryReader reader)
        {
            reader.ReadFlags(out DownedWhiteTail, out DownedBehemoth, out DownedWolfKing, out DownedShark);
            reader.ReadFlags(out HasBlackCubTF, out HasBlackAdultTF, out HasBlackFastTF, out HasBlackStrongTF);
            reader.ReadFlags(out HasWhiteCubTF, out HasWhiteAdultTF, out HasWhiteFastTF);
        }
    }
}