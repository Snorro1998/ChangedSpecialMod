using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Items.Licenses;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod
{
    public partial class ChangedSpecialMod
    {
        internal enum MessageType : byte
        {
            BlackLatexCubTownPetUnlock,     // client -> server
            WhiteLatexCubTownPetUnlock,     // client -> server
            PlaySwitchSound,                // server -> clients
            PlayerSpawnsWolfKing,           // client -> server
            TeleportPlayer,                 // client -> server
            TransfurPlayer,                 // client -> server
            SyncTransfurPlayer,             // server -> clients
            UntransfurPlayer                // client -> server
        }

        private void PlaySwitchSound(BinaryReader reader)
        {
            int i = reader.ReadInt16();
            int j = reader.ReadInt16();
            bool toggledOn = reader.ReadBoolean();

            SoundEngine.PlaySound(
                toggledOn
                    ? Sounds.SoundChime2
                    : Sounds.SoundBuzzer2,
                new Vector2(i * 16, j * 16)
            );
        }

        private void TeleportPlayer(BinaryReader reader)
        {
            int playerIndex = reader.ReadInt16();
            int xPos = reader.ReadInt16();
            int yPos = reader.ReadInt16();
            ChangedUtils.DoTeleportPlayer(playerIndex, xPos, yPos);
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();

            switch (msgType)
            {
                case MessageType.BlackLatexCubTownPetUnlock:
                    BlackLatexCubLicense.UnlockPet(ref TownPetSystem.boughtBlackLatexCubPet, ModContent.NPCType<Content.NPCs.TownPets.DarkLatexCubTownPet>(), ModContent.GetInstance<BlackLatexCubLicense>().GetLocalizationKey("UseLicense"));
                    break;
                case MessageType.WhiteLatexCubTownPetUnlock:
                    WhiteLatexCubLicense.UnlockPet(ref TownPetSystem.boughtWhiteLatexCubPet, ModContent.NPCType<Content.NPCs.TownPets.WhiteLatexCubTownPet>(), ModContent.GetInstance<WhiteLatexCubLicense>().GetLocalizationKey("UseLicense"));
                    break;
                case MessageType.PlaySwitchSound:
                    PlaySwitchSound(reader);
                    break;
                case MessageType.PlayerSpawnsWolfKing:
                {
                    int playerIndex = reader.ReadInt16();
                    ChangedUtils.WolfKingSpawnCheck(true, playerIndex);
                    break;
                }
                case MessageType.TeleportPlayer:
                    TeleportPlayer(reader);
                    break;
                case MessageType.TransfurPlayer:
                {
                    if (Main.netMode != NetmodeID.Server)
                        return;

                    byte playerIndex = reader.ReadByte();
                    int npcType = reader.ReadInt32();

                    ChangedUtils.SetTransfurFromNPCType(playerIndex, npcType);

                    // Broadcast result to all clients
                    ModPacket packet = ModContent.GetInstance<ChangedSpecialMod>().GetPacket();
                    packet.Write((byte)MessageType.SyncTransfurPlayer);
                    packet.Write(playerIndex);
                    packet.Write(npcType);
                    packet.Send();
                    break;
                }
                case MessageType.SyncTransfurPlayer:
                {
                    byte playerIndex = reader.ReadByte();
                    int npcType = reader.ReadInt32();

                    if (npcType == -1)
                        ChangedUtils.UntransfurPlayer(playerIndex);
                    else
                        ChangedUtils.SetTransfurFromNPCType(playerIndex, npcType);

                    break;
                }
                default:
                    Logger.WarnFormat("ChangedSpecialMod: Unknown Message type: {0}", msgType);
                    break;
            }
        }
    }
}
