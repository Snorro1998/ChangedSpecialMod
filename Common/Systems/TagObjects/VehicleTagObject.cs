using System.Collections.Generic;

namespace ChangedSpecialMod.Common.Systems.TagObjects
{
    public class VehicleTagObject : TagObject
    {
        public bool isShip = false;

        public bool hasWheels = false;
        public bool hasTyres = false;
        public bool hasTracks = false;

        public bool canFly = false;
        public bool canRide = false;

        public VehicleTagObject() : base() { tagCategory = "Vehicle"; }
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
                    isMetal = true,
                    canRide = true,
                    canPickColor = true
                },
                new VehicleTagObject()
                {
                    name = "bus",
                    isMetal = true,
                    canPickColor = true
                },
                new VehicleTagObject()
                {
                    name = "car",
                    isMetal = true,
                    canPickColor = true
                },
                new VehicleTagObject()
                {
                    name = "double-decker bus",
                    isMetal = true
                },
                new VehicleTagObject()
                {
                    name = "dump truck",
                    isMetal = true
                },
                new VehicleTagObject()
                {
                    name = "forklift",
                    isMetal = true
                },
                new VehicleTagObject()
                {
                    name = "moped",
                    isMetal = true,
                    canRide = true
                },
                new VehicleTagObject()
                {
                    name = "motorcycle",
                    isMetal = true,
                    canRide = true
                },
                new VehicleTagObject()
                {
                    name = "steam tractor",
                    isMetal = true
                },
                new VehicleTagObject()
                {
                    name = "tractor",
                    isMetal = true,
                    canPickColor = true
                },
                new VehicleTagObject()
                {
                    name = "truck",
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
                    isMetal = true,
                    canPickColor = true
                },
                new VehicleTagObject()
                {
                    name = "excavator",
                    isMetal = true,
                    canPickColor = true
                },
                new VehicleTagObject()
                {
                    name = "tank",
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
                    name = "airplane"
                },

                new VehicleTagObject()
                {
                    name = "blimp"
                },

                new VehicleTagObject()
                {
                    name = "biplane"
                },

                new VehicleTagObject()
                {
                    name = "hot air balloon"
                },

                new VehicleTagObject()
                {
                    name = "zeppelin"
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
