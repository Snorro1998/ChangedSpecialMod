using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Dusts;
using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using ChangedSpecialMod.Content.Projectiles;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Events;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace ChangedSpecialMod.Content.NPCs
{
    [AutoloadHead]
    public class Prototype : ModNPC
    {
        // Everything starting with state needs to be figured out still
        private enum ActionState
        {
            State2 = 2,
            State3 = 3,
            State4 = 4,
            Sitting = 5,
            State6 = 6,
            State7 = 7,
            State9 = 9,
            State10 = 10,
            State11 = 11,
            State12 = 12,
            State13 = 13,
            State14 = 14,
            State15 = 15,
            State16 = 16,
            State17 = 17,
            State18 = 18,
            State19 = 19,
            State20 = 20,
            State21 = 21,
            State22 = 22,
            State23 = 23,
            State24 = 24,
            State25 = 25,
            WaterPlants = 30,
            State1001 = 1001
        }

        public const string ShopName = "Shop";
        public const string ShopName2 = "Second shop";
        private static Profiles.StackedNPCProfile NPCProfile;

        // This should add up to 1 or it will break (so don't use something like 0.3)
        public float animationSpeed = 1.0f;

        public static DialogueObject DialogueNormal = new DialogueObject(
            "Prototype",
            new List<string>
            {                
                "Emotion1",
                "Emotion2",
                "Emotion3",
                "Emotion4",
                "Emotion5",
                "Emotion6"
            }
        );

        public static DialogueObject DialogueInjured = new DialogueObject(
            "Prototype.Injured",
            new List<string>
            {
                "Injured1",
                "Injured2",
                "Injured3"
            }
        );

        public static DialogueObject DialogueNearlyDead = new DialogueObject(
            "Prototype.NearlyDead",
            new List<string>
            {
                "NearlyDead1",
                "NearlyDead2",
                "NearlyDead3"
            }
        );

        private Color lightColor = Color.Blue;

        // Disco
        private float rgbSpeed = 1.0f / (2 * 60.0f);
        private float rgbTime = 0;
        private Color rgbColorFrom = Color.Red;
        private Color rgbColorTo = Color.Blue;

        public ref float AIState => ref NPC.ai[0];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 31;
            NPCID.Sets.ExtraFramesCount[Type] = 15;
            NPCID.Sets.AttackFrameCount[Type] = 4;
            NPCID.Sets.DangerDetectRange[Type] = 700;
            NPCID.Sets.AttackType[Type] = 0;
            NPCID.Sets.AttackTime[Type] = 90;
            NPCID.Sets.AttackAverageChance[Type] = 30;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness
                //.SetBiomeAffection<CityRuinsSurfaceBiome>(AffectionLevel.Love)
                .SetBiomeAffection<BlackLatexSurfaceBiome>(AffectionLevel.Love)
                .SetBiomeAffection<WhiteLatexSurfaceBiome>(AffectionLevel.Love)
                .SetBiomeAffection<OceanBiome>(AffectionLevel.Hate)

                // Happy boy doesn't hate anybody
                .SetNPCAffection(NPCID.Cyborg, AffectionLevel.Love)
                .SetNPCAffection(NPCID.Steampunker, AffectionLevel.Like);
        }


        public override int? PickEmote(Player closestPlayer, List<int> emoteList, WorldUIAnchor otherAnchor)
        {
            var changedNPC = NPC.Changed();
            emoteList = changedNPC.GetEmoteList(NPC, closestPlayer, emoteList);
            var index = ChangedUtils.MainRandNext(0, emoteList.Count);
            return emoteList[index];
        }

        public override bool UsesPartyHat()
        {
            return false;
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            // Can't use custom AI -1, because town npcs only check for NPCs with passive AI when trying to sit
            // Meaning if you don't use Passive AI here, they can sit down on his lap
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 25;
            NPC.defense = 15;
            NPC.lifeMax = 400;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            AnimationType = NPCID.None;
            SpawnModBiomes = new int[] { ModContent.GetInstance<CityRuinsSurfaceBiome>().Type };

            var changedNPC = NPC.Changed();
            changedNPC.DefaultHitEffect = true;
            changedNPC.GooType = GooType.None;

            changedNPC.RemoveHat(ItemID.JackOLanternMask);
            changedNPC.AddHat(new HatStruct(ItemID.JackOLanternMask, HatType.Halloween, new int[] { 0, 6 }));
            changedNPC.RemoveHat(ItemID.GarlandHat);
            changedNPC.AddHat(new HatStruct(ItemID.GarlandHat, HatType.Valentine, new int[] { 0, 1 }));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.Prototype.Description"))
            });
        }

		public override bool CanTownNPCSpawn(int numTownNPCs) 
		{
			return DownedBossSystem.DownedBehemoth;
		}

        public override ITownNPCProfile TownNPCProfile() 
        {
			return NPCProfile;
		}

		public override List<string> SetNPCNameList() 
        {
			return new List<string>() 
            {
                "A-126",    // Change the first letter to an e and swap the second and last letter
                "4MM13",    // Ammy
                "GH44",     // Super Bubsy cheatcode cuz why not
                "0R4-N93",  // Orange
                "XJ-9",     // Jenny Wakeman, also a blue robot
                "H4Y-D3N",  // Hayden
                "SH1-Z1"    // Shizi
            };
		}

		public override string GetChat() 
		{
            var changedNPC = NPC.Changed();
            var keyWords = changedNPC.GetChatKeyWords();
            DialogueObject dialogue = DialogueNormal;

            if (NPC.life <= 0.3f * NPC.lifeMax)
                dialogue = DialogueNearlyDead;
            else if (NPC.life <= 0.6f * NPC.lifeMax)
                dialogue = DialogueInjured;

            return dialogue.GetDialogue(keyWords);
		}

		public override void SetChatButtons(ref string button, ref string button2) 
		{
            var shopNamePath = "Mods.ChangedSpecialMod.ShopNames";
            button = Language.GetTextValue($"{shopNamePath}.FruitAndHerbs");
            button2 = Language.GetTextValue($"{shopNamePath}.MusicBoxes");
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) 
		{
			if (firstButton) 
            {
				shop = ShopName;
			}
			else 
            {
                shop = ShopName2;
            }
		}

		public override void AddShops() 
        {
			new NPCShop(Type, ShopName)
                // Fruit
                .Add(ItemID.Apple)
                .Add(ItemID.Apricot)
                .Add(ItemID.Banana)
                .Add(ItemID.BlackCurrant)
                .Add<Items.Orange>()
                .Add(ItemID.BloodOrange)
                .Add(ItemID.Cherry)
                .Add(ItemID.Coconut)
                .Add(ItemID.Elderberry)
                .Add(ItemID.Grapefruit)
                .Add(ItemID.Lemon)
                .Add(ItemID.Mango)
                .Add(ItemID.Peach)
                .Add(ItemID.Pineapple)
                .Add(ItemID.Plum)
                .Add(ItemID.Pomegranate)
                .Add(ItemID.Rambutan)
                .Add(ItemID.SpicyPepper)

                // Herbs
                .Add(ItemID.Daybloom)
                .Add(ItemID.Moonglow)
                .Add(ItemID.Blinkroot)
                .Add(ItemID.Deathweed)
                .Add(ItemID.Waterleaf)
                .Add(ItemID.Fireblossom)
                .Add(ItemID.Shiverthorn)
                .Register();

            new NPCShop(Type, ShopName2)
                .Add(ModContent.ItemType<MusicBoxBlackLatexZone1>())
                .Add(ModContent.ItemType<MusicBoxBlackLatexZone2>())
                .Add(ModContent.ItemType<MusicBoxCrystalZone>())
                .Add(ModContent.ItemType<MusicBoxWhiteLatexZone>())
                .Add(ModContent.ItemType<MusicBoxLabSlow>())
                .Add(ModContent.ItemType<MusicBoxLab>())
                .Add(ModContent.ItemType<MusicBoxLibrary>())
                .Add(ModContent.ItemType<MusicBoxGreenhouse>())
                .Add(ModContent.ItemType<MusicBoxPuro>())
                .Add(ModContent.ItemType<MusicBoxPuroDance>())
                .Add(ModContent.ItemType<MusicBoxVents>())
                .Add(ModContent.ItemType<MusicBoxHappyBirthday>())

                .Add(ModContent.ItemType<MusicBoxWhiteTail>())
                .Add(ModContent.ItemType<MusicBoxWolfKing>())
                .Add(ModContent.ItemType<MusicBoxBehemoth>())
                .Register();
        }

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WateringCan>()));
		}

		public override bool CanGoToStatue(bool toKingStatue)
		{
			return toKingStatue;
        }

		// Create a square of pixels around the NPC on teleport.
		public void StatueTeleport() {
			for (int i = 0; i < 30; i++) {
				Vector2 position = Main.rand.NextVector2Square(-20, 21);
				if (Math.Abs(position.X) > Math.Abs(position.Y)) {
					position.X = Math.Sign(position.X) * 20;
				}
				else {
					position.Y = Math.Sign(position.Y) * 20;
				}

				Dust.NewDustPerfect(NPC.Center + position, ModContent.DustType<Sparkle>(), Vector2.Zero).noGravity = true;
			}
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback) 
        {
			damage = NPC.damage;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) 
        {
			cooldown = 30;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay) 
        {
			projType = ModContent.ProjectileType<PotProjectile>();
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) 
        {
			multiplier = 12f;
			randomOffset = 2f;
		}

        private void UpdateHatPosition(int frameHeight)
        {
            /*
            standing    0 2 3 4 5 9 10 11 12 16 17 24 25
            wide legs   1 13 14 15
            leg frwd    6 7 8
            sitting     18 19 20 21 22 23
            throw       26 27 28 29
            */

            var changedNPC = NPC.Changed();
            var frame = NPC.frame;
            var frameIndex = frame.Top / frameHeight;
            var hatYOffset = -36;

            if (frameIndex == 6 || frameIndex == 7 || frameIndex == 8)
            {
                hatYOffset -= 2;
            }

            changedNPC.HatYOffset = hatYOffset;
        }

        private void DrawPartyRadioEyes(SpriteBatch spriteBatch, Vector2 drawPos, Color drawColor, SpriteEffects effects)
        {
            var frameIndexRadio = (int)((Main.GlobalTimeWrappedHourly * 3) % 3);
            var nFramesRadio = 3;
            Texture2D textureRadio = Mod.Assets.Request<Texture2D>("Content/NPCs/Prototype_Radio").Value;
            int frameHeight = textureRadio.Height / nFramesRadio;
            Rectangle frameRadio = new Rectangle(0, frameIndexRadio * frameHeight, textureRadio.Width, frameHeight);

            // Mmm spaghetti
            if (frameHeight > 0)
            {
                var frameIndex = NPC.frame.Top / frameHeight;
                if (frameIndex == 6 || frameIndex == 7 || frameIndex == 8)
                {
                    drawPos.Y -= 2;
                }
                else if (frameIndex == 24)
                {
                    drawPos.Y += 2;
                }
            }

            spriteBatch.Draw(
                textureRadio,
                drawPos,
                frameRadio,
                drawColor,
                NPC.rotation,
                new Vector2(textureRadio.Width / 2, textureRadio.Height / nFramesRadio / 2),
                NPC.scale,
                effects,
                0f
            );
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D textureBase = Mod.Assets.Request<Texture2D>("Content/NPCs/Prototype_Base").Value;
            Texture2D textureLight = Mod.Assets.Request<Texture2D>("Content/NPCs/Prototype_Light").Value;
            Vector2 drawPos = NPC.Top - screenPos;
            drawPos.Y += NPC.gfxOffY;

            SpriteEffects effects = NPC.direction == 1
                ? SpriteEffects.FlipHorizontally
                : SpriteEffects.None;

            var nFrames = Main.npcFrameCount[Type];

            // Body
            spriteBatch.Draw(
                textureBase,
                drawPos,
                NPC.frame,
                drawColor,
                NPC.rotation,
                new Vector2(textureBase.Size().X / 2, textureBase.Size().Y / nFrames / 2),
                NPC.scale,
                effects,
                0f
            );

            // Lights
            spriteBatch.Draw(
                textureLight,
                drawPos,
                NPC.frame,
                lightColor,
                NPC.rotation,
                new Vector2(textureBase.Size().X / 2, textureBase.Size().Y / nFrames / 2),
                NPC.scale,
                effects,
                0f
            );

            if (BirthdayParty.PartyIsUp)
            {
                DrawPartyRadioEyes(spriteBatch, drawPos, drawColor, effects);
            }

            return false;
        }

        private void UpdateLightColor()
        {
            // Light blue
            lightColor = new Color(0, 160, 255);

            if (BirthdayParty.PartyIsUp)
            {
                rgbTime += rgbSpeed;
                if (rgbTime >= 1)
                {
                    rgbTime = 0;
                    rgbColorFrom = rgbColorTo;
                    rgbColorTo = ChangedUtils.Choose(Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Purple, Color.White);
                }

                lightColor = Color.Lerp(rgbColorFrom, rgbColorTo, rgbTime);
            }

            else if (Main.bloodMoon)
            {
                lightColor = Color.Crimson;
            }
            else if (Main.eclipse)
            {
                lightColor = Color.Orange;
            }

            float speed = 2f;
            float amplitude = 0.15f;
            float mean = 0.65f;
            float alpha = (float)Math.Sin(Main.GlobalTimeWrappedHourly * speed) * amplitude + mean;
            lightColor *= alpha;
        }

        private void DetermineHat(ChangedNPC changedNPC)
        {
            var hatId = -1;
            string modHat = null;

            if (BirthdayParty.PartyIsUp)
            {
                hatId = ItemID.PartyHat;
            }
            else if (Main.invasionType == InvasionID.PirateInvasion)
            {
                hatId = ItemID.BuccaneerBandana;
            }
            else
            {
                switch (SeasonSystem.season)
                {
                    case SeasonalEvent.Birthday:
                        hatId = ItemID.PartyHat;
                        break;
                    case SeasonalEvent.Valentine:
                        hatId = ItemID.GarlandHat;
                        break;
                    case SeasonalEvent.Oktoberfest:
                        hatId = ItemID.BallaHat;
                        break;
                    case SeasonalEvent.Halloween:
                        hatId = ItemID.JackOLanternMask;
                        break;
                    case SeasonalEvent.XMas:
                        hatId = ItemID.SantaHat;
                        break;
                }
            }

            if (hatId != -1 || modHat != null)
            {
                if (modHat != null)
                {
                    changedNPC.SetHat(modHat);
                }
                else
                {
                    changedNPC.SetHat(hatId);
                }
            }
            else
            {
                changedNPC.RemoveHat();
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            UpdateLightColor();
            Lighting.AddLight(NPC.Center, lightColor.ToVector3());
            var changedNPC = NPC.Changed();
            DetermineHat(changedNPC);
            changedNPC.PostDrawExtra(NPC, spriteBatch, screenPos, drawColor);
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }

        public bool IsGrounded() => NPC.velocity.Y == 0f;

        public override bool PreAI()
        {
            AI_TownEntity.AI_007_TownEntities(NPC);
            return false;
        }

        #region StateAnimations
        private void FindFrame23(int frameHeight, int nFramesWithoutAttackAnim)
        {
            NPC.frameCounter += animationSpeed;
            int num319 = NPC.frame.Y / frameHeight;
            int num320 = nFramesWithoutAttackAnim - num319;
            if ((uint)(num320 - 1) > 1u && (uint)(num320 - 4) > 1u && num319 != 0)
            {
                NPC.frame.Y = 0;
                NPC.frameCounter = 0.0;
            }
            int frameNumber0 = 0;
            frameNumber0 = ((!(NPC.frameCounter < 6.0)) ? (nFramesWithoutAttackAnim - 4) : (nFramesWithoutAttackAnim - 5));
            if (NPC.ai[1] < 6f)
            {
                frameNumber0 = nFramesWithoutAttackAnim - 5;
            }
            NPC.frame.Y = 0;
            //NPC.frame.Y = frameHeight * frameNumber0;
        }

        private void FindFrame202122(int frameHeight)
        {
            int currentFrame = NPC.frame.Y / frameHeight;
            switch ((int)NPC.ai[0])
            {
                case 20:
                    if (NPC.ai[1] > 30f && (currentFrame < 23 || currentFrame > 27))
                    {
                        currentFrame = 23;
                    }
                    if (currentFrame > 0)
                    {
                        NPC.frameCounter += animationSpeed;
                    }
                    if (NPC.frameCounter > 4.0)
                    {
                        NPC.frameCounter = 0.0;
                        currentFrame++;
                        if (currentFrame > 26 && NPC.ai[1] > 30f)
                        {
                            currentFrame = 24;
                        }
                        if (currentFrame > 27)
                        {
                            currentFrame = 0;
                        }
                    }
                    break;
                case 21:
                    if (NPC.ai[1] > 30f && (currentFrame < 17 || currentFrame > 22))
                    {
                        currentFrame = 17;
                    }
                    if (currentFrame > 0)
                    {
                        NPC.frameCounter += animationSpeed;
                    }
                    if (NPC.frameCounter > 4.0)
                    {
                        NPC.frameCounter = 0.0;
                        currentFrame++;
                        if (NPC.ai[1] > 30f && currentFrame > 21)
                        {
                            currentFrame = 18;
                        }
                        if (currentFrame > 22)
                        {
                            currentFrame = 0;
                        }
                    }
                    break;
                case 22:
                    if (NPC.ai[1] > 30f && (currentFrame < 17 || currentFrame > 27))
                    {
                        currentFrame = 17;
                    }
                    if (currentFrame > 0)
                    {
                        NPC.frameCounter += animationSpeed;
                    }
                    if (NPC.frameCounter > 4.0)
                    {
                        NPC.frameCounter = 0.0;
                        currentFrame++;
                        if (currentFrame > 27)
                        {
                            currentFrame = ((!(NPC.ai[1] <= 30f)) ? 22 : 20);
                        }
                        else if (NPC.ai[1] <= 30f && currentFrame == 22)
                        {
                            currentFrame = 0;
                        }
                        else if (NPC.ai[1] > 30f && currentFrame > 19 && currentFrame < 22)
                        {
                            currentFrame = 22;
                        }
                    }
                    break;
            }
            NPC.frame.Y = 0;
            //NPC.frame.Y = frameNumber * frameHeight;
        }

        private void FindFrame2(int frameHeight, int nFramesWithoutAttackAnim)
        {
            NPC.frameCounter += animationSpeed;
            if (NPC.frame.Y / frameHeight == nFramesWithoutAttackAnim - 1 && NPC.frameCounter >= 5.0)
            {
                NPC.frame.Y = 0;
                NPC.frameCounter = 0.0;
            }
            else if (NPC.frame.Y / frameHeight == 0 && NPC.frameCounter >= 40.0)
            {
                NPC.frame.Y = 0;
                //NPC.frame.Y = frameHeight * (nFramesWithoutAttackAnim - 1);
                NPC.frameCounter = 0.0;
            }
            else if (NPC.frame.Y != 0 && NPC.frame.Y != frameHeight * (nFramesWithoutAttackAnim - 1))
            {
                NPC.frame.Y = 0;
                NPC.frameCounter = 0.0;
            }
        }

        private void FindFrame11(int frameHeight, int nFramesWithoutAttackAnim)
        {
            NPC.frameCounter += animationSpeed;
            if (NPC.frame.Y / frameHeight == nFramesWithoutAttackAnim - 1 && NPC.frameCounter >= 50.0)
            {
                if (NPC.frameCounter == 50.0)
                {
                    int num324 = Main.rand.Next(4);
                    for (int m = 0; m < 3 + num324; m++)
                    {
                        int num325 = Dust.NewDust(NPC.Center + Vector2.UnitX * -NPC.direction * 8f - Vector2.One * 5f + Vector2.UnitY * 8f, 3, 6, 216, -NPC.direction, 1f);
                        Main.dust[num325].velocity /= 2f;
                        Main.dust[num325].scale = 0.8f;
                    }
                }
                if (NPC.frameCounter >= 100.0 && Main.rand.Next(20) == 0)
                {
                    NPC.frame.Y = 0;
                    NPC.frameCounter = 0.0;
                }
            }
            else if (NPC.frame.Y / frameHeight == 0 && NPC.frameCounter >= 20.0)
            {
                NPC.frame.Y = 0;
                NPC.frameCounter = 0.0;
                EmoteBubble.NewBubble(89, new WorldUIAnchor(NPC), 90);
            }
            else if (NPC.frame.Y != 0 && NPC.frame.Y != frameHeight * (nFramesWithoutAttackAnim - 1))
            {
                NPC.frame.Y = 0;
                NPC.frameCounter = 0.0;
            }
        }

        private void FindFrameWaterPlants(int frameHeight)
        {
            var frame = 24;
            NPC.frame.Y = frameHeight * frame;
            var waterParticle = Dust.dustWater();
            var screenPos = Main.screenPosition;
            var position = NPC.Center + new Vector2(NPC.direction * 25f, -8);
            int particleID = Dust.NewDust(position, 0, 0, waterParticle, 0, 5, 0, default(Color), 1.2f);
            if (particleID != -1)
            {
                Main.dust[particleID].scale = (0.7f + Main.rand.NextFloat() * 0.3f) * 1.5f;
            }
        }

        private void FindFrameSitting(int frameHeight)
        {
            //frame     begin   end
            //0         00      60
            //1         60      75
            //2         75      90
            //3         90      150
            //4         150     165
            //5         165     180

            var frame = 0;

            if (NPC.frameCounter >= 160)
                NPC.frameCounter = 0;

            if (NPC.frameCounter < 60)
                frame = 18;
            else if (NPC.frameCounter >= 60 && NPC.frameCounter < 70)
                frame = 19;
            else if (NPC.frameCounter >= 70 && NPC.frameCounter < 80)
                frame = 20;
            else if (NPC.frameCounter >= 80 && NPC.frameCounter < 140)
                frame = 21;
            else if (NPC.frameCounter >= 140 && NPC.frameCounter < 150)
                frame = 22;
            else if (NPC.frameCounter >= 150 && NPC.frameCounter < 160)
                frame = 23;

            NPC.frame.Y = frameHeight * frame;
            NPC.frameCounter += animationSpeed;
        }

        private void FindFrame6(int frameHeight, int nFramesWithoutAttackAnim)
        {
            NPC.frameCounter += animationSpeed;
            int num327 = NPC.frame.Y / frameHeight;
            int num328 = nFramesWithoutAttackAnim - num327;
            if ((uint)(num328 - 1) > 1u && (uint)(num328 - 4) > 1u && num327 != 0)
            {
                NPC.frame.Y = 0;
                NPC.frameCounter = 0.0;
            }
            int num329 = 0;
            num329 = ((!(NPC.frameCounter < 10.0)) ? ((NPC.frameCounter < 16.0) ? (nFramesWithoutAttackAnim - 5) : ((NPC.frameCounter < 46.0) ? (nFramesWithoutAttackAnim - 4) : ((NPC.frameCounter < 60.0) ? (nFramesWithoutAttackAnim - 5) : ((!(NPC.frameCounter < 66.0)) ? ((NPC.frameCounter < 72.0) ? (nFramesWithoutAttackAnim - 5) : ((NPC.frameCounter < 102.0) ? (nFramesWithoutAttackAnim - 4) : ((NPC.frameCounter < 108.0) ? (nFramesWithoutAttackAnim - 5) : ((!(NPC.frameCounter < 114.0)) ? ((NPC.frameCounter < 120.0) ? (nFramesWithoutAttackAnim - 5) : ((NPC.frameCounter < 150.0) ? (nFramesWithoutAttackAnim - 4) : ((NPC.frameCounter < 156.0) ? (nFramesWithoutAttackAnim - 5) : ((!(NPC.frameCounter < 162.0)) ? ((NPC.frameCounter < 168.0) ? (nFramesWithoutAttackAnim - 5) : ((NPC.frameCounter < 198.0) ? (nFramesWithoutAttackAnim - 4) : ((NPC.frameCounter < 204.0) ? (nFramesWithoutAttackAnim - 5) : ((!(NPC.frameCounter < 210.0)) ? ((NPC.frameCounter < 216.0) ? (nFramesWithoutAttackAnim - 5) : ((NPC.frameCounter < 246.0) ? (nFramesWithoutAttackAnim - 4) : ((NPC.frameCounter < 252.0) ? (nFramesWithoutAttackAnim - 5) : ((!(NPC.frameCounter < 258.0)) ? ((NPC.frameCounter < 264.0) ? (nFramesWithoutAttackAnim - 5) : ((NPC.frameCounter < 294.0) ? (nFramesWithoutAttackAnim - 4) : ((NPC.frameCounter < 300.0) ? (nFramesWithoutAttackAnim - 5) : 0))) : 0)))) : 0)))) : 0)))) : 0)))) : 0)))) : 0);
            if (num329 == nFramesWithoutAttackAnim - 4 && num327 == nFramesWithoutAttackAnim - 5)
            {
                Vector2 vector5 = NPC.Center + new Vector2(10 * NPC.direction, -4f);
                for (int n = 0; n < 8; n++)
                {
                    int num330 = Main.rand.Next(139, 143);
                    int num331 = Dust.NewDust(vector5, 0, 0, num330, NPC.velocity.X + (float)NPC.direction, NPC.velocity.Y - 2.5f, 0, default(Color), 1.2f);
                    Main.dust[num331].velocity.X += (float)NPC.direction * 1.5f;
                    Main.dust[num331].position -= new Vector2(4f);
                    Main.dust[num331].velocity *= 2f;
                    Main.dust[num331].scale = 0.7f + Main.rand.NextFloat() * 0.3f;
                }
            }
            NPC.frame.Y = 0;

            if (NPC.frameCounter >= 300.0)
            {
                NPC.frameCounter = 0.0;
            }
        }

        private void FindFrame9(int frameHeight, int nFramesWithoutAttackAnim)
        {
            NPC.frameCounter += animationSpeed;
            int num335 = NPC.frame.Y / frameHeight;
            int num336 = nFramesWithoutAttackAnim - num335;
            if ((uint)(num336 - 1) > 1u && (uint)(num336 - 4) > 1u && num335 != 0)
            {
                NPC.frame.Y = 0;
                NPC.frameCounter = 0.0;
            }
            int num337 = 0;
            num337 = ((!(NPC.frameCounter < 10.0)) ? ((!(NPC.frameCounter < 16.0)) ? (nFramesWithoutAttackAnim - 4) : (nFramesWithoutAttackAnim - 5)) : 0);
            if (NPC.ai[1] < 16f)
            {
                num337 = nFramesWithoutAttackAnim - 5;
            }
            if (NPC.ai[1] < 10f)
            {
                num337 = 0;
            }
            NPC.frame.Y = 0;
        }

        private void FindFrame18(int frameHeight, int nFramesWithoutAttackAnim)
        {
            NPC.frameCounter += animationSpeed;
            int num338 = NPC.frame.Y / frameHeight;
            int num339 = nFramesWithoutAttackAnim - num338;
            if ((uint)(num339 - 1) > 1u && (uint)(num339 - 4) > 1u && num338 != 0)
            {
                NPC.frame.Y = 0;
                NPC.frameCounter = 0.0;
            }
            int frameNumber1 = 0;
            if (NPC.frameCounter < 10.0)
            {
                frameNumber1 = 0;
            }
            else if (NPC.frameCounter < 16.0)
            {
                frameNumber1 = nFramesWithoutAttackAnim - 1;
            }
            else
            {
                frameNumber1 = nFramesWithoutAttackAnim - 2;
            }
            if (NPC.ai[1] < 16f)
            {
                frameNumber1 = nFramesWithoutAttackAnim - 1;
            }
            if (NPC.ai[1] < 10f)
            {
                frameNumber1 = 0;
            }
            frameNumber1 = Main.npcFrameCount[NPC.type] - 2;
            NPC.frame.Y = 0;
        }

        private void FindFrame1013(int frameHeight, int nFramesWithoutAttackAnim)
        {
            NPC.frameCounter += animationSpeed;
            int num341 = NPC.frame.Y / frameHeight;
            if ((uint)(num341 - nFramesWithoutAttackAnim) > 3u && num341 != 0)
            {
                NPC.frame.Y = 0;
                NPC.frameCounter = 0.0;
            }
            int num342 = 10;
            int num343 = 6;
            int frameNumber2 = 0;
            frameNumber2 = ((!(NPC.frameCounter < (double)num342)) ? ((NPC.frameCounter < (double)(num342 + num343)) ? nFramesWithoutAttackAnim : ((NPC.frameCounter < (double)(num342 + num343 * 2)) ? (nFramesWithoutAttackAnim + 1) : ((NPC.frameCounter < (double)(num342 + num343 * 3)) ? (nFramesWithoutAttackAnim + 2) : ((NPC.frameCounter < (double)(num342 + num343 * 4)) ? (nFramesWithoutAttackAnim + 3) : 0)))) : 0);
            NPC.frame.Y = frameHeight * frameNumber2;
        }

        private void FindFrame15(int frameHeight, int nFramesWithoutAttackAnim)
        {
            NPC.frameCounter += animationSpeed;
            int num345 = NPC.frame.Y / frameHeight;
            if ((uint)(num345 - nFramesWithoutAttackAnim) > 3u && num345 != 0)
            {
                NPC.frame.Y = 0;
                NPC.frameCounter = 0.0;
            }
            float num346 = NPC.ai[1] / (float)NPCID.Sets.AttackTime[NPC.type];
            int frameNumber3 = 0;
            frameNumber3 = ((num346 > 0.65f) ? nFramesWithoutAttackAnim : ((num346 > 0.5f) ? (nFramesWithoutAttackAnim + 1) : ((num346 > 0.35f) ? (nFramesWithoutAttackAnim + 2) : ((num346 > 0f) ? (nFramesWithoutAttackAnim + 3) : 0))));
            NPC.frame.Y = frameHeight * frameNumber3;
        }

        private void FindFrame25(int frameHeight)
        {
            NPC.frame.Y = frameHeight;
        }

        private void FindFrame12(int frameHeight, int nFramesWithoutAttackAnim)
        {
            NPC.frameCounter += animationSpeed;
            int num348 = NPC.frame.Y / frameHeight;
            if ((uint)(num348 - nFramesWithoutAttackAnim) > 4u && num348 != 0)
            {
                NPC.frame.Y = 0;
                NPC.frameCounter = 0.0;
            }
            int frameNumber4 = nFramesWithoutAttackAnim + NPC.GetShootingFrame(NPC.ai[2]);
            NPC.frame.Y = frameHeight * frameNumber4;
        }

        private void FindFrame1424(int frameHeight, int nFramesWithoutAttackAnim)
        {
            NPC.frameCounter += animationSpeed;
            int num350 = NPC.frame.Y / frameHeight;
            if ((uint)(num350 - nFramesWithoutAttackAnim) > 1u && num350 != 0)
            {
                NPC.frame.Y = 0;
                NPC.frameCounter = 0.0;
            }
            int num351 = 12;
            int num352 = ((NPC.frameCounter % (double)num351 * 2.0 < (double)num351) ? nFramesWithoutAttackAnim : (nFramesWithoutAttackAnim + 1));
            NPC.frame.Y = frameHeight * num352;
            if (AIState == (float)ActionState.State24)
            {
                if (NPC.frameCounter == 60.0)
                {
                    EmoteBubble.NewBubble(87, new WorldUIAnchor(NPC), 60);
                }
                if (NPC.frameCounter == 150.0)
                {
                    EmoteBubble.NewBubble(3, new WorldUIAnchor(NPC), 90);
                }
                if (NPC.frameCounter >= 240.0)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        private void FindFrame1001()
        {
            NPC.frame.Y = 0;
            NPC.frameCounter = 0.0;
        }
        #endregion

        private void FindFrameStandingStillAndWalking(int frameHeight, int extraFrames)
        {
            // Standing still
            if (NPC.velocity.X == 0f)
            {
                NPC.frame.Y = 0;
                NPC.frameCounter = 0.0;
            }
            // Walking normally
            else
            {
                int num375 = 6;
                NPC.frameCounter += Math.Abs(NPC.velocity.X) * 2f;
                NPC.frameCounter += animationSpeed;
                int num376 = frameHeight * 2;

                if (NPC.frame.Y < num376)
                {
                    NPC.frame.Y = num376;
                }
                if (NPC.frameCounter > (double)num375)
                {
                    NPC.frame.Y += frameHeight;
                    NPC.frameCounter = 0.0;
                }
                if (NPC.frame.Y / frameHeight >= Main.npcFrameCount[NPC.type] - extraFrames)
                {
                    NPC.frame.Y = num376;
                }
            }
        }

        // This is the vanilla code heavily cleaned up and with unnecessary conditions removed (like pets and town slimes)
        // The frame logic for talking and shaking hands is still here but disabled, because it got messed up after adding more frames for sitting
        public override void FindFrame(int frameHeight)
        {
            int extraFrames = (NPC.isLikeATownNPC ? NPCID.Sets.ExtraFramesCount[NPC.type] : 0);
            if (IsGrounded())
            {
                // Check if direction can be any other value. If so, this can be simplified
                if (NPC.direction == 1)
                {
                    NPC.spriteDirection = 1;
                }
                if (NPC.direction == -1)
                {
                    NPC.spriteDirection = -1;
                }

                int nFramesWithoutAttackAnim = Main.npcFrameCount[NPC.type] - NPCID.Sets.AttackFrameCount[NPC.type];

                var specialStates = new float[]
                {
                    (float)ActionState.State3,
                    (float)ActionState.State4,
                    (float)ActionState.State7,
                    (float)ActionState.State16,
                    (float)ActionState.State17,
                    (float)ActionState.State19
                };

                if (!specialStates.Contains(AIState))
                {
                    switch(AIState)
                    {
                        case (float)ActionState.State23:
                            FindFrame23(frameHeight, nFramesWithoutAttackAnim);
                            break;
                        case (float)ActionState.State20:
                        case (float)ActionState.State21:
                        case (float)ActionState.State22:
                            FindFrame202122(frameHeight);
                            break;
                        case (float)ActionState.State2:
                            FindFrame2(frameHeight, nFramesWithoutAttackAnim);
                            break;
                        case (float)ActionState.State11:
                            FindFrame11(frameHeight, nFramesWithoutAttackAnim);
                            break;
                        case (float)ActionState.WaterPlants:
                            FindFrameWaterPlants(frameHeight);
                            break;
                        case (float)ActionState.Sitting:
                            FindFrameSitting(frameHeight);
                            break;
                        case (float)ActionState.State6:
                            FindFrame6(frameHeight, nFramesWithoutAttackAnim);
                            break;
                        case (float)ActionState.State9:
                            FindFrame9(frameHeight, nFramesWithoutAttackAnim);
                            break;
                        case (float)ActionState.State18:
                            FindFrame18(frameHeight, nFramesWithoutAttackAnim);
                            break;
                        case (float)ActionState.State10:
                        case (float)ActionState.State13:
                            FindFrame1013(frameHeight, nFramesWithoutAttackAnim);
                            break;
                        case (float)ActionState.State15:
                            FindFrame15(frameHeight, nFramesWithoutAttackAnim);
                            break;
                        case (float)ActionState.State25:
                            FindFrame25(frameHeight);
                            break;
                        case (float)ActionState.State12:
                            FindFrame12(frameHeight, nFramesWithoutAttackAnim);
                            break;
                        case (float)ActionState.State14:
                        case (float)ActionState.State24:
                            FindFrame1424(frameHeight, nFramesWithoutAttackAnim);
                            break;
                        case (float)ActionState.State1001:
                            FindFrame1001();
                            break;
                        default:
                            FindFrameStandingStillAndWalking(frameHeight, extraFrames);
                            break;
                    }
                }
                else
                {
                    if ((AIState == (float)ActionState.State7 || AIState == (float)ActionState.State19) && !NPCID.Sets.IsTownPet[NPC.type])
                    {
                        NPC.frameCounter += animationSpeed;
                        int num332 = NPC.frame.Y / frameHeight;
                        int num333 = nFramesWithoutAttackAnim - num332;
                        if ((uint)(num333 - 1) > 1u && (uint)(num333 - 4) > 1u && num332 != 0)
                        {
                            NPC.frame.Y = 0;
                            NPC.frameCounter = 0.0;
                        }
                        int num334 = 0;
                        if (NPC.frameCounter < 16.0)
                        {
                            num334 = 0;
                        }
                        else if (NPC.frameCounter == 16.0)
                        {
                            EmoteBubble.NewBubbleNPC(new WorldUIAnchor(NPC), 112);
                        }
                        else if (NPC.frameCounter < 128.0)
                        {
                            num334 = ((NPC.frameCounter % 16.0 < 8.0) ? (nFramesWithoutAttackAnim - 2) : 0);
                        }
                        else if (NPC.frameCounter < 160.0)
                        {
                            num334 = 0;
                        }
                        else if (NPC.frameCounter != 160.0)
                        {
                            num334 = ((NPC.frameCounter < 220.0) ? ((NPC.frameCounter % 12.0 < 6.0) ? (nFramesWithoutAttackAnim - 2) : 0) : 0);
                        }
                        else
                        {
                            EmoteBubble.NewBubbleNPC(new WorldUIAnchor(NPC), 60);
                        }
                        NPC.frame.Y = 0;

                        if (NPC.frameCounter >= 220.0)
                        {
                            NPC.frameCounter = 0.0;
                        }
                    }
                    else if (NPC.CanTalk && (AIState == (float)ActionState.State3 || AIState == (float)ActionState.State4))
                    {
                        NPC.frameCounter += animationSpeed;
                        int num353 = NPC.frame.Y / frameHeight;
                        int num354 = nFramesWithoutAttackAnim - num353;
                        if ((uint)(num354 - 1) > 1u && (uint)(num354 - 4) > 1u && num353 != 0)
                        {
                            NPC.frame.Y = 0;
                            NPC.frameCounter = 0.0;
                        }
                        bool flag15 = NPC.ai[0] == 3f;
                        int num355 = 0;
                        int num356 = 0;
                        int num357 = -1;
                        int num358 = -1;
                        if (NPC.frameCounter < 10.0)
                        {
                            num355 = 0;
                        }
                        else if (NPC.frameCounter < 16.0)
                        {
                            num355 = nFramesWithoutAttackAnim - 5;
                        }
                        else if (NPC.frameCounter < 46.0)
                        {
                            num355 = nFramesWithoutAttackAnim - 4;
                        }
                        else if (NPC.frameCounter < 60.0)
                        {
                            num355 = nFramesWithoutAttackAnim - 5;
                        }
                        else if (NPC.frameCounter < 216.0)
                        {
                            num355 = 0;
                        }
                        else if (NPC.frameCounter == 216.0 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            num357 = 70;
                        }
                        else if (NPC.frameCounter < 286.0)
                        {
                            num355 = ((NPC.frameCounter % 12.0 < 6.0) ? (nFramesWithoutAttackAnim - 2) : 0);
                        }
                        else if (NPC.frameCounter < 320.0)
                        {
                            num355 = 0;
                        }
                        else if (NPC.frameCounter != 320.0 || Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            num355 = ((NPC.frameCounter < 420.0) ? ((NPC.frameCounter % 16.0 < 8.0) ? (nFramesWithoutAttackAnim - 2) : 0) : 0);
                        }
                        else
                        {
                            num357 = 100;
                        }
                        if (NPC.frameCounter < 70.0)
                        {
                            num356 = 0;
                        }
                        else if (NPC.frameCounter != 70.0 || Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            num356 = ((!(NPC.frameCounter < 160.0)) ? ((NPC.frameCounter < 166.0) ? (nFramesWithoutAttackAnim - 5) : ((NPC.frameCounter < 186.0) ? (nFramesWithoutAttackAnim - 4) : ((NPC.frameCounter < 200.0) ? (nFramesWithoutAttackAnim - 5) : ((!(NPC.frameCounter < 320.0)) ? ((NPC.frameCounter < 326.0) ? (nFramesWithoutAttackAnim - 1) : 0) : 0)))) : ((NPC.frameCounter % 16.0 < 8.0) ? (nFramesWithoutAttackAnim - 2) : 0));
                        }
                        else
                        {
                            num358 = 90;
                        }
                        if (flag15)
                        {
                            NPC nPC = Main.npc[(int)NPC.ai[2]];
                            if (num357 != -1)
                            {
                                EmoteBubble.NewBubbleNPC(new WorldUIAnchor(NPC), num357, new WorldUIAnchor(nPC));
                            }
                            if (num358 != -1 && nPC.CanTalk)
                            {
                                EmoteBubble.NewBubbleNPC(new WorldUIAnchor(nPC), num358, new WorldUIAnchor(NPC));
                            }
                        }
                        NPC.frame.Y = 0;

                        if (NPC.frameCounter >= 420.0)
                        {
                            NPC.frameCounter = 0.0;
                        }
                    }
                    // Rock paper scissors?
                    else if (NPC.CanTalk && (AIState == (float)ActionState.State16 || AIState == (float)ActionState.State17))
                    {
                        NPC.frameCounter += animationSpeed;
                        int num359 = NPC.frame.Y / frameHeight;
                        int num360 = nFramesWithoutAttackAnim - num359;
                        if ((uint)(num360 - 1) > 1u && (uint)(num360 - 4) > 1u && num359 != 0)
                        {
                            NPC.frame.Y = 0;
                            NPC.frameCounter = 0.0;
                        }
                        bool flag16 = NPC.ai[0] == 16f;
                        int num361 = 0;
                        int num362 = -1;
                        if (NPC.frameCounter < 10.0)
                        {
                            num361 = 0;
                        }
                        else if (NPC.frameCounter < 16.0)
                        {
                            num361 = nFramesWithoutAttackAnim - 5;
                        }
                        else if (NPC.frameCounter < 22.0)
                        {
                            num361 = nFramesWithoutAttackAnim - 4;
                        }
                        else if (NPC.frameCounter < 28.0)
                        {
                            num361 = nFramesWithoutAttackAnim - 5;
                        }
                        else if (NPC.frameCounter < 34.0)
                        {
                            num361 = nFramesWithoutAttackAnim - 4;
                        }
                        else if (NPC.frameCounter < 40.0)
                        {
                            num361 = nFramesWithoutAttackAnim - 5;
                        }
                        else if (NPC.frameCounter == 40.0 && Main.netMode != 1)
                        {
                            num362 = 45;
                        }
                        else if (NPC.frameCounter < 70.0)
                        {
                            num361 = nFramesWithoutAttackAnim - 4;
                        }
                        else if (NPC.frameCounter < 76.0)
                        {
                            num361 = nFramesWithoutAttackAnim - 5;
                        }
                        else if (NPC.frameCounter < 82.0)
                        {
                            num361 = nFramesWithoutAttackAnim - 4;
                        }
                        else if (NPC.frameCounter < 88.0)
                        {
                            num361 = nFramesWithoutAttackAnim - 5;
                        }
                        else if (NPC.frameCounter < 94.0)
                        {
                            num361 = nFramesWithoutAttackAnim - 4;
                        }
                        else if (NPC.frameCounter < 100.0)
                        {
                            num361 = nFramesWithoutAttackAnim - 5;
                        }
                        else if (NPC.frameCounter == 100.0 && Main.netMode != 1)
                        {
                            num362 = 45;
                        }
                        else if (NPC.frameCounter < 130.0)
                        {
                            num361 = nFramesWithoutAttackAnim - 4;
                        }
                        else if (NPC.frameCounter < 136.0)
                        {
                            num361 = nFramesWithoutAttackAnim - 5;
                        }
                        else if (NPC.frameCounter < 142.0)
                        {
                            num361 = nFramesWithoutAttackAnim - 4;
                        }
                        else if (NPC.frameCounter < 148.0)
                        {
                            num361 = nFramesWithoutAttackAnim - 5;
                        }
                        else if (NPC.frameCounter < 154.0)
                        {
                            num361 = nFramesWithoutAttackAnim - 4;
                        }
                        else if (NPC.frameCounter < 160.0)
                        {
                            num361 = nFramesWithoutAttackAnim - 5;
                        }
                        else if (NPC.frameCounter != 160.0 || Main.netMode == 1)
                        {
                            num361 = ((NPC.frameCounter < 220.0) ? (nFramesWithoutAttackAnim - 4) : ((NPC.frameCounter < 226.0) ? (nFramesWithoutAttackAnim - 5) : 0));
                        }
                        else
                        {
                            num362 = 75;
                        }
                        if (flag16 && num362 != -1)
                        {
                            int num363 = (int)NPC.localAI[2];
                            int num364 = (int)NPC.localAI[3];
                            int num365 = (int)Main.npc[(int)NPC.ai[2]].localAI[3];
                            int num366 = (int)Main.npc[(int)NPC.ai[2]].localAI[2];
                            int num367 = 3 - num363 - num364;
                            int num368 = 0;
                            if (NPC.frameCounter == 40.0)
                            {
                                num368 = 1;
                            }
                            if (NPC.frameCounter == 100.0)
                            {
                                num368 = 2;
                            }
                            if (NPC.frameCounter == 160.0)
                            {
                                num368 = 3;
                            }
                            int num369 = 3 - num368;
                            int num370 = -1;
                            int num371 = 0;
                            while (num370 < 0)
                            {
                                num360 = num371 + 1;
                                num371 = num360;
                                if (num360 >= 100)
                                {
                                    break;
                                }
                                num370 = Main.rand.Next(2);
                                if (num370 == 0 && num366 >= num364)
                                {
                                    num370 = -1;
                                }
                                if (num370 == 1 && num365 >= num363)
                                {
                                    num370 = -1;
                                }
                                if (num370 == -1 && num369 <= num367)
                                {
                                    num370 = 2;
                                }
                            }
                            if (num370 == 0)
                            {
                                Main.npc[(int)NPC.ai[2]].localAI[3] += 1f;
                                num365++;
                            }
                            if (num370 == 1)
                            {
                                Main.npc[(int)NPC.ai[2]].localAI[2] += 1f;
                                num366++;
                            }
                            int num372 = Utils.SelectRandom<int>(Main.rand, 38, 37, 36);
                            int num373 = num372;
                            switch (num370)
                            {
                                case 0:
                                    switch (num372)
                                    {
                                        case 38:
                                            num373 = 37;
                                            break;
                                        case 37:
                                            num373 = 36;
                                            break;
                                        case 36:
                                            num373 = 38;
                                            break;
                                    }
                                    break;
                                case 1:
                                    switch (num372)
                                    {
                                        case 38:
                                            num373 = 36;
                                            break;
                                        case 37:
                                            num373 = 38;
                                            break;
                                        case 36:
                                            num373 = 37;
                                            break;
                                    }
                                    break;
                            }
                            if (num369 == 0)
                            {
                                if (num365 >= 2)
                                {
                                    num372 -= 3;
                                }
                                if (num366 >= 2)
                                {
                                    num373 -= 3;
                                }
                            }
                            EmoteBubble.NewBubble(num372, new WorldUIAnchor(NPC), num362);
                            EmoteBubble.NewBubble(num373, new WorldUIAnchor(Main.npc[(int)NPC.ai[2]]), num362);
                        }
                        // Puro doesn't shake his hands so we ignore all the stuff above and use his idle animation instead
                        NPC.frame.Y = 0;

                        if (NPC.frameCounter >= 420.0)
                        {
                            NPC.frameCounter = 0.0;
                        }
                    }
                    // Standing still and walking
                    else
                    {
                        FindFrameStandingStillAndWalking(frameHeight, extraFrames);
                    }
                }
            }
            // Falling down
            else
            {
                NPC.frameCounter = 0.0;
                NPC.frame.Y = frameHeight;
            }

            UpdateHatPosition(frameHeight);
        }
    }
}