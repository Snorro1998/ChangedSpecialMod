using System.Collections.Generic;

namespace ChangedSpecialMod.Common.Systems.TagObjects
{
    public class VehicleTagObject : TagObject
    {
        public string category = "vehicle";

        public bool isShip = false;

        public bool hasWheels = false;
        public bool hasTyres = false;
        public bool hasTracks = false;

        public bool canFly = false;
        public bool canRide = false;
    }

    public class VehicleTagObjectList : TagObjectList<VehicleTagObject>
    {
        public override void Init()
        {
            // Crane

            // vehicles with wheels, but no tyres
            var listWheels = new List<VehicleTagObject>()
            {
                new VehicleTagObject()
                {
                    name = "steam locomotive",
                    category = "train",
                    isMetal = true
                }
            };

            foreach (var vehicle in listWheels)
                vehicle.hasWheels = true;

            // vehicles with tyres
            var listTyres = new List<VehicleTagObject>()
            {
                new VehicleTagObject()
                {
                    name = "bike",
                    category = "bike",
                    isMetal = true,
                    canRide = true,
                    canPickColor = true
                },
                new VehicleTagObject()
                {
                    name = "bus",
                    category = "bus",
                    isMetal = true,
                    canPickColor = true
                },
                new VehicleTagObject()
                {
                    name = "car",
                    category = "car",
                    isMetal = true,
                    canPickColor = true
                },
                new VehicleTagObject()
                {
                    name = "double-decker bus",
                    category = "bus",
                    isMetal = true
                },
                new VehicleTagObject()
                {
                    name = "dump truck",
                    category = "truck",
                    isMetal = true
                },
                new VehicleTagObject()
                {
                    name = "forklift",
                    category = "forklift",
                    isMetal = true
                },
                new VehicleTagObject()
                {
                    name = "moped",
                    category = "bike",
                    isMetal = true,
                    canRide = true
                },
                new VehicleTagObject()
                {
                    name = "motorcycle",
                    category = "motorcycle",
                    isMetal = true,
                    canRide = true
                },
                new VehicleTagObject()
                {
                    name = "steam tractor",
                    category = "tractor",
                    isMetal = true
                },
                new VehicleTagObject()
                {
                    name = "tractor",
                    category = "tractor",
                    isMetal = true,
                    canPickColor = true
                },
                new VehicleTagObject()
                {
                    name = "truck",
                    category = "truck",
                    isMetal = true,
                    canPickColor = true
                },
            };

            foreach (var vehicle in listTyres)
            {
                vehicle.hasWheels = true;
                vehicle.hasTyres = true;
            }

            // vehicles with tracks
            var listTracks = new List<VehicleTagObject>()
            {
                new VehicleTagObject()
                {
                    name = "bulldozer",
                    category = "bulldozer",
                    isMetal = true,
                    canPickColor = true
                },
                new VehicleTagObject()
                {
                    name = "excavator",
                    category = "excavator",
                    isMetal = true,
                    canPickColor = true
                },
                new VehicleTagObject()
                {
                    name = "tank",
                    category = "tank",
                    isMetal = true
                },
            };

            foreach (var vehicle in listTracks)
                vehicle.hasTracks = true;

            // boats
            var listBoats = new List<VehicleTagObject>()
            {
                new VehicleTagObject()
                {
                    name = "battleship",
                    isMetal = true,
                },
                new VehicleTagObject()
                {
                    name = "boat",
                    isMetal = true,
                    isWood = true,
                    canPickColor = true
                },
                new VehicleTagObject()
                {
                    name = "cruise ship",
                    isMetal = true
                },
                new VehicleTagObject()
                {
                    name = "jet ski",
                    isMetal = true
                },
                new VehicleTagObject()
                {
                    name = "paddle steamer",
                    isMetal = true
                },
                new VehicleTagObject()
                {
                    name = "pirate ship",
                    isMetal = true
                },
                new VehicleTagObject()
                {
                    name = "sailing ship",
                    isWood = true,
                },
                new VehicleTagObject()
                {
                    name = "ship",
                    isMetal = true,
                    isWood = true,
                },
                new VehicleTagObject()
                {
                    name = "steamboat",
                    isMetal = true,
                    canPickColor = true
                },
                new VehicleTagObject()
                {
                    name = "submarine",
                    isMetal = true,
                    canPickColor = true
                },
                new VehicleTagObject()
                {
                    name = "yacht",
                    isMetal = true
                }
            };

            foreach (var vehicle in listBoats)
                vehicle.isShip = true;

            // Air
            var listAir = new List<VehicleTagObject>()
            {
                new VehicleTagObject()
                {
                    name = "airplane",
                    category = "airplane"
                },

                new VehicleTagObject()
                {
                    name = "blimp",
                    category = "blimp"
                },

                new VehicleTagObject()
                {
                    name = "biplane",
                    category = "biplane"
                },

                new VehicleTagObject()
                {
                    name = "hot air balloon",
                    category = "hot air balloon"
                },

                new VehicleTagObject()
                {
                    name = "zeppelin",
                    category = "zeppelin"
                }
            };

            foreach (var vehicle in listAir)
                vehicle.canFly = true;

            var list = new List<VehicleTagObject>();
            list.AddRange(listWheels);
            list.AddRange(listTyres);
            list.AddRange(listTracks);
            list.AddRange(listBoats);
            list.AddRange(listAir);
            elements = list;

            elements =
            [
                .. listWheels,
                .. listTyres,
                .. listTracks,
                .. listBoats,
                .. listAir
            ];
        }
    }
}
