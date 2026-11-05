using ChangedSpecialMod.Common.WorldGeneration.Passes;
using ChangedSpecialMod.Content.Walls;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Structures
{
    public class LabRoomStructure1 : StructureBase
    {
        public LabRoomStructure1(
        string name,
        Dictionary<Color, int> tileTypes = null,
        Dictionary<Color, int> wallTypes = null,
        Dictionary<Color, int> furnitureTypes = null)
        : base(name, tileTypes, wallTypes, furnitureTypes)
        {
            Name = name;
            wallTypes = new Dictionary<Color, int>
            {
                { Colors2.Green, ModContent.WallType<LabWall>() }
            };
        }
    }
}
