using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Content.Items.Mounts;
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
	public class WhiteLatexTaur : ModNPC
	{
        public override void SetStaticDefaults() 
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Wolf];
            NPCID.Sets.ShimmerTransformToNPC[NPC.type] = ModContent.NPCType<WhiteKnight>();
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { // Influences how the NPC looks in the Bestiary
                Velocity = 2f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
                Scale = 1 / NPC.scale * 1.25f,
                PortraitScale = 1 / NPC.scale * 1.25f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

		public override void SetDefaults() {
			NPC.width = 18;
			NPC.height = 48;
            NPC.damage = 30;
            NPC.defense = 10;
            NPC.lifeMax = 80;
            NPC.HitSound = SoundID.NPCHit1; //SoundID.NPCHit6;
            NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.35f;
			NPC.aiStyle = NPCAIStyleID.Unicorn;
			AIType = NPCID.Wolf;
			AnimationType = NPCID.Wolf;
            SpawnModBiomes = new int[] { ModContent.GetInstance<WhiteLatexSurfaceBiome>().Type };

            var ChangedGlobalNPC = NPC.Changed();
            ChangedGlobalNPC.AdjustStatScaling(NPC);
            ChangedGlobalNPC.SetNPCName(NPC);

            ChangedGlobalNPC.HatXOffset = -8;
            ChangedGlobalNPC.HatYOffset = -34;
            ChangedGlobalNPC.GooType = GooType.White;
            ChangedGlobalNPC.ElementType = ElementType.None;
            ChangedGlobalNPC.DefaultOnHitPlayer = true;
            ChangedGlobalNPC.DefaultHitEffect = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.WhiteLatexTaur.Description"))
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WhiteLatexTaurMountItem>(), 20));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!ChangedUtils.CanSpawnFastLatex())
                return 0;
            var ChangedGlobalNPC = NPC.Changed();
            return ChangedUtils.GetSurfaceSpawnChance(spawnInfo, ChangedGlobalNPC, NPC.type);
        }

        public override void OnSpawn(IEntitySource source)
        {
            NPC.Changed().OnSpawnExtra(NPC);
            base.OnSpawn(source);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var changedNPC = NPC.Changed();
            var frame = NPC.frame;
            NPC.Changed().PostDrawExtra(NPC, spriteBatch, screenPos, drawColor);
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }

        private void UpdateHatPosition(int frameHeight)
        {
            int frameNumber = NPC.frame.Y / frameHeight;
            var ChangedNPC = NPC.Changed();
            var hatYOffset = -34;

            if (frameNumber == 9)
            {
                hatYOffset -= 2;
            }
            else if (frameNumber == 5)
            {
                hatYOffset -= 3;
            }
            else if (frameNumber == 6 || frameNumber == 8)
            {
                hatYOffset -= 4;
            }
            else if (frameNumber == 7)
            {
                hatYOffset -= 5;
            }

            ChangedNPC.HatYOffset = hatYOffset;
        }

        public override void FindFrame(int frameHeight)
        {
            int frameNumber = NPC.frame.Y / frameHeight;
            if (NPC.IsABestiaryIconDummy && frameNumber < 3)
            {
                frameNumber = 3;
            }
            // Falling
            if (NPC.velocity.Y < 0f)
            {
                frameNumber = 10;
            }
            // Jumping
            else if (NPC.velocity.Y > 0f)
            {
                frameNumber = 11;
            }
            // Standing still
            else if (NPC.velocity.X == 0f)
            {
                frameNumber = 0;
                NPC.frameCounter = 0.0;
            }
            // Turning around
            else if ((
                // Sliding to the left while facing right 
                (NPC.direction > 0 && NPC.velocity.X < 0f) || 
                // Sliding to the right while facing left
                (NPC.direction < 0 && NPC.velocity.X > 0f)) && Math.Abs(NPC.velocity.X) < 4f)
            {
                frameNumber = 2;
                NPC.frameCounter = 0.0f;
                NPC.spriteDirection = ((!(NPC.velocity.X < 0f)) ? 1 : -1);
            }
            else
            {
                NPC.spriteDirection = ((!(NPC.velocity.X < 0f)) ? 1 : -1);
                NPC.frameCounter += Math.Abs(NPC.velocity.X) * 0.4f;

                // Chance from turning frame to first frame of running animation
                if (frameNumber == 2)
                {
                    frameNumber = 3;
                    NPC.frameCounter = 0.0;
                }
                
                // Falling or Jumping frame and we hit the ground
                if (frameNumber == 10 || frameNumber == 11)
                {
                    frameNumber = 12;
                    NPC.frameCounter = 0.0;
                }
                // Normal running animation
                else if (NPC.frameCounter > 8.0)
                {
                    NPC.frameCounter -= 8.0;
                    frameNumber++;
                    if (frameNumber > 9)
                    {
                        frameNumber = 3;
                    }
                }
            }
            NPC.frame.Y = frameNumber * frameHeight;
            UpdateHatPosition(frameHeight);
        }
    }
}
