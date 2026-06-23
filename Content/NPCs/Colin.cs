using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Dusts;
using ChangedSpecialMod.Content.Items.Food;
using ChangedSpecialMod.Content.Projectiles;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
	public partial class Colin : ModNPC
	{
        private static int shopIndex = 0;
        private const string ShopNamePath = "Mods.ChangedSpecialMod.ShopNames";

        private int loveNPC = NPCID.BestiaryGirl;
        private int likeNPC = NPCID.PartyGirl;
        private int dislikeNPC = NPCID.TaxCollector;
        private int hateNPC = NPCID.Angler;

        private HappinessObject happinessOld = null;

        private static readonly List<ShopData> Shops = new()
        {
            new ShopData("First Shop", "Shop"),
            new ShopData("Second Shop", "Shop"),
        };

		//private static int ShimmerHeadIndex;
		private static Profiles.StackedNPCProfile NPCProfile;
        private DialogueObject dialogueCurrent = null;

        // This should add up to 1 or it will break (so don't use something like 0.3)
        public float animationSpeed = 1.0f;

		public override void SetStaticDefaults() 
		{
			Main.npcFrameCount[Type] = 44;
			NPCID.Sets.ExtraFramesCount[Type] = 28;
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
                .SetBiomeAffection<BlackLatexSurfaceBiome>(AffectionLevel.Love)
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Like) 
				.SetBiomeAffection<DesertBiome>(AffectionLevel.Hate) 
				
				.SetNPCAffection(loveNPC, AffectionLevel.Love) 
				.SetNPCAffection(likeNPC, AffectionLevel.Like) 
				.SetNPCAffection(dislikeNPC, AffectionLevel.Dislike)
				.SetNPCAffection(hateNPC, AffectionLevel.Hate); 
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
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
            AnimationType = NPCID.None;
            SpawnModBiomes = new int[] { ModContent.GetInstance<CityRuinsSurfaceBiome>().Type };

            var changedNPC = NPC.Changed();
            //changedNPC.DefaultHitEffect = true;
            changedNPC.GooType = GooType.Invalid;
            changedNPC.CanHaveBeer = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.Colin.Description"))
            });
        }

		public override bool CanTownNPCSpawn(int numTownNPCs) 
		{
			return DownedBossSystem.ColinCanSpawn;
		}

        public override ITownNPCProfile TownNPCProfile() 
        {
			return NPCProfile;
		}

		public override List<string> SetNPCNameList() 
        {
			return new List<string>() 
            {
				"Colin"
			};
		}

		public override string GetChat() 
		{
            /*
            var changedNPC = NPC.Changed();
            var keyWords = changedNPC.GetChatKeyWords();
            var invasionType = Main.invasionType;
            var eclipse = Main.eclipse;
            var bloodMoon = Main.bloodMoon;
            dialogueCurrent = DialogueNormal;

			switch (invasionType)
			{
				case InvasionID.PirateInvasion:
                    dialogueCurrent = DialoguePirates;
					break;
				case InvasionID.MartianMadness:
                    dialogueCurrent = DialogueMartians;
					break;
				default:
                    // He also wears a partyhat on the birthday season, but doesnt use the dialogue there
					if (BirthdayParty.PartyIsUp)
                        dialogueCurrent = DialogueParty;
                    else if (Main.bloodMoon)
                        dialogueCurrent = DialogueBloodMoon;
                    else if (Main.eclipse)
                        dialogueCurrent = DialogueEclipse;
                    break;
            }

            (string dialogueText, string emotionText) = dialogueCurrent.GetDialogue(keyWords);
            */
            NPCPortraitSystem.UpdatePortrait(ModContent.NPCType<Colin>(), "Neutral");
            return "...";
		}

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button2 = Language.GetTextValue($"{ShopNamePath}.CycleShop");
            var currentShop = Shops[shopIndex];
            button = Language.GetTextValue($"{ShopNamePath}.{currentShop.DisplayKey}");
            //UpdatePortraitOnHappinessButtonClicked();
        }

        /*
        // This is awful, because it is run every frame
        // happinessOld is used for caching so at least the texture won't be updated every time
        private void UpdatePortraitOnHappinessButtonClicked()
        {
            var happiness = new HappinessObject(
                NPC, "Puro", 
                NPCID.Angler, NPCID.TaxCollector, NPCID.PartyGirl, NPCID.BestiaryGirl, 
                "Desert", null, "Forest", "Mods.ChangedSpecialMod.BlackLatexSurfaceBiome.TownNPCDialogueName");
            // This will only be true if the happiness dialogue is open
            if (happiness.ContainsAny)
            {                
                if (happinessOld == null)
                {
                    happinessOld = happiness;
                    var emotion = happiness.GetEmotion();
                    NPCPortraitSystem.UpdatePortrait(ModContent.NPCType<Puro>(), emotion);
                }
            }
            else
            {
                happinessOld = null;
            }
        }
        */

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (firstButton)
                shop = Shops[shopIndex].InternalName;
            else
                shopIndex = (shopIndex + 1) % Shops.Count;
        }

        public override void AddShops() 
        {
			new NPCShop(Type, Shops[0].InternalName)
                .Add(ItemID.ZebraSkin)
                .Add(ItemID.TigerSkin)
                .Add(ItemID.LeopardSkin)
                .Add<Items.Placeable.Furniture.SkinSnep>()
                .Add<Items.Placeable.Furniture.SkinLion>()
                .Add(ItemID.LawnFlamingo)
                .Register();

			new NPCShop(Type, Shops[1].InternalName)
                // Normal paintings
                .Add(ItemID.WineGlass)
                .Add(ItemID.SteampunkCup)
                .Add(ItemID.FancyDishes)
                .Add(ItemID.BlueDungeonVase)
                .Add(ItemID.GreenDungeonVase)
                .Add(ItemID.PinkDungeonVase)
                .Add(ItemID.ObsidianVase)
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
            var changedNPC = NPC.Changed();
            changedNPC.AdjustStatScaling(NPC);
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

        private void DetermineHat(ChangedNPC changedNPC)
        {
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
                    case SeasonalEvent.Fiesta:
                        modHat = "Content/Items/SombreroHat";
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
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var changedNPC = NPC.Changed();
            DetermineHat(changedNPC);
            changedNPC.PostDrawExtra(NPC, spriteBatch, screenPos, drawColor);
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }

        private void UpdateHatPosition(int frameHeight)
        {
            var changedNPC = NPC.Changed();
            var frame = NPC.frame;

            var hatXOffset = 0;
            var hatYOffset = -36;

            var fr = frame.Top / frameHeight;
            var legsWide = fr == 1 || fr == 13 || fr == 14 || fr == 15 || fr == 35 || fr == 36 || fr == 37;
            var legsCrossed = fr == 6 || fr == 7 || fr == 8 || fr == 28 || fr == 29 || fr == 30;
            var isSitting = fr >= 18 && fr <= 23;
            if (isSitting)
                hatYOffset = -26;
            else if (legsCrossed)
                hatYOffset = -34;
            else if (legsWide)
                hatYOffset = -34;

            changedNPC.HatXOffset = hatXOffset;
            changedNPC.HatYOffset = hatYOffset;
        }

        private void UpdateBeerPosition(int frameHeight)
        {
            var changedNPC = NPC.Changed();
            var beerXOffset = -2;
            var beerYOffset = 2;
            var frame = NPC.frame;
            var fr = frame.Top / frameHeight;

            if (fr == 1 || fr == 13 || fr == 14 || fr == 15 || fr == 35 || fr == 36 || fr == 37)
            {
                beerXOffset = -1;
            }
            else if (fr == 6 || fr == 7 || fr == 8 || fr == 28 || fr == 29 || fr == 30)
            {
                beerXOffset = 3;
            }

            changedNPC.BeerXOffset = beerXOffset;
            changedNPC.BeerYOffset = beerYOffset;
        }

        public bool IsGrounded()
        {
            return NPC.velocity.Y == 0f;
        }

        public bool IsSitting()
        {
            return NPC.ai[0] == 5f;
        }
    }
}