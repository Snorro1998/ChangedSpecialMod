using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace ChangedSpecialMod.Common.Systems.TagObjects
{
    public class AnimalTagObject : TagObject
    {
        public bool regionForest = false;
        public bool regionDesert = false;
        public bool regionPolar = false;
        public bool regionRainforest = false;
        public bool regionSavannah = false;
        public bool regionWater = false;

        public bool isBird = false;
        public bool isDog = false;
        public bool isCat = false;
        public bool isBear = false;
        public bool isMonkey = false;
        public bool isFish = false;

        public bool hasPincers = false;
        public bool hasStinger = false;
        public bool hasTentacles = false;
        public bool hasFur = false;
        public bool hasEgg = false;

        public AnimalTagObject()
        {
            hasPersonality = true;
        }

        public string GetBodyPart()
        {
            var options = new List<string>();
            if (isBird)
            {
                options.Add("beaks");
                options.Add("feathers");
                options.Add("talons"); // only pred bird
            }

            if (isCat)
                options.Add("whiskers");
            if (isDog)
                options.Add("claws");
            if (isBear)
                options.Add("claws");
            if (isMonkey)
                options.Add("teeth");
            if (isFish)
                options.Add("fins");

            if (hasPincers)
                options.Add("pincers");
            if (hasStinger)
                options.Add("stingers");
            if (hasTentacles)
                options.Add("tentacles");
            if (hasFur)
                options.Add("fur");
            if (hasEgg)
                options.Add("eggs");

            if (!options.Any())
                return "";

            return Utils.SelectRandom(Main.rand, options.ToArray());
        }

        public string GetRegion()
        {
            var places = new List<string>() { };

            if (regionForest)
            {
                places.Add("forest");
            }
            if (regionDesert)
            {
                places.Add("desert");
                places.Add("sand dunes");
            }
            if (regionPolar)
            {
                places.Add("frozen north");
                places.Add("frozen south");
                places.Add("north");
                places.Add("tundra");
            }
            if (regionRainforest)
            {
                places.Add("jungle");
            }
            if (regionSavannah)
            {
                places.Add("savannah");
            }
            if (regionWater)
            {
                places.Add("deep");
                places.Add("ocean");
                places.Add("sea");
            }

            var place = Utils.SelectRandom(Main.rand, places.ToArray());
            return $"the {place}";
        }
    }

    public class AnimalTagObjectList : TagObjectList<AnimalTagObject>
    {
        public override void Init()
        {
            /*
            "capybara",
            "cat",
            "dog",
            "dolphin",
            "giraffe",
            "octopus",
            "squid",
            "zebra"
            */

            var animalsChanged = new List<AnimalTagObject>()
            {
                new AnimalTagObject()
                {
                    name = "squid dog",
                    isDog = true,
                    regionWater = true,
                    hasTentacles = true,
                    hasFur = true
                }
            };

            var animalsTentacles = new List<AnimalTagObject>()
            {
                new AnimalTagObject()
                {
                    name = "jellyfish",
                    nameGerman = "Qualle"
                },
                new AnimalTagObject()
                {
                    name = "squid",
                    nameGerman = "Tintenfisch"
                }
            };

            foreach (var item in animalsTentacles)
            {
                item.hasTentacles = true;
                item.regionWater = true;
            }

            var animalsRodents = new List<AnimalTagObject>()
            {
                new AnimalTagObject()
                {
                    name = "bunny",
                    nameGerman = "Hase",
                    regionForest = true,
                    hasFur = true
                },

                new AnimalTagObject()
                {
                    name = "squirrel",
                    nameGerman = "Eichhörnchen",
                    regionForest = true,
                    hasFur = true
                },
            };

            var animalsGrabbers = new List<AnimalTagObject>()
            {
                new AnimalTagObject()
                {
                    name = "crab",
                    nameGerman = "Krabbe",
                    regionWater = true,
                    hasPincers = true,
                },

                new AnimalTagObject()
                {
                    name = "lobster",
                    nameGerman = "Hummer",
                    regionWater = true,
                },

                new AnimalTagObject()
                {
                    name = "scorpion",
                    nameGerman = "Skorpion",
                    regionDesert = true,
                    hasStinger = true
                },
            };

            foreach (var item in animalsGrabbers)
            {
                item.hasPincers = true;
            }

            var animalsBird = new List<AnimalTagObject>()
            {
                new AnimalTagObject()
                {
                    name = "dodo",
                    nameGerman = "Dodo",
                    namePlural = "dodos",
                    regionRainforest = true,
                },

                new AnimalTagObject()
                {
                    name = "penguin",
                    nameGerman = "Pinguin",
                    regionPolar = true,
                },

                new AnimalTagObject()
                {
                    name = "vulture",
                    nameGerman = "Geier",
                    regionDesert = true,
                },
            };

            foreach (var animal in animalsBird)
            {
                animal.isBird = true;
                animal.hasEgg = true;
            }

            var animalsMonkey = new List<AnimalTagObject>()
            {
                new AnimalTagObject()
                {
                    name = "baboon",
                    nameGerman = "Pavian",
                    regionRainforest = true,
                },

                new AnimalTagObject()
                {
                    name = "gorilla",
                    nameGerman = "Gorilla",
                    regionRainforest = true,
                },

                new AnimalTagObject()
                {
                    name = "mandrill",
                    nameGerman = "Mandrill",
                    regionRainforest = true,
                },

                new AnimalTagObject()
                {
                    name = "monkey",
                    nameGerman = "Affe",
                    regionRainforest = true,
                },

                new AnimalTagObject()
                {
                    name = "orangutan",
                    nameGerman = "Orang-Utan",
                    regionRainforest = true,
                },
            };

            foreach (var animal in animalsMonkey)
            {
                animal.isMonkey = true;
                animal.hasFur = true;
            }

            var animalsDog = new List<AnimalTagObject>()
            {
                new AnimalTagObject()
                {
                    name = "fox",
                    nameGerman = "Fuchs",
                    regionForest = true,
                },

                new AnimalTagObject()
                {
                    name = "wolf",
                    nameGerman = "Wolf",
                    regionForest = true,
                }
            };

            foreach (var animal in animalsDog)
            {
                animal.isDog = true;
                animal.hasFur = true;
            }

            var animalsCat = new List<AnimalTagObject>()
            {
                new AnimalTagObject()
                {
                    name = "cheetah",
                    regionSavannah = true,
                },

                new AnimalTagObject()
                {
                    name = "leopard",
                    regionRainforest = true,
                },

                new AnimalTagObject()
                {
                    name = "lion",
                    regionSavannah = true,
                },
            };

            foreach (var animal in animalsCat)
            {
                animal.isCat = true;
                animal.hasFur = true;
            }

            var animalsBear = new List<AnimalTagObject>()
            {
                new AnimalTagObject()
                {
                    name = "panda",
                    regionRainforest = true,
                    hasFur = true
                },

                new AnimalTagObject()
                {
                    name = "polar bear",
                    regionPolar = true,
                    hasFur = true
                },
            };

            foreach (var animal in animalsBear)
                animal.isBear = true;

            var animalsFish = new List<AnimalTagObject>()
            {
                new AnimalTagObject()
                {
                    name = "orca",
                    regionWater = true
                },

                new AnimalTagObject()
                {
                    name = "shark",
                    regionWater = true,
                },

                new AnimalTagObject()
                {
                    name = "whale",
                    regionWater = true,
                },
            };

            foreach (var animal in animalsFish)
                animal.isFish = true;

            elements =
            [
                .. animalsChanged,
                .. animalsTentacles,
                .. animalsRodents,
                .. animalsGrabbers,
                .. animalsBird,
                .. animalsDog,
                .. animalsCat,
                .. animalsBear,
                .. animalsMonkey
            ];
        }
    }
}