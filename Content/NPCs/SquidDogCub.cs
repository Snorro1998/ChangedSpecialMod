using ChangedSpecialMod.Content.Biomes;
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
            NPCID.Sets.ShimmerTransformToNPC[NPC.type] = ModContent.NPCType<MaleDarkLatex>();
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
            AIType = 0;
            AnimationType = NPCID.None;
            SpawnModBiomes = new int[] { ModContent.GetInstance<CityRuinsSurfaceBiome>().Type };

            var ChangedGlobalNPC = NPC.Changed();
            ChangedGlobalNPC.AdjustStatScaling(NPC);
            ChangedGlobalNPC.SetNPCName(NPC);
            // Even though he is white, he should appear everywhere
            ChangedGlobalNPC.GooType = GooType.None;
            ChangedGlobalNPC.ElementType = ElementType.Water;
            ChangedGlobalNPC.IsFish = true;
            ChangedGlobalNPC.DefaultOnHitPlayer = true;
            ChangedGlobalNPC.DefaultHitEffect = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Remove the default portrait, otherwise you get two of them
            bestiaryEntry.Info.RemoveAt(2);
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                //  Add the new portrait with the modified rarity
                new NPCPortraitInfoElement(3),
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.SquidDogCub.Description")),
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot) 
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.BlackInk, 2));
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            var ChangedGlobalNPC = NPC.Changed();
            return ChangedUtils.GetSurfaceSpawnChance(spawnInfo, ChangedGlobalNPC, NPC.type);
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
            ChangedUtils.AI_018_JellyFish(NPC);
        }
    }
}
