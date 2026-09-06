using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace ChangedSpecialMod.Content.NPCs.AIStates.Passive
{
    public static class StateSitting
    {
        public static void Update(NPC npc)
        {
            Point coords = (npc.Bottom + Vector2.UnitY * -2f).ToTileCoordinates();
            Tile tile = Main.tile[coords.X, coords.Y];
            if (!TileID.Sets.CanBeSatOnForNPCs[tile.TileType])
            {
                npc.ai[1] = 0f;
            }
            else
            {
                Main.sittingManager.AddNPC(npc.whoAmI, coords);
            }
        }
    }
}
