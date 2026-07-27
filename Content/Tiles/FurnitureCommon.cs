using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles
{
    internal static class FurnitureCommon
    {
        /// <summary>
        /// Extension which initializes a ModTile to be a hanging lantern.
        /// </summary>
        /// <param name="mt">The ModTile which is being initialized.</param>
        /// <param name="itemDropID">The ID of the item this tile drops when broken.</param>
        /// <param name="lavaImmune">Whether this tile is supposed to be immune to lava. Defaults to false.</param>
        /// <param name="autoMapEntry">Whether this tile is supposed to use normal map entries. Defaults to true.</param>
        internal static void SetUpLantern(this ModTile mt, int itemDropID, bool lavaImmune = false, bool autoMapEntry = true)
        {
            mt.RegisterItemDrop(itemDropID);

            Main.tileLighted[mt.Type] = true;
            Main.tileFrameImportant[mt.Type] = true;
            Main.tileLavaDeath[mt.Type] = !lavaImmune;
            Main.tileWaterDeath[mt.Type] = false;
            TileID.Sets.DisableSmartCursor[mt.Type] = true;
            TileID.Sets.MultiTileSway[mt.Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.LavaDeath = !lavaImmune;
            TileObjectData.newTile.LavaPlacement = lavaImmune ? LiquidPlacement.Allowed : LiquidPlacement.NotAllowed;
            TileObjectData.newTile.StyleLineSkip = 2;
            TileObjectData.newTile.DrawYOffset = -2;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.Platform, TileObjectData.newTile.Width, 0);
            TileObjectData.newAlternate.DrawYOffset = -10;
            TileObjectData.addAlternate(0);
            TileObjectData.addTile(mt.Type);

            // All hanging lanterns count as light sources.
            mt.AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            if (autoMapEntry)
                mt.AddMapEntry(new Color(251, 235, 127), Language.GetText("MapObject.Lantern"));

            mt.AdjTiles = new int[] { TileID.HangingLanterns };
        }

        public static void LightHitWire(int type, int i, int j, int tileX, int tileY)
        {
            int x = i - Main.tile[i, j].TileFrameX / 18 % tileX;
            int y = j - Main.tile[i, j].TileFrameY / 18 % tileY;
            int tileXX18 = 18 * tileX;
            for (int l = x; l < x + tileX; l++)
            {
                for (int m = y; m < y + tileY; m++)
                {
                    if (Main.tile[l, m].HasTile && Main.tile[l, m].TileType == type)
                    {
                        if (Main.tile[l, m].TileFrameX < tileXX18)
                            Main.tile[l, m].TileFrameX += (short)(tileXX18);
                        else
                            Main.tile[l, m].TileFrameX -= (short)(tileXX18);
                    }
                }
            }

            if (Wiring.running)
            {
                for (int k = 0; k < tileX; k++)
                {
                    for (int l = 0; l < tileY; l++)
                        Wiring.SkipWire(x + k, y + l);
                }
            }

            if (Main.netMode != NetmodeID.SinglePlayer)
                NetMessage.SendTileSquare(-1, x, y, tileX, tileY);
        }
    }
}
