using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems
{
    public class NPCSpawnCheckSystem : ModSystem
    {
        public override void PreUpdateWorld()
        {
            DoBossSpawnChecks();
        }

        public static void DoBossSpawnChecks()
        {
            WhiteTailSpawnCheck();
            WolfKingSpawnCheck();
            BehemothSpawnCheck();
        }

        private static void WhiteTailSpawnCheck()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient || DownedBossSystem.DownedWhiteTail)
                return;

            if (Main.npc.Any(x => x.active && x.type == ModContent.NPCType<WhiteTail>()))
                return;

            List<float> playerChanges = new List<float>();

            for (int i = 0; i < 255; i++)
            {
                var player = Main.player[i];
                if (!player.active || !ChangedUtils.InChangedSurfaceBiome(player))
                {
                    playerChanges.Add(0);
                    continue;
                }

                var nMinKills = 20;     // Minimum monster kills needed to spawn
                var nMaxKills = 40;     // Guaranteed to spawn

                // Black
                var nGoop = ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<BlackGoop>());
                var nCub = ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<DarkLatexCub>());
                var nAdult = ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<MaleDarkLatex>());
                nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<FemaleDarkLatex>());
                var nFlying = ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<FlyingDarkLatex>());

                // White
                nGoop += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<WhiteGoop>());
                nCub += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<WhiteLatexCub>());
                nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<WhiteKnight>());

                // Drunk
                if (ChangedUtils.IsDrunk(player))
                {
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<BackLatex>());
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<DarkLatexCubOfDoom>());
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<PuroWormHead>());
                    nCub += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<QuackLatex>());
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<SnackLatex>());
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<StackLatex>());
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<WackLatex>());

                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<BrightLatex>());
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<FightLatex>());
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<FlightLatex>());
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<HideLatex>());
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<MightLatex>());
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<SideLatex>());
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<WideLatex>());
                }

                // Increase the change even further if the player has more then 200 hp
                var playerHP = player.statLifeMax2;
                var playerHPMultiplier = Math.Max(0, playerHP - 200) / 200 * nMinKills;

                var nKills = 0.3f * nGoop + 0.5f * nCub + nAdult + 1.5f * nFlying + playerHPMultiplier;
                var chance = 0.0f;

                if (nKills < nMinKills)
                {
                    playerChanges.Add(0);
                    continue;
                }

                chance = 0.5f + (nKills - nMinKills) * (2.5f / (nMaxKills - nMinKills));
                playerChanges.Add(chance);
            }

            int highestIndex = -1;
            float highestValue = 0;
            for (int i = 0; i < playerChanges.Count; i++)
            {
                var playerChance = playerChanges[i];
                if (playerChance > highestValue)
                {
                    highestValue = playerChance;
                    highestIndex = i;
                }
            }

            if (highestIndex == -1)
                return;

            var irandom = Math.Max(1, (int)(2000 / highestValue));

            if (Main.rand.NextBool(irandom))
                NPC.SpawnOnPlayer(highestIndex, ModContent.NPCType<WhiteTail>());
        }

        public static bool WolfKingSpawnCheck(bool summon = false, int playerIndex = -1)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return false;

            // Don't spawn if already killed or the previous bosses haven't been killed yet
            if (!summon && (!DownedBossSystem.DownedWhiteTail || DownedBossSystem.DownedWolfKing))
                return false;

            // Don't spawn if he is already present
            if (NPC.AnyNPCs(ModContent.NPCType<WolfKing>()) || NPC.AnyNPCs(ModContent.NPCType<WolfKingSpawn>()))
                return false;

            int blockCheckSpacing = 6;
            if (summon && playerIndex != -1)
            {
                for (int y = 0; y < Main.worldSurface; y += blockCheckSpacing)
                {
                    for (int x = 0; x < Main.maxTilesX; x += blockCheckSpacing)
                    {
                        if (x < Main.maxTilesX)
                        {
                            var tile = Main.tile[x, y];
                            if (ChangedUtils.IsBlackLatexWall(tile))
                            {
                                var player = Main.player[playerIndex];//GetClosestPlayer(x, y);
                                ChangedUtils.TeleportPlayer(player.whoAmI, x, y);

                                SoundEngine.PlaySound(SoundID.NPCDeath64, player.Center);
                                ChangedUtils.SpawnWolfKing(x, y, player);
                                return true;
                            }
                        }
                    }
                }
                Main.NewText("Failed to find a suitable place for the boss fight. Please make a room with slime walls covered in black paint and try again");
            }
            else
            {
                foreach (var player in Main.ActivePlayers)
                {
                    int minRangeFromPlayer = 80;
                    int maxRange = 120;                         // Same range as the clentaminator, so probably enough to be off-screen
                    var xPos = (int)(player.position.X / 16);
                    var yPos = (int)(player.position.Y / 16);

                    for (int y = -maxRange; y <= maxRange; y += blockCheckSpacing)
                    {
                        for (int x = -maxRange; x <= maxRange; x += blockCheckSpacing)
                        {
                            if (Math.Abs(x) <= minRangeFromPlayer)
                                continue;

                            var tmpX = xPos + x;
                            var tmpY = yPos + y;

                            if (tmpX < 0 || tmpX >= Main.maxTilesX)
                                continue;

                            if (tmpY < 0 || tmpY >= Main.maxTilesY)
                                continue;

                            var tile = Main.tile[tmpX, tmpY];

                            if (ChangedUtils.IsBlackLatexWall(tile))
                            {
                                ChangedUtils.SpawnWolfKing(tmpX, tmpY, player);
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public static void BehemothSpawnCheck()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            // Don't spawn if already killed or the previous bosses haven't been killed yet
            if (DownedBossSystem.DownedBehemoth || !DownedBossSystem.DownedWhiteTail || !DownedBossSystem.DownedWolfKing)
                return;

            // Don't spawn if he is already present
            if (NPC.AnyNPCs(ModContent.NPCType<Behemoth>()) || NPC.AnyNPCs(ModContent.NPCType<BehemothSpawn>()))
                return;

            foreach (var player in Main.ActivePlayers)
            {
                int minRangeFromPlayer = 80;
                int maxRange = 120;                         // Same range as the clentaminator, so probably enough to be off-screen
                int blockCheckSpacing = 8;                  // Only check every 8 blocks for performance
                var xPos = (int)(player.position.X / 16);
                var yPos = (int)(player.position.Y / 16);

                for (int y = -maxRange; y <= maxRange; y += blockCheckSpacing)
                {
                    for (int x = -maxRange; x <= maxRange; x += blockCheckSpacing)
                    {
                        if (Math.Abs(x) <= minRangeFromPlayer)
                            continue;

                        var tmpX = xPos + x;
                        var tmpY = yPos + y;

                        if (tmpX < 0 || tmpX >= Main.maxTilesX)
                            continue;

                        if (tmpY < 0 || tmpY >= Main.maxTilesY)
                            continue;

                        var tile = Main.tile[tmpX, tmpY];

                        if (ChangedUtils.IsWhiteLatexWall(tile))
                        {
                            ChangedUtils.SpawnBehemoth(tmpX, tmpY, player);
                            return;
                        }
                    }
                }
            }
        }
    }
}
