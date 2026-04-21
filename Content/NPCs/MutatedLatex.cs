using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Content.Items.Placeable.Banners;
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
            { // Influences how the NPC looks in the Bestiary
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
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
            AnimationType = NPCID.None;// NPCID.Zombie;
            SpawnModBiomes = new int[] { ModContent.GetInstance<BlackLatexSurfaceBiome>().Type };

            var changedNPC = NPC.Changed();
            changedNPC.BaseScaleMultiplier = 0.4f;
            changedNPC.AdjustStatScaling(NPC);
            //changedNPC.SetNPCName(NPC);

            changedNPC.SetHalloweenHatsForBlackLatex();

            changedNPC.GooType = GooType.None;
            changedNPC.ElementType = ElementType.None;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.CanHaveBeer = true;
            changedNPC.BeerXOffset = -16;



            //NPC.scale *= 0.4f;// Main.rand.NextFloat(0.4f, 0.7f);
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
            if (!ChangedUtils.CanSpawnFastLatex())
                return 0;

            var multiplier = 0.4f;
            if (spawnInfo.Player != null && ChangedUtils.IsDrunk(spawnInfo.Player))
                multiplier = 0.8f;
            var ChangedGlobalNPC = NPC.Changed();
            return multiplier * ChangedUtils.GetSurfaceSpawnChance(spawnInfo, ChangedGlobalNPC, NPC.type);
        }

        public override void OnSpawn(IEntitySource source)
        {
            var changedNPC = NPC.Changed();
            NPC.TargetClosest();
            //NPC.GivenName = "Mutated Latex";

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

            var tmpName = "Mutated Latex";

            // Pick skin color
            switch (changedNPC.GooType)
            {
                // GooType none, drunk
                default:
                    tmpName = "Furry";
                    color = new Color(Main.rand.Next(255), Main.rand.Next(255), Main.rand.Next(255));
                    // Pick tuft hair so you always get multiple colors
                    hairIndex = ChangedUtils.Choose(4, 5, 6);
                    break;
                case GooType.Black:
                    tmpName = "Mutated Black Latex";
                    color = new Color(57, 57, 57);
                    hasMask = true;
                    break;
                case GooType.White:
                    tmpName = "Mutated White Latex";
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

        private void DrawTexture(SpriteBatch spriteBatch, Texture2D texture, Vector2 drawPos, Vector2 offset, Color drawColor, SpriteEffects effects, float rotation)
        {
            offset.X *= NPC.spriteDirection;
            spriteBatch.Draw(
                texture,
                drawPos + (offset * NPC.scale),
                null,
                drawColor,
                NPC.rotation + rotation,
                texture.Size() / 2f,
                NPC.scale,
                effects,
                0f
            );
        }

        private void DrawHead(ChangedNPC changedNPC, SpriteBatch spriteBatch, Texture2D headTexture, Texture2D earsTexture, Texture2D eyeTexture, Texture2D hairTexture, Texture2D pupilTexture, Vector2 drawPos, Vector2 offset, Color drawColor, Color hairColor, Color eyeColor, Color lightingColor, SpriteEffects effects, float rotation, bool hideEars)
        {
            offset *= NPC.spriteDirection;
            rotation *= NPC.spriteDirection;
            drawPos += offset;


            // Ears
            if (!hideEars)
                DrawTexture(spriteBatch, earsTexture, drawPos, new Vector2(0, -64), drawColor, effects, rotation);
            // Head
            DrawTexture(spriteBatch, headTexture, drawPos, new Vector2(0, -64), drawColor, effects, rotation);

            if (hasMask)
            {
                var blackLatexMaskTexture = Mod.Assets.Request<Texture2D>("Assets/Textures/Body/HeadBlackLatex1").Value;
                DrawTexture(spriteBatch, blackLatexMaskTexture, drawPos, new Vector2(0, -64), lightingColor, effects, rotation);
            }
            else
            {
                // Eye base should be white
                DrawTexture(spriteBatch, eyeTexture, drawPos, new Vector2(0, -64), lightingColor, effects, rotation);

                if (eyeIndex < 6)
                {
                    DrawTexture(spriteBatch, pupilTexture, drawPos, new Vector2(0, -64), eyeColor, effects, rotation);
                }
            }

            // Hair
            DrawTexture(spriteBatch, hairTexture, drawPos, new Vector2(0, -64), hairColor, effects, rotation);
        }

        public override void FindFrame(int frameHeight)
        {
            int frameNumber = NPC.frame.Y / frameHeight;

            if (NPC.velocity.Y == 0 && Math.Abs(NPC.velocity.X) > 0.4f)
            {
                
                NPC.frameCounter += 1.0f;
            }

            // No idea why 1000 is needed here, because it isn't for other npcs
            if (NPC.frameCounter >= 8)
            {
                NPC.frameCounter = 0;
                frameNumber = (frameNumber + 1) % 4;
            }

            NPC.frame.Y = frameNumber * frameHeight;
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
            drawColor = new Color((int)((float)color.R * ((float)lightingColor.R / 255.0f)), 
                (int)((float)color.G * ((float)lightingColor.G / 255.0f)), 
                (int)((float)color.B * ((float)lightingColor.B / 255.0f)));

            // Hair color
            var drawColor2 = new Color((int)((float)hairColor.R * ((float)lightingColor.R / 255.0f)),
                (int)((float)hairColor.G * ((float)lightingColor.G / 255.0f)),
                (int)((float)hairColor.B * ((float)lightingColor.B / 255.0f)));

            // Eye color
            var drawColor3 = new Color((int)((float)eyeColor.R * ((float)lightingColor.R / 255.0f)),
                (int)((float)eyeColor.G * ((float)lightingColor.G / 255.0f)),
                (int)((float)eyeColor.B * ((float)lightingColor.B / 255.0f)));

            var tailTexture = Mod.Assets.Request<Texture2D>($"Assets/Textures/Body/Tail{tailIndex}").Value;
            
            var legLTexture = Mod.Assets.Request<Texture2D>($"Assets/Textures/Body/LegL{legsIndex}").Value;
            var legRTexture = Mod.Assets.Request<Texture2D>($"Assets/Textures/Body/LegR{legsIndex}").Value;

            //var legsTexture = Mod.Assets.Request<Texture2D>($"Assets/Textures/Body/Legs{legsIndex}").Value;
            var bodyTexture = Mod.Assets.Request<Texture2D>($"Assets/Textures/Body/Torso{torsoIndex}").Value;
            
            var headTexture = Mod.Assets.Request<Texture2D>($"Assets/Textures/Body/Head{headIndex}").Value;
            var earsTexture = Mod.Assets.Request<Texture2D>($"Assets/Textures/Body/Ears{earsIndex}").Value;

            var armLTexture = Mod.Assets.Request<Texture2D>($"Assets/Textures/Body/ArmL{armIndex}").Value;
            var armRTexture = Mod.Assets.Request<Texture2D>($"Assets/Textures/Body/ArmR{armIndex}").Value;

            var eyeTexture = Mod.Assets.Request<Texture2D>($"Assets/Textures/Body/Eye{eyeIndex}").Value;
            var hairTexture = Mod.Assets.Request<Texture2D>($"Assets/Textures/Body/Hair{hairIndex}").Value;

            var fluffTexture = Mod.Assets.Request<Texture2D>($"Assets/Textures/Body/Fluff{fluffIndex}").Value;
            var pupilTexture = Mod.Assets.Request<Texture2D>("Assets/Textures/Body/Pupil").Value;

            var blackLatexMaskTexture = Mod.Assets.Request<Texture2D>("Assets/Textures/Body/HeadBlackLatex1").Value;

            // Base NPC draw position (matches vanilla)
            Vector2 drawPos = NPC.Center - screenPos;

            // Remove gfxOffY so it doesn't get applied twice
            drawPos.Y += NPC.gfxOffY;

            drawPos.Y += 8;

            SpriteEffects effects = NPC.spriteDirection == 1
                ? SpriteEffects.FlipHorizontally
                : SpriteEffects.None;

            var walkCycleYOffset = 0;
            var tailRotation = 0.3f;
            var armLRotation = 0.3f;
            var armRotation = -0.1f;
            var legLRotation = 0f;
            var legRRotation = 0f;

            int frame = NPC.frame.Y / 59;

            if (frame == 0)
            {
                walkCycleYOffset = 0;
                armLRotation = 0.6f;
                armRotation = -0.4f;
                tailRotation = 0.3f;
                legLRotation = 0;
                legRRotation = 0;
            }

            if (frame == 1)
            {
                walkCycleYOffset = -1;
                armLRotation = 0.3f;
                armRotation = -0.1f;
                tailRotation = 0.6f;
                legLRotation = 0.375f;
                legRRotation = -0.375f;
            }

            if (frame == 2)
            {
                walkCycleYOffset = -2;
                armLRotation = 0f;
                armRotation = 0.2f;
                tailRotation = 0.3f;
                legLRotation = 0.75f;
                legRRotation = -0.75f;
            }

            if (frame == 3)
            {
                walkCycleYOffset = -1;
                armLRotation = 0.3f;
                armRotation = -0.1f;
                tailRotation = 0f;
                legLRotation = 0.375f;
                legRRotation = -0.375f;
            }
            
            tailRotation *= NPC.spriteDirection;
            armLRotation *= NPC.spriteDirection;
            armRotation *= NPC.spriteDirection;
            legLRotation *= NPC.spriteDirection;
            legRRotation *= NPC.spriteDirection;

            drawPos.Y += walkCycleYOffset;

            var hideEars = headIndex > 12;
            var hideTail = legsIndex == 10; 

            // Tail
            if (!hideTail)
                DrawTexture(spriteBatch, tailTexture, drawPos, Vector2.Zero, drawColor, effects, tailRotation);
            // Left Leg
            DrawTexture(spriteBatch, legLTexture, drawPos, Vector2.Zero, drawColor, effects, legLRotation);
            // Left arm
            DrawTexture(spriteBatch, armLTexture, drawPos, new Vector2(0 + 10, -32 - 13), drawColor, effects, armLRotation);
            // Right Leg
            DrawTexture(spriteBatch, legRTexture, drawPos, Vector2.Zero, drawColor, effects, legRRotation);
            // Torso
            DrawTexture(spriteBatch, bodyTexture, drawPos, new Vector2(0, -32), drawColor, effects, 0);
            // Fluff
            DrawTexture(spriteBatch, fluffTexture, drawPos, new Vector2(0, -56), drawColor2, effects, 0);//-48
            // Head
            if (secondHead)
            {
                DrawHead(changedNPC, spriteBatch, headTexture, earsTexture, eyeTexture, hairTexture, pupilTexture, drawPos, new Vector2(8, 0), drawColor, drawColor2, drawColor3, lightingColor, effects, 0.15f, hideEars);
                DrawHead(changedNPC, spriteBatch, headTexture, earsTexture, eyeTexture, hairTexture, pupilTexture, drawPos, new Vector2(-8, 0), drawColor, drawColor2, drawColor3, lightingColor, effects, -0.15f, hideEars);
            }
            else
            {
                // Head
                DrawHead(changedNPC, spriteBatch, headTexture, earsTexture, eyeTexture, hairTexture, pupilTexture, drawPos, Vector2.Zero, drawColor, drawColor2, drawColor3, lightingColor, effects, 0, hideEars);
            }
            
            // Right arm
            DrawTexture(spriteBatch, armRTexture, drawPos, new Vector2(0 - 11, -32 - 14), drawColor, effects, armRotation);
 

            return false;
        }
    }
}
