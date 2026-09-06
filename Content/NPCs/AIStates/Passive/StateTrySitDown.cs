using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace ChangedSpecialMod.Content.NPCs.AIStates.Passive
{
    public static class StateTrySitDown
    {
        public static void Update(NPC npc)
        {
            Point point = (npc.Bottom + Vector2.UnitY * -2f).ToTileCoordinates();
            bool isNotOccupied = WorldGen.InWorld(point.X, point.Y, 1);
            if (isNotOccupied)
            {
                // Check if another NPC is sitting here
                for (int npcIndex = 0; npcIndex < Main.maxNPCs; npcIndex++)
                {
                    var otherNpc = Main.npc[npcIndex];
                    if (otherNpc.active && otherNpc.aiStyle == NPCAIStyleID.Passive && otherNpc.townNPC && otherNpc.ai[0] == 5f && (otherNpc.Bottom + Vector2.UnitY * -2f).ToTileCoordinates() == point)
                    {
                        isNotOccupied = false;
                        break;
                    }
                }
                // Check if any player is sitting here
                for (int playerIndex = 0; playerIndex < Main.maxPlayers; playerIndex++)
                {
                    var player = Main.player[playerIndex];
                    if (player.active && player.sitting.isSitting && player.Center.ToTileCoordinates() == point)
                    {
                        isNotOccupied = false;
                        break;
                    }
                }
            }
            // Nobody is sitting here
            if (isNotOccupied)
            {
                Tile tile2 = Main.tile[point.X, point.Y];
                // Is there actually a chair here we can sit on
                isNotOccupied = TileID.Sets.CanBeSatOnForNPCs[tile2.TileType];
                // Disable sitting if actuated? Not sure
                if (isNotOccupied && tile2.TileType == TileID.Chairs && tile2.TileFrameY >= 1080 && tile2.TileFrameY <= 1098)
                {
                    isNotOccupied = false;
                }
                if (isNotOccupied)
                {
                    npc.ai[0] = 5f;
                    npc.ai[1] = 900 + Main.rand.Next(10800);
                    npc.SitDown(point, out var targetDirection, out var bottom);
                    npc.direction = targetDirection;
                    npc.Bottom = bottom;
                    npc.velocity = Vector2.Zero;
                    npc.localAI[3] = 0f;
                    npc.netUpdate = true;
                }
            }
        }
    }
}
