using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Utilities;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
	public class FlightLatex : ModNPC
	{
        public override string Texture => "ChangedSpecialMod/Content/NPCs/Drunk/FlightLatex";

        public override void SetStaticDefaults() 
        {
			Main.npcFrameCount[Type] = 6;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f,
                Scale = 1 / NPC.scale * 1.25f,
                PortraitScale = 1 / NPC.scale * 1.25f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

		public override void SetDefaults() 
        {
            NPC.width = 18;
			NPC.height = 45;
            NPC.damage = 25;
            NPC.defense = 10;
            NPC.lifeMax = 70;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = NPCAIStyleID.Bat;
			NPC.noGravity = true;
            AnimationType = NPCID.None;
            SpawnModBiomes = new int[] { ModContent.GetInstance<ZDrunkBiome>().Type };

            var changedNPC = NPC.Changed();
            changedNPC.AdjustStatScaling(NPC);
            changedNPC.SetNPCName(NPC);
            changedNPC.GooType = GooType.White;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.DoOnSpawnExtra = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.FlightLatex.Description")),
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!ChangedUtils.IsDrunk(spawnInfo.Player))
                return 0;
            var changedNPC = NPC.Changed();
            return ChangedUtils.GetSurfaceSpawnChance(spawnInfo, changedNPC, NPC.type);
        }

        public override void FindFrame(int frameHeight)
        {
            int currentFrame = NPC.frame.Y / frameHeight;

            NPC.frameCounter++;
            if (NPC.frameCounter > 4.0)
            {
                NPC.frameCounter = 0.0;
                currentFrame++;
            }

            if (currentFrame >= Main.npcFrameCount[Type])
            {
                currentFrame = 0;
            }

            NPC.frame.Y = currentFrame * frameHeight;
        }

        public override void AI()
        {
            base.AI();
            NPC.spriteDirection = Math.Sign(NPC.velocity.X);
        }
    }
}
