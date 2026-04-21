using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Dusts;
using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Content.Projectiles;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
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
using Terraria.ModLoader.IO;


namespace ChangedSpecialMod.Content.NPCs
{
    public class DialogueObject
	{
        private string BasePathDialogue = "Mods.ChangedSpecialMod.NPCs.Dialogue";
        private string CharacterName = "Puro";
        private List<string> Dialogue = new List<string>();
        private Dictionary<string, string> KeyWords = new Dictionary<string, string>();

		public DialogueObject(string characterName, List<string> dialogue)
		{
            CharacterName = characterName;
			Dialogue = dialogue ?? new List<string>();
			ChangedUtils.Shuffle(Dialogue);
		}

        // Keywords can often change so we don't set it during creation. For example when an NPC dies it should not pick an option with his name anymore
        public string GetDialogue(Dictionary<string, string> keyWords)
		{
			if (keyWords != null) KeyWords = keyWords;
            var length = Dialogue.Count;
            if (length == 0) return null;

            // Iterate all dialogue options
            for (var i = 0; i < length; i++)
            {
                var strKey = Dialogue.First();
                var str = Language.GetTextValue($"{BasePathDialogue}.{CharacterName}.{strKey}");

                Dialogue.RemoveAt(0);
				Dialogue.Add(strKey);

				// Get all variables from the string which are formatted like {0}, {1}, {2}
                var matches = Regex.Matches(str, @"\{([^}]*)\}");
                List<string> keys = matches.Cast<Match>().Select(m => m.Groups[1].Value).ToList();
				if (keys != null && keys.Count > 0)
				{
					var failed = false;
					foreach (var key in keys)
					{
						if (!KeyWords.ContainsKey(key))
						{
							failed = true;
							break;
						}
						str = str.Replace("{" + key + "}", KeyWords[key]);
					}
					if (!failed) return str;
				}
				else
				{
					return str;
				}
            }
			return null;
		}
	}



	[AutoloadHead]
	public class Puro : ModNPC
	{
		public const string ShopName = "Shop";
		public const string ShopName2 = "Second shop";
		public int NumberOfTimesTalkedTo = 0;

		private static int ShimmerHeadIndex;
		private static Profiles.StackedNPCProfile NPCProfile;

        // This should add up to 1 or it will break (so don't use something like 0.3)
        public float animationSpeed = 1.0f;
		
		public static DialogueObject DialogueNormal = new DialogueObject(
            "Puro",
			new List<string>
			{
				// NPC
                "NPCDryad",
                "NPCGuide",
                "NPCNurse",
                "NPCMerchant",
                "NPCTavernKeep",
                "NPCAngler",

				// Books
				"Book1",
                "Book2",
                "Book3",
                "Book4",
                "Book5",
                "Book6",
                "Book7",

				// Orange
                "Orange1",
                "Orange2",
                "Orange3",
                "Orange4",
                "Orange5",
                "Orange6",

				// Normal
				"Normal1",

				// World Evil
                "Crimson1",
                "Crimson2",
                "Corruption1",

				// Changed
                "Changed1",
                "Changed2",
                //"Changed3",
                //"Changed4",
                //"Changed5",
                //"Changed6",

				//Rain
                "Rain1",
                "Rain2",

				//Thunder
                "Thunder1",
                "Thunder2",

				//Windy
				"Windy1",
				"Windy2",
				"Windy3",

                //Valentine
                "Valentine1",
                "Valentine2",

                //Oktoberfest
                "Oktoberfest1",
                "Oktoberfest2",
                "Oktoberfest3",
                "Oktoberfest4",

                //Halloween
                "Halloween1",
                "Halloween2",
                "Halloween3",
                "Halloween4",
                "Halloween5",

                //Xmas
                "Xmas1",
                "Xmas2",
                "Xmas3",
                "Xmas4",
                "Xmas5",

				//Items
				"PlayerHasOrange",
                "PlayerHasBook",
                "PlayerHasBookOfSkulls",
                "PlayerHasWaterBolt",
                "PlayerHasGoldenShower",
                "PlayerIsWearingBalloon",
				"PlayerIsWearingWeddingDress",
                "PlayerHasPurrpurr",

                //Transfurs
                "TransfurCub"
            }
		);

