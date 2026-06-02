using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Items.Licenses;
using NVorbis.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod
{
    public partial class ChangedSpecialMod
    {
        internal enum MessageType : byte
        {
            BlackLatexCubTownPetUnlock,
            WhiteLatexCubTownPetUnlock,
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();

            switch (msgType)
            {
                case MessageType.BlackLatexCubTownPetUnlock:
                    BlackLatexCubLicense.UnlockPet(ref TownPetSystem.boughtBlackLatexCubPet, ModContent.NPCType<Content.NPCs.TownPets.DarkLatexCubTownPet>(), ModContent.GetInstance<BlackLatexCubLicense>().GetLocalizationKey("UseBlackLatexCubLicense"));
                    break;
                case MessageType.WhiteLatexCubTownPetUnlock:
                    WhiteLatexCubLicense.UnlockPet(ref TownPetSystem.boughtWhiteLatexCubPet, ModContent.NPCType<Content.NPCs.TownPets.WhiteLatexCubTownPet>(), ModContent.GetInstance<WhiteLatexCubLicense>().GetLocalizationKey("UseWhiteLatexCubLicense"));
                    break;
                default:
                    Logger.WarnFormat("ExampleMod: Unknown Message type: {0}", msgType);
                    break;
            }
        }

    }
}
