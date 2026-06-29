using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items.Food;
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
    public class BodyPart
    {
        private string bodyPartName;
        public int bodyPartID;
        public float Rotation;

        public BodyPart(string bodyPartName, int bodyPartID)
        {
            this.bodyPartName = bodyPartName;
            this.bodyPartID = bodyPartID;
        }

        private Texture2D GetTexture()
        {
            // Caching this doesn't work and I don't know why
            return ChangedSpecialMod.Instance.Assets.Request<Texture2D>($"Assets/Textures/Body/{bodyPartName}{bodyPartID}").Value;
        }

        public void Draw(NPC npc, SpriteBatch spriteBatch, Vector2 drawPos, Vector2 offset, Color drawColor, SpriteEffects effects)
        {
            var rotation = Rotation * npc.spriteDirection;
            var texture = GetTexture();
            offset.X *= npc.spriteDirection;
            spriteBatch.Draw(
                texture,
                drawPos + (offset * npc.scale),
                null,
                drawColor,
                npc.rotation + rotation,
                texture.Size() / 2f,
                npc.scale,
                effects,
                0f
            );
        }
    }

	public class MutatedLatex : ModNPC
	{
        private Color color = Color.White;
        private Color hairColor = Color.White;
        private Color eyeColor = Color.Blue;
        private int torsoIndex = 1;
        private int tailIndex = 1;
        private int legsIndex = 1;
        private int headIndex = 1;
        private int earsIndex = 1;
        private int armIndex = 1;
        private int eyeIndex = 1;
        private int fluffIndex = 1;
        private int hairIndex = 1;

        private bool secondHead = false;
        private bool hasMask = false;

        public override void SetStaticDefaults() 
        {
			Main.npcFrameCount[Type] = 4;
            NPCID.Sets.ShimmerTransformToNPC[Type] = ModContent.NPCType<WhiteKnight>();
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f,
                Scale = 1 / NPC.scale * 0.2f,
                PortraitScale = 1 / NPC.scale * 0.2f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

		public override void SetDefaults() 
        {
            NPC.width = 18;
            NPC.height = 120;
            NPC.damage = 40;
            NPC.defense = 12;
            NPC.lifeMax = 100;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = NPCAIStyleID.Fighter;
			AIType = NPCID.GoblinScout;
            AnimationType = NPCID.None;
            SpawnModBiomes = new int[] { ModContent.GetInstance<BlackLatexSurfaceBiome>().Type };

            var changedNPC = NPC.Changed();
            changedNPC.BaseScaleMultiplier = 0.4f;
            changedNPC.AdjustStatScaling(NPC);

            changedNPC.SetHalloweenHatsForBlackLatex();
            changedNPC.GooType = GooType.None;
            changedNPC.ElementType = ElementType.None;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.CanHaveBeer = true;
            changedNPC.BeerXOffset = -16;
            changedNPC.spawnRequirement = SpawnRequirement.WhiteTail;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.MutatedLatex.Description")),
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Orange>(), 20));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            var multiplier = 0.4f;
            if (spawnInfo.Player != null && ChangedUtils.IsDrunk(spawnInfo.Player))
                multiplier = 0.8f;
            var ChangedGlobalNPC = NPC.Changed();
            return multiplier * ChangedUtils.GetSurfaceSpawnChance(spawnInfo, ChangedGlobalNPC, NPC.type);
        }

        private void ChooseBodyParts()
        {
            // Choose body parts
            tailIndex = ChangedUtils.Choose(3, 4, 7, 8, 10, 12, 13, 14);
            torsoIndex = ChangedUtils.Choose(1, 2, 3, 11, 12);
            legsIndex = ChangedUtils.Choose(1, 2, 4, 7, 8, 9);
            earsIndex = ChangedUtils.Choose(1, 2, 3, 4, 7, 8, 9, 10, 11, 12);
            armIndex = ChangedUtils.Choose(1, 5, 9, 10, 11);
            eyeIndex = ChangedUtils.Choose(1, 2, 3, 4, 5, 6, 7);
            headIndex = ChangedUtils.Choose(1, 2, 3, 4);
            hairIndex = ChangedUtils.Choose(1, 2, 3, 4, 5, 6);
            fluffIndex = ChangedUtils.Choose(1, 2, 7, 9, 11);
            secondHead = Main.rand.NextBool(3);
        }

        public override void OnSpawn(IEntitySource source)
        {
            var changedNPC = NPC.Changed();
            NPC.TargetClosest();

            ChooseBodyParts();
            hasMask = false;

            changedNPC.GooType = ChangedUtils.Choose(GooType.Black, GooType.White);

            if (NPC.HasValidTarget)
            {
                var player = Main.player[NPC.target];
                if (!ChangedUtils.IsDrunk(player))
                {
                    if (ChangedUtils.InBlackLatexBiome(player))
                    {
                        changedNPC.GooType = GooType.Black;
                    }
                    else if (ChangedUtils.InWhiteLatexBiome(player))
                    {
                        changedNPC.GooType = GooType.White;
                    }
                }
                else
                {
                    changedNPC.GooType = GooType.None;
                }
            }

            var tmpName = Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.MutatedLatex.DisplayName");

            // Pick skin color
            switch (changedNPC.GooType)
            {
                // GooType none, drunk
                default:
                    tmpName = Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.MutatedLatex.DisplayNameDrunk");
                    color = new Color(Main.rand.Next(255), Main.rand.Next(255), Main.rand.Next(255));
                    // Pick tuft hair so you always get multiple colors
                    hairIndex = ChangedUtils.Choose(4, 5, 6);
                    break;
                case GooType.Black:
                    tmpName = Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.MutatedLatex.DisplayNameBlack");
                    color = new Color(57, 57, 57);
                    hasMask = true;
                    break;
                case GooType.White:
                    tmpName = Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.MutatedLatex.DisplayNameWhite");
                    color = Color.White;
                    break;
            }

            changedNPC.SetNPCName(NPC, tmpName);
            hairColor = color;
            eyeColor = ChangedUtils.Choose(Color.Blue, Color.Red, Color.Yellow, Color.Purple, Color.Green);

            // If the chosen hair is tuft and not one with the head, allow hair color to differ from skin color            
            var isTuftHair = hairIndex > 3;
            if (isTuftHair)
            {
                switch (changedNPC.GooType)
                {
                    default:
                        hairColor = new Color(Main.rand.Next(255), Main.rand.Next(255), Main.rand.Next(255));
                        break;
                    case GooType.Black:
                        // Red hair color from black latex on spritesheet 25 
                        hairColor = new Color(255, 1, 78);
                        // Reroll eyes to exclude some
                        eyeIndex = ChangedUtils.Choose(1, 2, 3, 4, 5);
                        eyeColor = hairColor;
                        hasMask = false;
                        break;
                    case GooType.White:
                        eyeColor = Color.White;
                        break;
                }
            }

            NPC.Changed().OnSpawnExtra(NPC);
            base.OnSpawn(source);
        }

        private void DrawHead(ChangedNPC changedNPC, SpriteBatch spriteBatch, Vector2 drawPos, Vector2 offset, Color drawColor, Color hairColor, Color eyeColor, Color lightingColor, SpriteEffects effects, float rotation, bool hideEars)
        {
            offset.X *= NPC.spriteDirection;
            //rotation *= NPC.spriteDirection;
            drawPos += offset;

            // Ears
            if (!hideEars)
            {
                var bodyPartEars = new BodyPart("Ears", earsIndex);
                bodyPartEars.Rotation = rotation;
                bodyPartEars.Draw(NPC, spriteBatch, drawPos, new Vector2(0, -64), drawColor, effects);
            }
            
            // Head
            var bodyPartHead = new BodyPart("Head", headIndex);
            bodyPartHead.Rotation = rotation;
            bodyPartHead.Draw(NPC, spriteBatch, drawPos, new Vector2(0, -64), drawColor, effects);

            if (hasMask)
            {
                var bodyPartBlackLatexMask = new BodyPart("HeadBlackLatex", 1);
                bodyPartBlackLatexMask.Rotation = rotation;
                bodyPartBlackLatexMask.Draw(NPC, spriteBatch, drawPos, new Vector2(0, -64), lightingColor, effects);
            }
            else
            {
                // Eye base should be white
                var bodyPartEye = new BodyPart("Eye", eyeIndex);
                bodyPartEye.Rotation = rotation;
                bodyPartEye.Draw(NPC, spriteBatch, drawPos, new Vector2(0, -64), lightingColor, effects);

                // Pupil
                if (eyeIndex < 6)
                {
                    var bodyPartPupil = new BodyPart("Pupil", 1);
                    bodyPartPupil.Rotation = rotation;
                    bodyPartPupil.Draw(NPC, spriteBatch, drawPos, new Vector2(0, -64), eyeColor, effects);
                }
            }

            // Hair
            var bodyPartHair = new BodyPart("Hair", hairIndex);
            bodyPartHair.Rotation = rotation;
            bodyPartHair.Draw(NPC, spriteBatch, drawPos, new Vector2(0, -64), hairColor, effects);
        }

        public override void FindFrame(int frameHeight)
        {
            int frameNumber = NPC.frame.Y / frameHeight;

            if (NPC.velocity.Y == 0 && Math.Abs(NPC.velocity.X) > 0.4f)
                NPC.frameCounter += 1.0f;

            if (NPC.frameCounter >= 8)
            {
                NPC.frameCounter = 0;
                frameNumber = (frameNumber + 1) % 4;
            }

            NPC.frame.Y = frameNumber * frameHeight;
        }

        private Color GetColor(Color color, Color lightingColor)
        {
            return new Color((int)((float)color.R * ((float)lightingColor.R / 255.0f)),
                (int)((float)color.G * ((float)lightingColor.G / 255.0f)),
                (int)((float)color.B * ((float)lightingColor.B / 255.0f)));
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.velocity.Y == 0)
                NPC.spriteDirection = NPC.direction;

            Color lightingColor = Lighting.GetColor((int)NPC.Center.X / 16, (int)(NPC.Center.Y / 16f));
            var changedNPC = NPC.Changed();
            
            if (NPC.IsABestiaryIconDummy)
            {
                changedNPC.GooType = GooType.Black;
                lightingColor = Color.White;
                color = new Color(57, 57, 57);
                hairColor = new Color(57, 57, 57);
                armIndex = 11;
                legsIndex = 7;
                torsoIndex = 3;
                headIndex = 2;
                earsIndex = 4;
                eyeIndex = 2;
                fluffIndex = 1;
                hairIndex = 1;
                hasMask = true;
            }

            // Body color
            drawColor = GetColor(color, lightingColor);

            // Hair color
            var drawColorHair = GetColor(hairColor, lightingColor);

            // Eye color
            var drawColorEyes = GetColor(eyeColor, lightingColor);

            var bodyPartTail = new BodyPart("Tail", tailIndex);
            var bodyPartLegL = new BodyPart("LegL", legsIndex);
            var bodyPartLegR = new BodyPart("LegR", legsIndex);
            var bodyPartTorso = new BodyPart("Torso", torsoIndex);
            //head
            //ears
            var bodyPartArmL = new BodyPart("ArmL", armIndex);
            var bodyPartArmR = new BodyPart("ArmR", armIndex);
            // eye
            // hair
            var bodyPartFluff = new BodyPart("Fluff", fluffIndex);
            // pupil
            // blackmask

            Vector2 drawPos = NPC.Center - screenPos;
            drawPos.Y += NPC.gfxOffY;
            drawPos.Y += 8;

            SpriteEffects effects = NPC.spriteDirection == 1
                ? SpriteEffects.FlipHorizontally
                : SpriteEffects.None;

            var walkCycleYOffset = 0;
            int frame = NPC.frame.Y / 59;

            if (frame == 0)
            {
                walkCycleYOffset = 0;
                bodyPartArmL.Rotation = 0.6f;
                bodyPartArmR.Rotation = -0.4f;
                bodyPartTail.Rotation = 0.3f;
                bodyPartLegL.Rotation = 0;
                bodyPartLegR.Rotation = 0;
            }

            else if (frame == 1)
            {
                walkCycleYOffset = -1;
                bodyPartArmL.Rotation = 0.3f;
                bodyPartArmR.Rotation = -0.1f;
                bodyPartTail.Rotation = 0.6f;
                bodyPartLegL.Rotation = 0.375f;
                bodyPartLegR.Rotation = -0.375f;
            }

            else if (frame == 2)
            {
                walkCycleYOffset = -2;
                bodyPartArmL.Rotation = 0f;
                bodyPartArmR.Rotation = 0.2f;
                bodyPartTail.Rotation = 0.3f;
                bodyPartLegL.Rotation = 0.75f;
                bodyPartLegR.Rotation = -0.75f;
            }

            else if (frame == 3)
            {
                walkCycleYOffset = -1;
                bodyPartArmL.Rotation = 0.3f;
                bodyPartArmR.Rotation = -0.1f;
                bodyPartTail.Rotation = 0f;
                bodyPartLegL.Rotation = 0.375f;
                bodyPartLegR.Rotation = -0.375f;
            }

            drawPos.Y += walkCycleYOffset;
            var hideEars = headIndex > 12;
            var hideTail = bodyPartLegL.bodyPartID == 7;

            if (!hideTail)
                bodyPartTail.Draw(NPC, spriteBatch, drawPos, Vector2.Zero, drawColor, effects);

            bodyPartArmL.Draw(NPC, spriteBatch, drawPos, new Vector2(10, -45), drawColor, effects);
            bodyPartLegL.Draw(NPC, spriteBatch, drawPos, Vector2.Zero, drawColor, effects);
            bodyPartLegR.Draw(NPC, spriteBatch, drawPos, Vector2.Zero, drawColor, effects);
            bodyPartTorso.Draw(NPC, spriteBatch, drawPos, new Vector2(0, -32), drawColor, effects);
            bodyPartFluff.Draw(NPC, spriteBatch, drawPos, new Vector2(0, -56), drawColorHair, effects);

            // Two heads
            if (secondHead)
            {
                DrawHead(changedNPC, spriteBatch, drawPos, new Vector2(8, 0), drawColor, drawColorHair, drawColorEyes, lightingColor, effects, 0.15f, hideEars);
                DrawHead(changedNPC, spriteBatch, drawPos, new Vector2(-8, 0), drawColor, drawColorHair, drawColorEyes, lightingColor, effects, -0.15f, hideEars);
            }
            // One head
            else
            {
                DrawHead(changedNPC, spriteBatch, drawPos, Vector2.Zero, drawColor, drawColorHair, drawColorEyes, lightingColor, effects, 0, hideEars);
            }

            bodyPartArmR.Draw(NPC, spriteBatch, drawPos, new Vector2(-11, -46), drawColor, effects);
            return false;
        }
    }
}
