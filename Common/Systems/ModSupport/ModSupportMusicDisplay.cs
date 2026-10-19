using ChangedSpecialMod.Assets;
using log4net;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems.ModSupport
{
    public class ModSupportMusicDisplay : ModSupportData
    {
        public ModSupportMusicDisplay(Mod mod, string modName) : base(mod, modName) { }

        private void AddTrack(Mod hostMod, LocalizedText modName, LocalizedText author, string musicPath, string musicName, Color[] colors)
        {
            LocalizedText displayName = Language.GetText($"Mods.ChangedSpecialMod.MusicDisplay.Music.{musicName}");
            mod.Call("AddMusic", (short)MusicLoader.GetMusicSlot(hostMod, musicPath), displayName, author, modName, null, colors);
        }

        public override void PostSetupContent(ILog log)
        {
            var changedMod = ChangedSpecialMod.Instance;

            if (changedMod == null || mod == null)
                return;

            LocalizedText modName = Language.GetText("Mods.ChangedSpecialMod.MusicDisplay.ModName");
            LocalizedText author = Language.GetText("Mods.ChangedSpecialMod.MusicDisplay.Authors.Shizi");

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
            AddTrack(changedMod, modName, author, Sounds.MusicWhiteTailChase1, "MusicWhiteTailChase1", defaultColors);
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
