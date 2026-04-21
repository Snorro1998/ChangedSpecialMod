using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems
{
    public class AudioSystem : ModSystem
    {
        public static string CurrentMusicBlackLatexZone = Sounds.MusicBlackLatexZone;
        public static string CurrentMusicWhiteLatexZone = Sounds.MusicWhiteLatexZone;
        public static string CurrentMusicCityRuins = Sounds.MusicLabSlow;
        public static string CurrentMusicParty = Sounds.MusicPuroDance;

        public override void PostSetupContent()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, Sounds.MusicCrystalZone), ModContent.ItemType<MusicBoxCrystalZone>(), ModContent.TileType<MusicBoxCrystalZoneTile>());
                MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, Sounds.MusicLabSlow), ModContent.ItemType<MusicBoxLabSlow>(), ModContent.TileType<MusicBoxLabSlowTile>());
                MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, Sounds.MusicLab), ModContent.ItemType<MusicBoxLab>(), ModContent.TileType<MusicBoxLabTile>());
                MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, Sounds.MusicPuro), ModContent.ItemType<MusicBoxPuro>(), ModContent.TileType<MusicBoxPuroTile>());
                MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, Sounds.MusicPuroDance), ModContent.ItemType<MusicBoxPuroDance>(), ModContent.TileType<MusicBoxPuroDanceTile>());
                MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, Sounds.MusicHappyBirthday), ModContent.ItemType<MusicBoxHappyBirthday>(), ModContent.TileType<MusicBoxHappyBirthdayTile>());
                MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, Sounds.MusicLibrary), ModContent.ItemType<MusicBoxLibrary>(), ModContent.TileType<MusicBoxLibraryTile>());
                MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, Sounds.MusicWhiteLatexZone), ModContent.ItemType<MusicBoxWhiteLatexZone>(), ModContent.TileType<MusicBoxWhiteLatexZoneTile>());
                MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, Sounds.MusicBlackLatexZone2), ModContent.ItemType<MusicBoxBlackLatexZone2>(), ModContent.TileType<MusicBoxBlackLatexZone2Tile>());
                MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, Sounds.MusicBlackLatexZone), ModContent.ItemType<MusicBoxBlackLatexZone1>(), ModContent.TileType<MusicBoxBlackLatexZone1Tile>());
                MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, Sounds.MusicGreenhouse), ModContent.ItemType<MusicBoxGreenhouse>(), ModContent.TileType<MusicBoxGreenhouseTile>());
                MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, Sounds.MusicVents), ModContent.ItemType<MusicBoxVents>(), ModContent.TileType<MusicBoxVentsTile>());

                MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, Sounds.MusicWhiteTailChase2), ModContent.ItemType<MusicBoxWhiteTail>(), ModContent.TileType<MusicBoxWhiteTailTile>());
                MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, Sounds.MusicWolfKing), ModContent.ItemType<MusicBoxWolfKing>(), ModContent.TileType<MusicBoxWolfKingTile>());
                MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, Sounds.MusicBehemoth), ModContent.ItemType<MusicBoxBehemoth>(), ModContent.TileType<MusicBoxBehemothTile>());
            }
        }

        /// <summary>
        /// Randomize the current music for the biome that was entered by the player
        /// </summary>
        /// <param name="player"></param>
        /// <param name="gooType"></param>
        public static void RandomizeMusic(Player player, GooType gooType)
        {
            var nearPuro = ChangedUtils.IsPlayerNearPuro(player);
            var nearPrototype = ChangedUtils.IsPlayerNearPrototype(player);

            // Black latex zone
            if (gooType == GooType.Black)
            {
                var musicOptions = new List<string>()
                {
                    Sounds.MusicBlackLatexZone,
                    Sounds.MusicBlackLatexZone2,
                    Sounds.MusicCrystalZone,
                };
                if (player.townNPCs > 0)
                    musicOptions.Add(Sounds.MusicLibrary);
                if (nearPuro)
                    musicOptions.Add(Sounds.MusicPuro);
                if (nearPrototype)
                    musicOptions.Add(Sounds.MusicGreenhouse);
                var choices = musicOptions.ToArray();
                CurrentMusicBlackLatexZone = Utils.SelectRandom(Main.rand, choices);
            }

            // White latex zone
            else if (gooType == GooType.White)
            {
                var musicOptions = new List<string>()
                {
                    Sounds.MusicWhiteLatexZone,
                    Sounds.MusicLabSlow,
                    //Sounds.MusicGreenhouse
                };
                if (player.townNPCs > 0)
                    musicOptions.Add(Sounds.MusicLibrary);
                if (nearPuro)
                    musicOptions.Add(Sounds.MusicPuro);
                if (nearPrototype)
                    musicOptions.Add(Sounds.MusicGreenhouse);
                var choices = musicOptions.ToArray();
                CurrentMusicWhiteLatexZone = Utils.SelectRandom(Main.rand, choices);
            }

            // City ruins biome
            else
            {
                var musicOptions = new List<string>()
                {
                    Sounds.MusicBlackLatexZone,
                    Sounds.MusicBlackLatexZone2,
                    Sounds.MusicCrystalZone,
                    Sounds.MusicWhiteLatexZone,
                    Sounds.MusicLabSlow
                };
                if (player.townNPCs > 0)
                    musicOptions.Add(Sounds.MusicLibrary);
                if (nearPuro)
                    musicOptions.Add(Sounds.MusicPuro);
                if (nearPrototype)
                    musicOptions.Add(Sounds.MusicGreenhouse);
                var choices = musicOptions.ToArray();
                CurrentMusicCityRuins = Utils.SelectRandom(Main.rand, choices);
            }
            // Always pick a random party song
            var MusicParty = new string[]
            {
                Sounds.MusicHappyBirthday,
                Sounds.MusicPuroDance,
            };
            CurrentMusicParty = Utils.SelectRandom(Main.rand, MusicParty);
        }

        public static string GetMusic(Player player, GooType gooType)
        {
            // This also causes the party music to play if you are near the old man at the dungeon
            if (player != null && player.townNPCs > 0 && BirthdayParty.PartyIsUp)
                return CurrentMusicParty;
            if (!Main.IsItDay())
                return Sounds.MusicVents;
            if (gooType == GooType.Black)
                return CurrentMusicBlackLatexZone;
            if (gooType == GooType.White)
                return CurrentMusicWhiteLatexZone;
            return CurrentMusicCityRuins;
        }

        public static void PlaySoundWithProbability(SoundStyle sound, Vector2 position, int probability = 1)
        {
            if (Main.rand.NextBool(probability))
                SoundEngine.PlaySound(sound, position);
        }
    }
}
