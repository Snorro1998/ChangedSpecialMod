using Terraria;
using Terraria.ID;

namespace ChangedSpecialMod.Common.WorldGeneration.Passes
{
    public class PassPaintWholeWorldOrange : WorldGenerationPass
    {
        private byte color = PaintID.WhitePaint;
        
        // Update message if using a different color
        public PassPaintWholeWorldOrange(byte color)
        {
            this.color = color;
            PassName = "ChangedSeedOrange";
            GenerationMessage = "SpecialOrange";
        }

        public override void Generate()
        {
            for (int y = 0; y < Main.maxTilesY; y++)
            {
                for (int x = 0; x < Main.maxTilesX; x++)
                {
                    var tile = Main.tile[x, y];
                    if (tile != null)
                    {
                        if (tile.HasTile)
                            tile.TileColor = color;
                        if (tile.WallType != WallID.None)
                            tile.WallColor = color;
                    }
                }
            }
        }
    }
}