		public static DialogueObject DialoguePirates = new DialogueObject(
            "Puro.Pirates",
            new List<string>
			{
                "Pirates1",
                "Pirates2",
                "Pirates3",
                "Pirates4",
                "Pirates5"
            }
		);

        public static DialogueObject DialogueMartians = new DialogueObject(
            "Puro.Martians",
            new List<string>
            {
                "Martians1",
                "Martians2",
                "Martians3"
            }
        );

        public static DialogueObject DialogueBloodMoon = new DialogueObject(
            "Puro.BloodMoon",
            new List<string>
			{
				"BloodMoon1",
				"BloodMoon2",
				"BloodMoon3",
				"BloodMoon4",
				"BloodMoon5"
			}
		);

		public static DialogueObject DialogueEclipse = new DialogueObject(
            "Puro.Eclipse",
            new List<string>
            {
                "Eclipse1",
                "Eclipse2",
                "Eclipse3"
            }
        );

        public static DialogueObject DialogueParty = new DialogueObject(
            "Puro.Party",
            new List<string>
			{
				"Party1",
				"Party2",
                "Party3",
                "Party4",
                "Party5",
                "Party6",
                "Party7",
                "Party8",
                "Party9",
                "Party10",
                "Party11",
                "Party12",
            }
		);

        public override void Load() 
		{
			ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");
		}

