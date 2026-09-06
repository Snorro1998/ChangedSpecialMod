using ChangedSpecialMod.Content.NPCs.AIStyles;
using ChangedSpecialMod.Utilities;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
	public class SquidDogCub : ModNPC
	{
        public override void SetStaticDefaults() 
		{
			Main.npcFrameCount[Type] = 4;
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
			NPC.width = 22;
			NPC.height = 42;
			NPC.damage = 20;
			NPC.defense = 6;
			NPC.lifeMax = 50;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
            NPC.aiStyle = -1;
            AIType = NPCID.None;
            AnimationType = NPCID.None;

            var changedNPC = NPC.Changed();
            changedNPC.AdjustStatScaling(NPC);
            changedNPC.SetNPCName(NPC);
            changedNPC.GooType = GooType.None;
            changedNPC.ElementType = ElementType.Water;
            changedNPC.IsFish = true;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.spawnDepth = SpawnDepth.Everywhere;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.SquidDogCub.Description")),
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot) 
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.BlackInk, 2));
            npcLoot.Add(ItemDropRule.Common(ItemID.JellyfishNecklace, 100));
        }

        // Maybe add a config option for the old logic?
        // return ChangedUtils.GetFishSpawnChance(spawnInfo, changedNPC, NPC.type);
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            var changedNPC = NPC.Changed();
            var spawnTileIsWater = spawnInfo.Water;

            if (spawnInfo.Player.ZoneBeach && spawnTileIsWater && ChangedUtils.CanSpawn(changedNPC.spawnRequirement))
                return 0.3f;

            return 0f;
        }

        // Squid animation logic
        public override void FindFrame(int frameHeight)
        {
            // Chasing player
            if (NPC.localAI[2] == 1f)
            {
                var currentFrame = NPC.frame.Y / frameHeight;
                if (Math.Abs(NPC.velocity.X) > 1f)
                {
                    if (currentFrame < 2)
                    {
                        NPC.frameCounter += 2.0;
                    }
                }
                else
                {
                    if (currentFrame > 0)
                    {
                        NPC.frameCounter += 1.0;
                    }
                }

                if (NPC.frameCounter >= 8.0)
                {
                    currentFrame++;
                    if (currentFrame > 3)
                        currentFrame = 0;
                    NPC.frame.Y = currentFrame * frameHeight;
                    NPC.frameCounter = 0.0;
                }
            }
            // Idling
            else
            {
                NPC.frameCounter += 1.0;
                if (NPC.frameCounter >= 13.0)
                {
                    NPC.frame.Y += frameHeight;
                    NPC.frameCounter = 0.0;
                }
                if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[NPC.type])
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override void AI()
        {
            NPC.noGravity = true;
            AIJellyFish.Update(NPC);
        }
    }
}
