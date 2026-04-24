using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
	public class DarkLatexCub : ModNPC
	{
        private enum ActionState
        {
            Walking,
            Sitting
        }

        public ref float AI_State => ref NPC.ai[3];
        public float MoveSpeed = 3.0f;
        public float MinRange = 64.0f;
        public float Acceleration = 1.0f / 60.0f * 4.0f;

        public override void SetStaticDefaults() 
		{
            Main.npcFrameCount[Type] = 4;
            NPCID.Sets.ShimmerTransformToNPC[NPC.type] = ModContent.NPCType<WhiteKnight>();
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
			NPC.height = 18;
			NPC.damage = 15;
			NPC.defense = 6;
			NPC.lifeMax = 40;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
            NPC.aiStyle = NPCAIStyleID.Fighter;
			AIType = NPCID.GoblinScout;
			AnimationType = NPCID.Zombie;
            SpawnModBiomes = new int[] { ModContent.GetInstance<BlackLatexSurfaceBiome>().Type };

            var changedNPC = NPC.Changed();
            changedNPC.AdjustStatScaling(NPC);
            changedNPC.SetNPCName(NPC);
            changedNPC.HatXOffset = -2;
            changedNPC.HatYOffset = -15;
            changedNPC.GooType = GooType.Black;
            changedNPC.ElementType = ElementType.None;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.SetHalloweenHatsForBlackLatex();
            changedNPC.DoOnSpawnExtra = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.DarkLatexCub.Description")),
            });
        }


        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Orange>(), 20));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            var ChangedGlobalNPC = NPC.Changed();
            return ChangedUtils.GetSurfaceSpawnChance(spawnInfo, ChangedGlobalNPC, NPC.type);
        }

        private void UpdateHatPosition(int frameHeight)
        {
            var changedNPC = NPC.Changed();
            var frame = NPC.frame;
            var fr = frame.Top / frameHeight;

            if (fr == 1)
            {
                changedNPC.HatYOffset = -17;//-16
            }
            else 
            {
                changedNPC.HatYOffset = -15;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
            UpdateHatPosition(frameHeight);
        }

        public void Walking()
        {
            NPC.TargetClosest(true);
            NPC.velocity.X = Math.Clamp(NPC.velocity.X + NPC.direction * Acceleration, -MoveSpeed, MoveSpeed);
            
            if (NPC.velocity.Y == 0)
            {
                if (NPC.HasValidTarget && Main.player[NPC.target].Distance(NPC.Center) < MinRange)
                {
                    AI_State = (float)ActionState.Sitting;
                    SoundEngine.PlaySound(Assets.Sounds.SoundTransfur, NPC.Center);
                }
                else
                {
                    NPC.velocity.Y = -5;
                }
            }

        }

        public void Sitting()
        {
            if (NPC.velocity.Y == 0)
            {
                NPC.velocity.X = 0;
            }

            NPC.TargetClosest(true);

            if (NPC.HasValidTarget && Main.player[NPC.target].Distance(NPC.Center) > MinRange)
            {
                AI_State = (float)ActionState.Walking;
                SoundEngine.PlaySound(Assets.Sounds.SoundTransfur, NPC.Center);
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC.Changed().PostDrawExtra(NPC, spriteBatch, screenPos, drawColor);
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }
    }
}
