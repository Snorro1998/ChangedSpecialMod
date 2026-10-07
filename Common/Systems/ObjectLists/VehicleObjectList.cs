using ChangedSpecialMod.Utilities;
using System.Collections.Generic;
using Terraria;

namespace ChangedSpecialMod.Common.Systems.ObjectLists
{
    /*
    public class VehicleObject
    {
        public string name = "vehicle";
        public string category = "vehicle";

        public bool isShip = false;
        public bool isMetal = false;
        public bool isWood = false;

        public bool hasWheels = false;
        public bool hasTyres = false;
        public bool hasTracks = false;

        public bool canFly = false;
        public bool canRide = false;

        public string GetAdjective()
        {
            var optionsColors = new List<string>()
            {
                "blue",
                "green",
                "orange",
                "purple",
                "red",
                "yellow"
            };

            var options = new List<string>()
            {
                "big",
                "small",

                "fast",
                "slow",

                "antique",
                "brand new",
                "old"
            };
            if (isShip)
            {
                options.Add("leaky");
                options.Add("unsinkable");
            }
            if (isMetal)
                options.Add("rusty");
            if (isWood)
            {
                options.Add("creaky");
                options.Add("rotten");
            }

            var color = Utils.SelectRandom(Main.rand, optionsColors.ToArray());
            var other = Utils.SelectRandom(Main.rand, options.ToArray());

            return ChangedUtils.Choose(color, other, other);
        }
    }

    public class VehicleObjectList
    {
        public static List<VehicleObject> GetElements()
        {
            // vehicles with wheels, but no tyres
            var listWheels = new List<VehicleObject>()
            {
                new VehicleObject()
                {
                    name = "steam locomotive",
                    category = "train",
                    isMetal = true
                }
            };

            foreach (var vehicle in listWheels)
                vehicle.hasWheels = true;

            // vehicles with tyres
            var listTyres = new List<VehicleObject>()
            {
                new VehicleObject()
                {
                    name = "bike",
                    category = "bike",
                    isMetal = true,
                    canRide = true
                },
                new VehicleObject()
                {
                    name = "bus",
                    category = "bus",
                    isMetal = true
                },
                new VehicleObject()
                {
                    name = "car",
                    category = "car",
                    isMetal = true
                },
                new VehicleObject()
                {
                    name = "double-decker bus",
                    category = "bus",
                    isMetal = true
                },
                new VehicleObject()
                {
                    name = "dump truck",
                    category = "truck",
                    isMetal = true
                },
                new VehicleObject()
                {
                    name = "forklift",
                    category = "forklift",
                    isMetal = true
                },
                new VehicleObject()
                {
                    name = "moped",
                    category = "bike",
                    isMetal = true,
                    canRide = true
                },
                new VehicleObject()
                {
                    name = "motorcycle",
                    category = "motorcycle",
                    isMetal = true,
                    canRide = true
                },
                new VehicleObject()
                {
                    name = "steam tractor",
                    category = "tractor",
                    isMetal = true
                },
                new VehicleObject()
                {
                    name = "tractor",
                    category = "tractor",
                    isMetal = true
                },
                new VehicleObject()
                {
                    name = "truck",
                    category = "truck",
                    isMetal = true
                },
            };

            foreach (var vehicle in listTyres)
            {
                vehicle.hasWheels = true;
                vehicle.hasTyres = true;
            }

            // vehicles with tracks
            var listTracks = new List<VehicleObject>()
            {
                new VehicleObject()
                {
                    name = "bulldozer",
                    category = "bulldozer",
                    isMetal = true
                },
                new VehicleObject()
                {
                    name = "excavator",
                    category = "excavator",
                    isMetal = true
                },
                new VehicleObject()
                {
                    name = "tank",
                    category = "tank",
                    isMetal = true
                },
            };

            foreach (var vehicle in listTracks)
                vehicle.hasTracks = true;

            // boats
            var listBoats = new List<VehicleObject>()
            {
                new VehicleObject()
                {
                    name = "battleship",
                    isMetal = true,
                },
                new VehicleObject()
                {
                    name = "boat",
                    isMetal = true,
                    isWood = true,
                },
                new VehicleObject()
                {
                    name = "cruise ship",
                    isMetal = true
                },
                new VehicleObject()
                {
                    name = "jet ski",
                    isMetal = true
                },
                new VehicleObject()
                {
                    name = "paddle steamer",
                    isMetal = true
                },
                new VehicleObject()
                {
                    name = "pirate ship",
                    isMetal = true
                },
                new VehicleObject()
                {
                    name = "sailing ship",
                    isWood = true,
                },
                new VehicleObject()
                {
                    name = "ship",
                    isMetal = true,
                    isWood = true,
                },
                new VehicleObject()
                {
                    name = "steamboat",
                    isMetal = true
                },
                new VehicleObject()
                {
                    name = "submarine",
                    isMetal = true
                },
                new VehicleObject()
                {
                    name = "yacht",
                    isMetal = true
                }
            };

            foreach (var vehicle in listBoats)
                vehicle.isShip = true;

            // Air
            var listAir = new List<VehicleObject>()
            {
                new VehicleObject()
                {
                    name = "airplane",
                    category = "airplane"
                },

                new VehicleObject()
                {
                    name = "blimp",
                    category = "blimp"
                },

                new VehicleObject()
                {
                    name = "biplane",
                    category = "biplane"
                },

                new VehicleObject()
                {
                    name = "hot air balloon",
                    category = "hot air balloon"
                },

                new VehicleObject()
                {
                    name = "zeppelin",
                    category = "zeppelin"
                }
            };

            foreach (var vehicle in listAir)
                vehicle.canFly = true;

            var list = new List<VehicleObject>();
            list.AddRange(listWheels);
            list.AddRange(listTyres);
            list.AddRange(listTracks);
            list.AddRange(listBoats);
            list.AddRange(listAir);

            return list;
        }
    }
    */
}
