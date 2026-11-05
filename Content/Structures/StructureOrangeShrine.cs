using ChangedSpecialMod.Common.WorldGeneration.Passes;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ID;

namespace ChangedSpecialMod.Content.Structures
{
    public class StructureOrangeShrine : StructureBase
    {
        public StructureOrangeShrine(
        string name,
        Dictionary<Color, int> tileTypes = null,
        Dictionary<Color, int> wallTypes = null,
        Dictionary<Color, int> furnitureTypes = null)
        : base(name, tileTypes, wallTypes, furnitureTypes)
        {
            Name = name;
        }
    }
}
