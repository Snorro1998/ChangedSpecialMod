using System.Collections.Generic;

namespace ChangedSpecialMod.Common.Systems.TagObjects
{
    public class PlanetTagObject : TagObject
    {
        public bool isGasPlanet = false;
        public bool hasMoons = false;

        public PlanetTagObject() : base() { tagCategory = "Planet"; }
    }

    public class PlanetTagObjectList : TagObjectList<PlanetTagObject>
    {
        public override void Init()
        {
            // Fruit
            elements = new List<PlanetTagObject>()
            {
                 new PlanetTagObject() { name = "Jupiter", isGasPlanet = true, hasMoons = true },
                 new PlanetTagObject() { name = "Mars", hasMoons = true },
                 new PlanetTagObject() { name = "Mercury" },
                 new PlanetTagObject() { name = "Neptune", isGasPlanet = true, hasMoons = true },
                 new PlanetTagObject() { name = "Pluto" },
                 new PlanetTagObject() { name = "Saturn", isGasPlanet = true, hasMoons = true },
                 new PlanetTagObject() { name = "Uranus", isGasPlanet = true, hasMoons = true },
                 new PlanetTagObject() { name = "Venus" },
                 //new PlanetTagObject() { name = "the Moon" },
            };
        }
    }
}
