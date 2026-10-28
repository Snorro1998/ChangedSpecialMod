using Microsoft.Win32;
using ReLogic.OS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

// This system can read steam's libraryfolders.vdf file on windows, which contain the ids of installed games
// It does not collect or send this info in any way. It only compares the ids to the hardcoded dictionary.
// If a match is found, a dialogue keyword is added in GetChatKeywords like this: HasGame{GameName}
// Puro and Dr K can then talk about these games, such as "Would you like to play 'game' together?

// It is currently unused, because I am not sure if this is against TModloader's TOS and it would creep players out.
// It was never present in any version of the mod and it was only used as something fun to test.
// It still works if you call LoadGames during setup

namespace ChangedSpecialMod.Common.Systems
{
    public static class GameSupportSystem
    {
        // List of recognized games
        public static List<string> games = new List<string>();

        // lookup dictionary for installed games
        public static Dictionary<int, string> steamGames = new Dictionary<int, string>()
        {
            // Factory games
            { 427520, "Factorio" },
            { 526870, "Satisfactory" },

            // Farming games
            { 413150, "Stardew Valley" },
            { 447020, "Farming Simulator" }, //17
            { 787860, "Farming Simulator" }, //19
            { 1248130, "Farming Simulator" }, //22
            { 2142790, "Fields of Mistria" },
            { 2300320, "Farming Simulator" }, //25

            // Horror
            { 252330, "Slender Man" },  //Slender: The Arrival, Slender: the eigth pages
            { 319510, "Five Nights at Freddy's" }, //1
            { 332800, "Five Nights at Freddy's" }, //2
            { 354140, "Five Nights at Freddy's" }, //3
            { 388090, "Five Nights at Freddy's" }, //4
            { 506610, "Five Nights at Freddy's" }, //: Sister Location
            { 521890, "Hello Neighbor" },
            { 1721470, "Poppy Playtime" },
            { 2232840, "Garten of Banban" },
            { 2504480, "Indigo Park" }, // Chapter 1
            { 3722130, "Indigo Park" }, // Chapter 2

            // Party
            { 331670, "Jackbox" }, // Jackbox 1..12
            { 397460, "Jackbox" },
            { 434170, "Jackbox" },
            { 610180, "Jackbox" },
            { 774461, "Jackbox" },
            { 945360, "Among Us" },
            { 1005300, "Jackbox" },
            { 1211630, "Jackbox" },
            { 1552350, "Jackbox" },
            { 1850960, "Jackbox" },
            { 2216830, "Jackbox" },
            { 3364070, "Jackbox" },
            { 4199960, "Jackbox" },

            // Furry
            { 570840, "Nekojishi" },
            { 587620, "Okami" },        //okami hd
            { 814540, "Changed" },
            { 2154650, "Pawperty Damage" },
            { 3237970, "Bubsy 4D" },
            { 4032170, "Bubsy 4D" }, // Demo
            { 4576510, "Casualties Unknown" }, // Demo
            { 4576490, "Casualties Unknown" },
            { 4739660, "Drag'n Wash" }, 

            // Misc
            { 391540, "Undertale" },
            { 1454400, "Cookie Clicker" },
            { 1671210, "Deltarune" },
            { 3164500, "Schedule I" }
        };

        public static void LoadGames()
        {
            try
            {
                // Only do this on Windows, because it doesn't work elsewhere
                if (Platform.Current.Type != PlatformType.Windows)
                    return;

                string vdfPath = FindLibraryFoldersVdfWindows();

                if (CanReadFile(vdfPath))
                {
                    games = new List<string>();
                    var ids = GetSteamAppIds(vdfPath);

                    foreach (var id in ids)
                    {
                        if (steamGames.ContainsKey(id))
                        {
                            games.Add(steamGames[id]);
                        }
                    }
                }
            }
            catch
            {
                // ignore
            }
        }

        public static int[] GetSteamAppIds(string vdfPath)
        {
            try
            {
                string content = File.ReadAllText(vdfPath);
                var appIds = new HashSet<int>();

                var appsBlocks = Regex.Matches(
                    content,
                    @"""apps""\s*\{([^}]*)\}"
                );

                foreach (Match block in appsBlocks)
                {
                    var entries = Regex.Matches(
                        block.Groups[1].Value,
                        @"""(\d+)""\s*""[^""]*"""
                    );

                    foreach (Match entry in entries)
                    {
                        if (int.TryParse(entry.Groups[1].Value, out int appId))
                        {
                            appIds.Add(appId);
                        }
                    }
                }

                return appIds.ToArray();
            }
            catch
            {
                return null;
            }
        }

        public static bool CanReadFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                return false;

            try
            {
                using var stream = new FileStream(
                    path,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite | FileShare.Delete
                );

                return true;
            }
            catch (IOException)
            {
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
        }

        public static string FindLibraryFoldersVdfWindows()
        {
            var steamPaths = new[]
            {
                (Registry.CurrentUser, @"Software\Valve\Steam", "SteamPath"),
                (Registry.LocalMachine, @"SOFTWARE\WOW6432Node\Valve\Steam", "InstallPath"),
                (Registry.LocalMachine, @"SOFTWARE\Valve\Steam", "InstallPath")
            };

            foreach (var (hive, keyPath, valueName) in steamPaths)
            {
                try
                {
                    using var key = hive.OpenSubKey(keyPath);
                    var value = key?.GetValue(valueName) as string;

                    if (string.IsNullOrWhiteSpace(value))
                        continue;

                    var steamPath = value.Replace('/', Path.DirectorySeparatorChar);
                    var vdfPath = Path.Combine(
                        steamPath,
                        "steamapps",
                        "libraryfolders.vdf");

                    if (File.Exists(vdfPath))
                        return vdfPath;
                }
                catch (IOException)
                {

                }
                catch (UnauthorizedAccessException)
                {

                }
            }

            var defaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Steam", "steamapps", "libraryfolders.vdf");
            return File.Exists(defaultPath) ? defaultPath : null;
        }
    }
}
