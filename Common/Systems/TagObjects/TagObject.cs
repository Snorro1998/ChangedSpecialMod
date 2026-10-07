using ChangedSpecialMod.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;

namespace ChangedSpecialMod.Common.Systems.TagObjects
{
    public class TagObject
    {
        public string name = "object";
        public string namePlural = null;

        public string nameGerman = null;
        public string nameFrench = null;
        public string nameSpanish = null;
        public string nameItalian = null;
        public string nameBrazilianPortuguese = null;
        public string nameRussian = null;
        public string nameChinese = null;
        
        public string color = "none";
        public bool canPickColor = false;

        public bool isMetal = false;
        public bool isWood = false;

        public bool isFoodIngredient = false;

        public bool runsOnSteam = false;
        public bool runsOnDiesel = false;

        public bool hasPersonality = false;

        public string tagCategory = null;

        public string GetName()
        {
            if (tagCategory == null)
                return name;

            var nname = ChangedUtils.ToPascalCase(name);
            var path = $"Mods.ChangedSpecialMod.Tags.{tagCategory}.{nname}";
            if (Language.Exists(path))
                return Language.GetTextValue(path);

            return name;
        }

        public string GetPlural()
        {
            // Check if there is an entry in localization files
            // If not, continue with the old logic after this block
            if (tagCategory != null)
            {
                var nname = ChangedUtils.ToPascalCase(name) + "s";
                var path = $"Mods.ChangedSpecialMod.Tags.{tagCategory}.{nname}";
                if (Language.Exists(path))
                    return Language.GetTextValue(path);
            }

            // Explicit override
            if (!string.IsNullOrEmpty(namePlural))
                return namePlural;

            var word = name;

            // Common irregular plurals
            switch (word.ToLowerInvariant())
            {
                case "man": return "men";
                case "woman": return "women";
                case "child": return "children";
                case "person": return "people";
                case "mouse": return "mice";
                case "goose": return "geese";
                case "tooth": return "teeth";
                case "foot": return "feet";
                case "ox": return "oxen";
                case "die": return "dice";
                case "sheep": return word;
                case "fish": return word;
                case "deer": return word;
                case "moose": return word;
            }

            // Words ending in consonant + "y":
            // city -> cities, party -> parties
            if (word.Length > 1 &&
                word.EndsWith("y", StringComparison.OrdinalIgnoreCase) &&
                !"aeiou".Contains(char.ToLowerInvariant(word[^2])))
            {
                return word[..^1] + "ies";
            }

            // Words ending in vowel + "y":
            // boy -> boys, key -> keys
            if (word.EndsWith("y", StringComparison.OrdinalIgnoreCase))
                return word + "s";

            // Words ending in sibilant sounds
            // bus -> buses, box -> boxes, church -> churches, dish -> dishes
            if (word.EndsWith("s", StringComparison.OrdinalIgnoreCase) ||
                word.EndsWith("x", StringComparison.OrdinalIgnoreCase) ||
                word.EndsWith("z", StringComparison.OrdinalIgnoreCase) ||
                word.EndsWith("ch", StringComparison.OrdinalIgnoreCase) ||
                word.EndsWith("sh", StringComparison.OrdinalIgnoreCase))
            {
                return word + "es";
            }

            // Words ending in consonant + "o":
            // potato -> potatoes, tomato -> tomatoes
            // (There are many exceptions, so keep this list conservative.)
            if (word.EndsWith("o", StringComparison.OrdinalIgnoreCase))
            {
                var exceptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                    {
                        "photo", "piano", "halo", "memo", "solo", "zero", "kimono"
                    };

                return exceptions.Contains(word)
                    ? word + "s"
                    : word + "es";
            }

            // Words ending in "f" / "fe"
            // knife -> knives, wife -> wives, leaf -> leaves
            var fToVes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "calf", "half", "knife", "leaf", "life",
                    "loaf", "self", "shelf", "thief", "wife",
                    "wolf", "elf"
                };

            if (fToVes.Contains(word))
            {
                if (word.EndsWith("fe", StringComparison.OrdinalIgnoreCase))
                    return word[..^2] + "ves";

                return word[..^1] + "ves";
            }

            // Words ending in "fe" that commonly just take "s"
            // safe -> safes, belief -> beliefs, etc.
            if (word.EndsWith("fe", StringComparison.OrdinalIgnoreCase))
                return word + "s";

            // Most other words
            return word + "s";
        }

        public string GetArticle()
        {
            if (string.IsNullOrWhiteSpace(name))
                return "a";

            var word = name.Trim().ToLowerInvariant();

            // Words that begin with a vowel sound despite starting with a consonant.
            if (word.StartsWith("honest") ||
                word.StartsWith("honor") ||
                word.StartsWith("hour") ||
                word.StartsWith("heir"))
            {
                return "an";
            }

            // Words that begin with a consonant sound despite starting with a vowel.
            if (word.StartsWith("university") ||
                word.StartsWith("unicorn") ||
                word.StartsWith("unique") ||
                word.StartsWith("user") ||
                word.StartsWith("usual") ||
                word.StartsWith("one") ||
                word.StartsWith("once"))
            {
                return "a";
            }

            // Standard vowel sound cases.
            if ("aeiou".Contains(word[0]))
                return "an";

            return "a";
        }

        public static T PickRandom<T>(List<T> input)
        {
            return Utils.SelectRandom(Main.rand, input.ToArray());
        }

        public string PickColor()
        {
            return PickRandom(adjColors.Where(x => x != color).ToList());
        }

        public virtual string GetAdjective()
        {
            var culture = Language.ActiveCulture.Name;

            // If german, only pick colors for now
            if (culture == "de-DE")
            {
                return PickRandom(new List<string>()
                {
                    "roter",
                    "blauer",
                    "grüner",
                    "gelber",
                    "violetter"
                });
            }

            var list = new List<string>() { };

            // Material
            if (isMetal)
                list.AddRange(adjMetal);
            if (isWood)
                list.AddRange(adjWood);

            if (isFoodIngredient)
                list.AddRange(adjFoodIngredient);

            if (runsOnSteam)
            {
                list.Add("filthy");
                list.Add("grimy");
            }
            if (runsOnDiesel)
            {
                list.Add("filthy");
                list.Add("grimy");
            }

            if (hasPersonality)
            {
                list.Add("adorable");
                list.Add("crazy");
                list.Add("cute");
                list.Add("dangerous");
                list.Add("evil");
                list.Add("lazy");
                list.Add("naughty");
                list.Add("violent");
            }

            // Remove duplicates
            list = list.Distinct().ToList();

            if (list.Count == 0)
            {
                list.Add("big");
                list.Add("small");
            }

            var adjective = PickRandom(list);
            if (!canPickColor)
                return adjective;
            var color = PickColor();
            return ChangedUtils.Choose(adjective, color);
        }

        public List<string> adjMetal = new List<string>()
        {
            //"corroded",
            "rusty"
        };

        public List<string> adjWood = new List<string>()
        {
            "creaking",
            "rotten"
        };
        /*
        public List<string> adjTaste = new List<string>()
        {
            "delicious",
            "disgusting"
        };
        */

        public List<string> adjFoodIngredient = new List<string>()
        {
            //"award-winning",
            //"bitter",
            "expired",
            "fresh",
            "juicy",
            "moldy",
            "rotten",
            "spicy",
            "stale"
            //"sour",
            //"sweet"
            //"mouth-watering"
        };

        public List<string> adjColors = new List<string>()
        {
            "black",
            "blue",
            "green",
            "red",
            "white"
        };
    }
}
