using ChangedSpecialMod.Content.NPCs.AIStyles;
using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
    public class TabbyCat : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 10;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f,
                Scale = 1 / NPC.scale,
                PortraitScale = 1 / NPC.scale
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 35;
            NPC.damage = 30;
            NPC.defense = 12;
            NPC.lifeMax = 80;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = -1;
            AIType = NPCID.None;

            var changedNPC = NPC.Changed();
            changedNPC.AdjustStatScaling(NPC);
            changedNPC.SetNPCName(NPC);
            changedNPC.HatXOffset = -2;
            changedNPC.HatYOffset = -15;
            changedNPC.GooType = GooType.None;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.RemoveAllHats();
            changedNPC.DoOnSpawnExtra = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.TabbyCat.Description")),
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            var changedNPC = NPC.Changed();
            if (!ChangedUtils.CommonCanSpawn(spawnInfo, changedNPC))
                return 0f;

            var correctTime = Main.IsItDay() || Main.remixWorld;
            if (!ChangedUtils.CanSpawn(SpawnRequirement.WolfKing) || !correctTime || Main.eclipse || NPC.AnyNPCs(ModContent.NPCType<GermanShepherd>()))
                return 0;

            var spawnChance = 0.08f;
            var spawnTileType = spawnInfo.SpawnTileType;
            var player = spawnInfo.Player;
            var validTileTypes = new List<int>()
            {
                TileID.Grass,
                ModContent.TileType<DryDirtGrassTile>()
            };

            if (validTileTypes.Contains(spawnTileType))
            {
                if (BiomeChecks.InChangedBiome(player))
                {
                    return spawnChance;
                }
                else if (spawnInfo.Player.townNPCs <= 2)
                {
                    return spawnChance;
                }
            }

            return 0;
        }

        public override void FindFrame(int frameHeight)
        {
            var imageIndex = NPC.frame.Y / frameHeight;

            if (NPC.velocity.Y != 0)
            {
                imageIndex = 1;
            }
            else
            {
                NPC.spriteDirection = NPC.direction;
                NPC.frameCounter += Math.Abs(NPC.velocity.X);
                if (NPC.frameCounter >= 5)
                {
                    NPC.frameCounter = 0;
                    imageIndex++;
                    if (imageIndex < 2 || imageIndex >= 10)
                    {
                        imageIndex = 2;
                    }
                }
            }

            NPC.frame.Y = imageIndex * frameHeight;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC.Changed().PostDrawExtra(NPC, spriteBatch, screenPos, drawColor);
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }

        public override void AI()
        {
            AIFighter.Update(NPC);
        }
    }
}
