using System.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ChangedSpecialMod.Common.Systems
{
    public class TownPetSystem : ModSystem
    {
        public static bool boughtBlackLatexCubPet = false;
        public static bool boughtWhiteLatexCubPet = false;

        public override void ClearWorld()
        {
            boughtBlackLatexCubPet = false;
            boughtWhiteLatexCubPet = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (boughtBlackLatexCubPet)
            {
                tag["boughtBlackLatexCubPet"] = true;
            }
            if (boughtWhiteLatexCubPet)
            {
                tag["boughtWhiteLatexCubPet"] = true;
            }
        }

        public override void LoadWorldData(TagCompound tag)
        {
            boughtBlackLatexCubPet = tag.ContainsKey("boughtBlackLatexCubPet");
            boughtWhiteLatexCubPet = tag.ContainsKey("boughtWhiteLatexCubPet");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.WriteFlags(boughtBlackLatexCubPet);
            writer.WriteFlags(boughtWhiteLatexCubPet);
        }

        public override void NetReceive(BinaryReader reader)
        {
            reader.ReadFlags(out boughtBlackLatexCubPet);
            reader.ReadFlags(out boughtWhiteLatexCubPet);
        }
    }
}
