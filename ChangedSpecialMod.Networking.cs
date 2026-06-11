using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Items.Licenses;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod
{
    public partial class ChangedSpecialMod
    {
        internal enum MessageType : byte
        {
            BlackLatexCubTownPetUnlock,
            WhiteLatexCubTownPetUnlock,
            PlaySwitchSound,
            SetPlayerTransfur,
            PlayerSpawnsWolfKing,
            TeleportPlayer
        }

        private void PlaySwitchSound(BinaryReader reader, int whoAmI)
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

        private void SetPlayerTransfur(BinaryReader reader, int whoAmI)
        {

        }

        private void TeleportPlayer(BinaryReader reader, int whoAmI)
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
                    PlaySwitchSound(reader, whoAmI);
                    break;
                case MessageType.SetPlayerTransfur:
                    SetPlayerTransfur(reader, whoAmI);
                    break;
                case MessageType.PlayerSpawnsWolfKing:
                    int playerIndex = reader.ReadInt16();
                    ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"Server, playerspawnswolf {playerIndex.ToString()}"), Color.Red);
                    ChangedUtils.WolfKingSpawnCheck(true, playerIndex);
                    break;
                case MessageType.TeleportPlayer:
                    TeleportPlayer(reader, whoAmI);
                    break;
                default:
                    Logger.WarnFormat("ExampleMod: Unknown Message type: {0}", msgType);
                    break;
            }
        }
    }
}
