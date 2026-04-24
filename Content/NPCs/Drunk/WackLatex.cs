using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace ChangedSpecialMod.Content.NPCs
{
	public class WackLatex : ModNPC
	{
        public override string Texture => "ChangedSpecialMod/Content/NPCs/Drunk/WackLatex";

        public override void SetStaticDefaults() 
        {
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Zombie];
            //NPCID.Sets.ShimmerTransformToNPC[NPC.type] = ModContent.NPCType<WhiteKnight>();
            //ChangedUtils.HideFromBestiary(this);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { // Influences how the NPC looks in the Bestiary
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
                Scale = 1 / NPC.scale * 1.25f,
                PortraitScale = 1 / NPC.scale * 1.25f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

		public override void SetDefaults() 
        {
			NPC.width = 18;
			NPC.height = 35;
            NPC.damage = 30;
            NPC.defense = 12;
            NPC.lifeMax = 80; //320
            NPC.HitSound = SoundID.NPCHit1; //SoundID.NPCHit6;
            NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = NPCAIStyleID.Fighter;
			AIType = NPCID.GoblinScout;
			AnimationType = NPCID.Zombie;
            SpawnModBiomes = new int[] { ModContent.GetInstance<ZDrunkBiome>().Type };

            var changedNPC = NPC.Changed();
            changedNPC.AdjustStatScaling(NPC);
            changedNPC.SetNPCName(NPC);
            changedNPC.SetHalloweenHatsForBlackLatex();
            changedNPC.GooType = GooType.Black;
            changedNPC.ElementType = ElementType.None;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.RemoveHat(ItemID.FlowerBoyHat);
            changedNPC.RemoveHat(ItemID.TheBrideHat);
            changedNPC.RemoveHat(ItemID.BuccaneerBandana);
            changedNPC.DoOnSpawnExtra = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.WackLatex.Description")),
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Orange>(), 20));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!ChangedUtils.IsDrunk(spawnInfo.Player))
                return 0;
            var ChangedGlobalNPC = NPC.Changed();
            return ChangedUtils.GetSurfaceSpawnChance(spawnInfo, ChangedGlobalNPC, NPC.type);
        }

        public override void AI()
        {
            base.AI();
            NPC.rotation += (float)(Math.PI * 2 / 60.0f * NPC.spriteDirection);
            var ChangedGlobalNPC = NPC.Changed();
            ChangedGlobalNPC.HatXOffset = (float)(Math.Sin(NPC.rotation) * -22 * NPC.spriteDirection); //0
            ChangedGlobalNPC.HatYOffset = (float)(Math.Cos(NPC.rotation) * -22);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC.Changed().PostDrawExtra(NPC, spriteBatch, screenPos, drawColor);
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }
    }
}