		public override void SetStaticDefaults() 
		{
			Main.npcFrameCount[Type] = 30; //25
			NPCID.Sets.ExtraFramesCount[Type] = 14; //9 // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
			NPCID.Sets.AttackFrameCount[Type] = 4; // The amount of frames in the attacking animation.
			NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the NPC that it tries to attack enemies.
			NPCID.Sets.AttackType[Type] = 0; // The type of attack the Town NPC performs. 0 = throwing, 1 = shooting, 2 = magic, 3 = melee
			NPCID.Sets.AttackTime[Type] = 90; // The amount of time it takes for the NPC's attack animation to be over once it starts.
			NPCID.Sets.AttackAverageChance[Type] = 30; // The denominator for the chance for a Town NPC to attack. Lower numbers make the Town NPC appear more aggressive.
			NPCID.Sets.ShimmerTownTransform[Type] = true; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() 
			{
				Velocity = 1f
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPC.Happiness
                //.SetBiomeAffection<CityRuinsSurfaceBiome>(AffectionLevel.Love)
                .SetBiomeAffection<BlackLatexSurfaceBiome>(AffectionLevel.Love)
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Like) 
				.SetBiomeAffection<DesertBiome>(AffectionLevel.Hate) 
                // Black latexes hate white ones, so maybe that should also be added
				
				.SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Love) 
				.SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Like) 
				.SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Angler, AffectionLevel.Hate); 

			NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture), Texture + "_Party"),
				new Profiles.DefaultNPCProfile(Texture + "_Shimmer", ShimmerHeadIndex, Texture + "_Shimmer_Party")
			);
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
			NPC.aiStyle = NPCAIStyleID.Passive;
			NPC.damage = 25;
			NPC.defense = 15;
			NPC.lifeMax = 400;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
            AnimationType = NPCID.None;
            SpawnModBiomes = new int[] { ModContent.GetInstance<BlackLatexSurfaceBiome>().Type };

            var changedNPC = NPC.Changed();
            changedNPC.DefaultHitEffect = true;
            changedNPC.GooType = GooType.Black;
            changedNPC.CanHaveBeer = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.Puro.Description"))
            });
        }

		public override bool CanTownNPCSpawn(int numTownNPCs) 
		{
			return DownedBossSystem.DownedWhiteTail || DownedBossSystem.DownedWolfKing || DownedBossSystem.DownedBehemoth;
		}

        public override ITownNPCProfile TownNPCProfile() 
        {
			return NPCProfile;
		}

		public override List<string> SetNPCNameList() 
        {
			return new List<string>() 
            {
				"Puro"
			};
		}

		public override string GetChat() 
		{
            var changedNPC = NPC.Changed();
            var keyWords = changedNPC.GetChatKeyWords();
            var invasionType = Main.invasionType;
            var eclipse = Main.eclipse;
            var bloodMoon = Main.bloodMoon;
            DialogueObject dialogue = DialogueNormal;

			switch (invasionType)
			{
				case InvasionID.PirateInvasion:
					dialogue = DialoguePirates;
					break;
				case InvasionID.MartianMadness:
					dialogue = DialogueMartians;
					break;
				default:
                    // He also wears a partyhat on the birthday season, but doesnt use the dialogue there
					if (BirthdayParty.PartyIsUp)
					{
						dialogue = DialogueParty;
                    }
					else if (Main.bloodMoon)
					{
						dialogue = DialogueBloodMoon;
					}
					else if (Main.eclipse)
					{
						dialogue = DialogueEclipse;
					}
					break;
            }

            return dialogue.GetDialogue(keyWords);
		}

		public override void SetChatButtons(ref string button, ref string button2) 
		{
			button = Language.GetTextValue("LegacyInterface.28");
			button2 = "Paintings";//Language.GetTextValue("LegacyInterface.28");
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
                .Add(ItemID.Book)
                .Add<Items.Weapons.Encyclopedia>()
                .Add<Items.Placeable.DryDirtBlock>()
                .Add<Items.Placeable.BlackLatexBlock>()
                .Add<Items.Placeable.WhiteLatexBlock>()
				.Add<Items.Placeable.Furniture.PackingBox>()
				.Add<Items.Placeable.Furniture.DocumentPaper>()
				.Add<Items.Placeable.Furniture.StorageBox>()
				.Add<Items.Placeable.Furniture.StackOfBoxes>()
				.Add<Items.Placeable.Furniture.PuroPlush>()
				.Add<Items.Placeable.Furniture.SharkPlush>()
				.Add<Items.Placeable.Furniture.FennecPlush>()
				.Add<Items.Placeable.Furniture.Basketball>()
				.Add<Items.Placeable.Furniture.Blocks>()
				.Add<Items.Seasons.SetSeasonValentine>()
				.Add<Items.Seasons.SetSeasonOktoberfest>()
				.Add<Items.Seasons.SetSeasonHalloween>()
				.Add<Items.Seasons.SetSeasonXmas>()
				.Add<Items.Seasons.SetSeasonNone>()
                .Register();

			new NPCShop(Type, ShopName2)
                // Normal paintings
                .Add<Items.Placeable.Furniture.Painting1>()
                .Add<Items.Placeable.Furniture.Painting2>()
                .Add<Items.Placeable.Furniture.Painting3>()
                .Add<Items.Placeable.Furniture.Painting4>()
                .Add<Items.Placeable.Furniture.Painting5>()
                .Add<Items.Placeable.Furniture.Painting8>()
                .Add<Items.Placeable.Furniture.Painting9>()
                .Add<Items.Placeable.Furniture.Painting10>()
                .Add<Items.Placeable.Furniture.Painting11>()
                .Add<Items.Placeable.Furniture.Painting16>()
                .Add<Items.Placeable.Furniture.Painting17>()

                // Big paintings
                .Add<Items.Placeable.Furniture.Painting6>()
                .Add<Items.Placeable.Furniture.Painting7>()
                .Add<Items.Placeable.Furniture.Painting12>()
                .Add<Items.Placeable.Furniture.Painting13>()
                .Add<Items.Placeable.Furniture.Painting14>()
                .Add<Items.Placeable.Furniture.Painting15>()

                // Drunk paintings
                .Add<Items.Placeable.Furniture.DrunkPainting1>()
                .Add<Items.Placeable.Furniture.DrunkPainting2>()
                .Add<Items.Placeable.Furniture.DrunkPainting3>()
                .Add<Items.Placeable.Furniture.DrunkPainting4>()

                // Pictures
                .Add<Items.Placeable.Furniture.Pictures1>()
                .Add<Items.Placeable.Furniture.Pictures2>()
                .Add<Items.Placeable.Furniture.Pictures3>()
                .Add<Items.Placeable.Furniture.Pictures4>()
                .Add<Items.Placeable.Furniture.Pictures5>()
                .Add<Items.Placeable.Furniture.Pictures6>()

                .Register();

        }

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Orange>()));
		}

		// Make this Town NPC teleport to the King and/or Queen statue when triggered. Return toKingStatue for only King Statues. Return !toKingStatue for only Queen Statues. Return true for both.
		public override bool CanGoToStatue(bool toKingStatue)
		{
			return toKingStatue;
        }

		// Create a square of pixels around the NPC on teleport.
		public void StatueTeleport() 
        {
			for (int i = 0; i < 30; i++) 
            {
				Vector2 position = Main.rand.NextVector2Square(-20, 21);
				if (Math.Abs(position.X) > Math.Abs(position.Y)) 
                {
					position.X = Math.Sign(position.X) * 20;
				}
				else {
					position.Y = Math.Sign(position.Y) * 20;
				}

				Dust.NewDustPerfect(NPC.Center + position, ModContent.DustType<Sparkle>(), Vector2.Zero).noGravity = true;
			}
		}

		public void UpdateStats()
		{
            var ChangedGlobalNPC = NPC.Changed();
            ChangedGlobalNPC.AdjustStatScaling(NPC);
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
			projType = ModContent.ProjectileType<OrangeProjectile>();
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) 
        {
			multiplier = 12f;
			randomOffset = 2f;
		}

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var changedNPC = NPC.Changed();
            var hatId = -1;
            string modHat = null;
            changedNPC.HasBeer = false;

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
                        hatId = ItemID.Heart;
                        break;
                    case SeasonalEvent.Oktoberfest:
                        changedNPC.HasBeer = true;
                        modHat = "Content/Items/AlpineHat";
                        break;
                    case SeasonalEvent.Halloween:
                        hatId = ItemID.WizardHat;
                        break;
                    case SeasonalEvent.XMas:
                        hatId = ItemID.SantaHat;
                        break;
                }
			}

			if (hatId != -1 || modHat != null)
			{
                if (modHat != null)
                    changedNPC.SetHat(modHat);
                else
                    changedNPC.SetHat(hatId);
            }
            else
            {
                changedNPC.RemoveHat();
            }

            changedNPC.PostDrawExtra(NPC, spriteBatch, screenPos, drawColor);
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }

        private void UpdateHatPosition(int frameHeight)
        {
            var changedNPC = NPC.Changed();
            var frame = NPC.frame;

            var hatXOffset = 0;
            var hatYOffset = -43;

            // Adjust hat postion when sitting
            // 18 = sitting frame
            var fr = frame.Top / frameHeight;
            if (fr == 18 || fr == 22 || fr == 23)
            {
                hatXOffset = 8;
                hatYOffset = -32;
            }
            else if (fr == 19 || fr == 20 || fr == 21)
            {
                hatYOffset = -32;
            }

            changedNPC.HatXOffset = hatXOffset;
            changedNPC.HatYOffset = hatYOffset;
        }

        private void UpdateBeerPosition(int frameHeight)
        {
            var changedNPC = NPC.Changed();
            var beerXOffset = 6;
            var beerYOffset = 2;
            var frame = NPC.frame;
            var fr = frame.Top / frameHeight;

            if (fr == 1 || fr == 13 || fr == 14 || fr == 15)
            {
                beerXOffset = 5;
            }
            else if (fr == 6 || fr == 7 || fr == 8)
            {
                beerXOffset = 11;
            }

            changedNPC.BeerXOffset = beerXOffset;
            changedNPC.BeerYOffset = beerYOffset;
        }

        public override void LoadData(TagCompound tag) 
        {
			NumberOfTimesTalkedTo = tag.GetInt("numberOfTimesTalkedTo");
		}

		public override void SaveData(TagCompound tag)
        {
			tag["numberOfTimesTalkedTo"] = NumberOfTimesTalkedTo;
		}

        public bool IsGrounded()
        {
            return NPC.velocity.Y == 0f;
        }

        public bool IsSitting()
        {
            return NPC.ai[0] == 5f;
        }

        // This is the vanilla code with unnecessary conditions removed (like pets and town slimes)
        // The frame logic for talking and shaking hands is still here but disabled, because it got messed up after adding more frames for sitting
        public override void FindFrame(int frameHeight)
        {
            // Set this to true anywhere to determine when a certain piece of code is executed. It will rapidly flip the sprite around
            bool testFlag = false;
            int extraFrames = (NPC.isLikeATownNPC ? NPCID.Sets.ExtraFramesCount[NPC.type] : 0);
            if (IsGrounded())
            {
                if (NPC.direction == 1)
                {
                    NPC.spriteDirection = 1;
                }
                if (NPC.direction == -1)
                {
                    NPC.spriteDirection = -1;
                }

                int nFramesWithoutAttackAnim = Main.npcFrameCount[NPC.type] - NPCID.Sets.AttackFrameCount[NPC.type];
                int frameSleep = nFramesWithoutAttackAnim - 3;

                if (NPC.ai[0] == 23f)
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
                else if (NPC.ai[0] >= 20f && NPC.ai[0] <= 22f)
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
                else if (NPC.ai[0] == 2f)
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
                else if (NPC.ai[0] == 11f)
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
                            if (Main.rand.Next(30) == 0)
                            {

                                //int num326 = Gore.NewGore(NPC.Center + Vector2.UnitX * -NPC.direction * 8f, Vector2.Zero, Main.rand.Next(580, 583));
                                //Main.gore[num326].velocity /= 2f;
                                //Main.gore[num326].velocity.Y = Math.Abs(Main.gore[num326].velocity.Y);
                                //Main.gore[num326].velocity.X = (0f - Math.Abs(Main.gore[num326].velocity.X)) * (float)NPC.direction;

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
                        //NPC.frame.Y = frameHeight * (nFramesWithoutAttackAnim - 1);
                        NPC.frameCounter = 0.0;
                        EmoteBubble.NewBubble(89, new WorldUIAnchor(NPC), 90);
                    }
                    else if (NPC.frame.Y != 0 && NPC.frame.Y != frameHeight * (nFramesWithoutAttackAnim - 1))
                    {
                        NPC.frame.Y = 0;
                        NPC.frameCounter = 0.0;
                    }
                }
                // Sitting. This is the sole reason we make a custom animation type so he can shake his head
                else if (IsSitting())
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
                else if (NPC.ai[0] == 6f)
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
                    //NPC.frame.Y = frameHeight * num329;
                    if (NPC.frameCounter >= 300.0)
                    {
                        NPC.frameCounter = 0.0;
                    }
                }
                else if ((NPC.ai[0] == 7f || NPC.ai[0] == 19f) && !NPCID.Sets.IsTownPet[NPC.type])
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
                    //NPC.frame.Y = frameHeight * num334;
                    if (NPC.frameCounter >= 220.0)
                    {
                        NPC.frameCounter = 0.0;
                    }
                }
                else if (NPC.ai[0] == 9f)
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
                    //NPC.frame.Y = frameHeight * num337;
                }
                else if (NPC.ai[0] == 18f)
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
                    //NPC.frame.Y = frameHeight * frameNumber1;
                }
                else if (NPC.ai[0] == 10f || NPC.ai[0] == 13f)
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
                else if (NPC.ai[0] == 15f)
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
                else if (NPC.ai[0] == 25f)
                {
                    NPC.frame.Y = frameHeight;
                }
                // Shooting?
                else if (NPC.ai[0] == 12f)
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
                else if (NPC.ai[0] == 14f || NPC.ai[0] == 24f)
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
                    if (NPC.ai[0] == 24f)
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
                else if (NPC.ai[0] == 1001f)
                {
                    NPC.frame.Y = 0;
                    //NPC.frame.Y = frameHeight * (nFramesWithoutAttackAnim - 1);
                    NPC.frameCounter = 0.0;
                }
                else if (NPC.CanTalk && (NPC.ai[0] == 3f || NPC.ai[0] == 4f))
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
                    //NPC.frame.Y = frameHeight * (flag15 ? num355 : num356);
                    if (NPC.frameCounter >= 420.0)
                    {
                        NPC.frameCounter = 0.0;
                    }
                }
                // Rock paper scissors?
                else if (NPC.CanTalk && (NPC.ai[0] == 16f || NPC.ai[0] == 17f))
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
                    //NPC.frame.Y = frameHeight * (flag16 ? num361 : num361);
                    if (NPC.frameCounter >= 420.0)
                    {
                        NPC.frameCounter = 0.0;
                    }
                }
                // Standing still
                else if (NPC.velocity.X == 0f)
                {
                    NPC.frame.Y = 0;
                    NPC.frameCounter = 0.0;
                }
                // Walking
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
            // Falling down
            else
            {
                NPC.frameCounter = 0.0;
                NPC.frame.Y = frameHeight;
            }

            if (testFlag)
                NPC.spriteDirection = Utils.SelectRandom(Main.rand, -1, 1);

            UpdateHatPosition(frameHeight);
            UpdateBeerPosition(frameHeight);
        }
    }
}