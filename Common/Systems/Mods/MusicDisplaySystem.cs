using ChangedSpecialMod.Assets;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems.Mods
{
    public static class MusicDisplaySystem
    {
        private static ChangedSpecialMod changedMod;
        private static Mod targetMod;

        private static void AddTrack(Mod hostMod, LocalizedText modName, LocalizedText author, string musicPath, string musicName, Color[] colors)
        {
            LocalizedText displayName = Language.GetText($"Mods.ChangedSpecialMod.MusicDisplay.Music.{musicName}");
            targetMod.Call("AddMusic", (short)MusicLoader.GetMusicSlot(hostMod, musicPath), displayName, author, modName, null, colors);
        }

        public static void Setup(ChangedSpecialMod _changedMod, Mod _targetMod)
        {
            changedMod = _changedMod;
            targetMod = _targetMod;

            if (changedMod == null || targetMod == null)
                return;

            LocalizedText modName = Language.GetText("Mods.ChangedSpecialMod.MusicDisplay.ModName");
            LocalizedText author = Language.GetText("Mods.ChangedSpecialMod.MusicDisplay.Authors.Shizi");
            LocalizedText authorHaise = Language.GetText("Mods.ChangedSpecialMod.MusicDisplay.Authors.Haise");

            // Is displayed like this. The number is the parameter index
            // 4: Current Music
            // 1: Song name
            // 2: Artist
            // 3: Mod name
            var defaultColors = new Color[] { Color.White, new Color(230, 230, 230), new Color(180, 180, 180), new Color(120, 120, 120) };
            var partyColors = new Color[] { Color.Red, Color.Green, Color.Blue, Color.Yellow };

            // Normal
            AddTrack(changedMod, modName, author, Sounds.MusicBlackLatexZone, "MusicBlackLatexZone", defaultColors);
            AddTrack(changedMod, modName, author, Sounds.MusicBlackLatexZone2, "MusicBlackLatexZone2", defaultColors);
            AddTrack(changedMod, modName, author, Sounds.MusicCrystalZone, "MusicCrystalZone", defaultColors);
            AddTrack(changedMod, modName, author, Sounds.MusicWhiteLatexZone, "MusicWhiteLatexZone", defaultColors);
            AddTrack(changedMod, modName, author, Sounds.MusicLibrary, "MusicLibrary", defaultColors);
            AddTrack(changedMod, modName, author, Sounds.MusicLabSlow, "MusicLabSlow", defaultColors);
            AddTrack(changedMod, modName, author, Sounds.MusicLab, "MusicLab", defaultColors);
            AddTrack(changedMod, modName, author, Sounds.MusicVents, "MusicVents", defaultColors);
            AddTrack(changedMod, modName, author, Sounds.MusicPuro, "MusicPuro", defaultColors);
            AddTrack(changedMod, modName, author, Sounds.MusicGreenhouse, "MusicGreenhouse", defaultColors);

            // Party
            AddTrack(changedMod, modName, author, Sounds.MusicHappyBirthday, "MusicHappyBirthday", partyColors);
            AddTrack(changedMod, modName, author, Sounds.MusicPuroDance, "MusicPuroDance", partyColors);

            // Drunk
            AddTrack(changedMod, modName, author, Sounds.MusicRun, "MusicRun", defaultColors);

            // Bosses
            AddTrack(changedMod, modName, author, Sounds.MusicWhiteTailChase2, "MusicWhiteTailChase2", defaultColors);
            AddTrack(changedMod, modName, author, Sounds.MusicWolfKing, "MusicWolfKing", defaultColors);
            AddTrack(changedMod, modName, author, Sounds.MusicBehemoth, "MusicBehemoth", defaultColors);
            AddTrack(changedMod, modName, author, Sounds.MusicShark, "MusicShark", defaultColors);
            AddTrack(changedMod, modName, author, Sounds.MusicSquidDog, "MusicSquidDog", defaultColors);

            // Upcoming, unknown song names
            AddTrack(changedMod, modName, author, Sounds.Music30, "Music30", defaultColors);

        }
    }
}
