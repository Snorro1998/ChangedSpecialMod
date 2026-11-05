using ChangedSpecialMod.Common.WorldGeneration.Passes;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ID;

namespace ChangedSpecialMod.Content.Structures
{
    public class StructureOrangeShrineMarble : StructureBase
    {
        public StructureOrangeShrineMarble(
        string name,
        Dictionary<Color, int> tileTypes = null,
        Dictionary<Color, int> wallTypes = null,
        Dictionary<Color, int> furnitureTypes = null)
        : base(name, tileTypes, wallTypes, furnitureTypes)
        {
            Name = name;

            tileTypes = new Dictionary<Color, int>
            {
                { Colors2.Red, TileID.MarbleBlock },
                { Colors2.Blue, TileID.MarbleColumn },
            };

            wallTypes = new Dictionary<Color, int>
            {
                { Colors2.Green, WallID.MarbleBlock }
            };
        }
    }
}
