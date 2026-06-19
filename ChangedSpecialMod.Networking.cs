using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Items.Licenses;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
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
            RequestAllActiveTransfurs       // client -> server
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
                    NPCSpawnCheckSystem.WolfKingSpawnCheck(true, playerIndex);
                    break;
                }
                case MessageType.TeleportPlayer:
                    TeleportPlayer(reader);
                    break;
                case MessageType.TransfurPlayer:
                {
                    if (Main.netMode != NetmodeID.Server)
                        return;
                    
                    int npcType = reader.ReadInt32();

                    TransfurSystem.SetTransfurFromNPCType(whoAmI, npcType);
                    TransfurSystem.SyncTransfur(whoAmI);
                        /*
                    // Broadcast result to all clients
                    ModPacket packet = ModContent.GetInstance<ChangedSpecialMod>().GetPacket();
                    packet.Write((byte)MessageType.SyncTransfurPlayer);
                    packet.Write((byte)whoAmI);
                    packet.Write(npcType);
                    packet.Send();
                        */
                    break;
                }
                case MessageType.SyncTransfurPlayer:
                {
                    byte playerIndex = reader.ReadByte();
                    int npcType = reader.ReadInt32();

                    Main.player[playerIndex]
                        .GetModPlayer<ChangedSpecialModPlayer>()
                        .NpcType = npcType;

                    break;
                }
                case MessageType.RequestAllActiveTransfurs:
                {
                    if (Main.netMode != NetmodeID.Server)
                        return;

                    for (int i = 0; i < Main.player.Length; i++)
                    {
                        var player = Main.player[i];
                        if (player == null || !player.active)
                            continue;

                        var changedPlayer = player.ChangedPlayer();
                        if (changedPlayer == null || !changedPlayer.IsTransfurred)
                            continue;

                        TransfurSystem.SyncTransfur(i, whoAmI);
                    }

                    break;
                }
                default:
                    Logger.WarnFormat("ChangedSpecialMod: Unknown Message type: {0}", msgType);
                    break;
            }
        }
    }
}
