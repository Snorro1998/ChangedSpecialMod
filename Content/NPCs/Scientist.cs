using ChangedSpecialMod.Common.Configs;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Dusts;
using ChangedSpecialMod.Content.Items.Ammo;
using ChangedSpecialMod.Content.Items.Debug;
using ChangedSpecialMod.Content.Items.Rockets;
using ChangedSpecialMod.Content.Items.Syringes;
using ChangedSpecialMod.Content.NPCs.AIStyles;
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
    public class ShopData
    {
        public string InternalName;
        public string DisplayKey;

        public ShopData(string internalName, string displayKey)
        {
            InternalName = internalName;
            DisplayKey = displayKey;
        }
    }

    [AutoloadHead]
    public class Scientist : ModNPC
    {
        private static int shopIndex = 0;
        private const string ShopNamePath = "Mods.ChangedSpecialMod.ShopNames";

        private static readonly List<ShopData> Shops = new()
        {
            new ShopData("First Shop", "Furniture"),
            new ShopData("Second Shop", "Potions"),
            new ShopData("Third Shop", "BossSummons"),
            new ShopData("Fourth Shop", "Solutions"),
            new ShopData("Fifth Shop", "Syringes"),
            new ShopData("Sixth Shop", "Fireworks"),
            new ShopData("Seventh Shop", "DebugItems"),
        };

        //private static int ShimmerHeadIndex;
        private static Profiles.StackedNPCProfile NPCProfile;
        private DialogueObject dialogueCurrent = null;

        // This should add up to 1 or it will break (so don't use something like 0.3)
        public float animationSpeed = 1.0f;

        public static DialogueObject DialogueNormal = new DialogueObject(
            "DrK",
            new List<DialogueElement>
            {
				// NPC
                new DialogueElement("NPCNurse"),
                new DialogueElement("NPCAngler", "Happy"),
                new DialogueElement("NPCZoologist"),

                // Mod NPCs
                new DialogueElement("NPCAncientsAwakenedAnubis", "Naughty"),
                
				// Normal
                new DialogueElement("Normal1", "Happy"),
                new DialogueElement("Normal2"),
                new DialogueElement("Normal3"),
                new DialogueElement("Normal4", "Happy"),
                new DialogueElement("Normal5"),
                new DialogueElement("Normal6"),
                new DialogueElement("Normal7", "Happy"),
                new DialogueElement("Normal8"),
                new DialogueElement("Normal9"),
                new DialogueElement("Normal10"),
                new DialogueElement("Normal11"),
                new DialogueElement("Normal12"),
                new DialogueElement("Normal13"),

                // World Evil
                new DialogueElement("Crimson1", "Tired"),
                new DialogueElement("Crimson2"),
                new DialogueElement("Corruption1", "Tired"),
                new DialogueElement("Corruption2"),
                new DialogueElement("BothWorldEvils1", "Tired"),

                //Thunder
                new DialogueElement("Thunder1"),
                new DialogueElement("Thunder2"),

                //Windy
                new DialogueElement("Windy1", "Angry"),

                // Valentine
                new DialogueElement("Valentine1"),
                new DialogueElement("Valentine2"),

                //Oktoberfest
                new DialogueElement("Oktoberfest1"),
                new DialogueElement("Oktoberfest2"),
                new DialogueElement("Oktoberfest3"),
                new DialogueElement("Oktoberfest4"),

                //Halloween
                new DialogueElement("Halloween1"),
                new DialogueElement("Halloween2"),
                new DialogueElement("Halloween3"),
                new DialogueElement("Halloween4"),

                //Xmas
                new DialogueElement("Xmas1"),
                new DialogueElement("Xmas2"),
                new DialogueElement("Xmas3"),
                new DialogueElement("Xmas4"),

                //Items
                new DialogueElement("PlayerHasGoldenShower"),
                new DialogueElement("PlayerIsWearingWeddingDress"),
                new DialogueElement("PlayerHasPurrpurr"),

                // Redemption
                new DialogueElement("ModRedemption.Lab1"),
                new DialogueElement("ModRedemption.Lab2"),

                // Terra Guardian
                // Teasing the player for having many normal sized furries in his party
                // (which is much bigger than the player)
                new DialogueElement("ModTerraGuardian.PlayerPartyBigFurries", "Naughty"),
                new DialogueElement("ModTerraGuardian.PlayerPartyBigFurryMen", "Naughty"),
                new DialogueElement("ModTerraGuardian.PlayerPartyBigFurryWomen", "Naughty"),

                // Teasing the player for having many shrunken furries in his party
                // (which is the same size as the player)
                new DialogueElement("ModTerraGuardian.PlayerPartySmallFurries", "Naughty"),
                new DialogueElement("ModTerraGuardian.PlayerPartySmallFurryMen", "Naughty"),
                new DialogueElement("ModTerraGuardian.PlayerPartySmallFurryWomen", "Naughty"),

                //new DialogueElement("ModTerraGuardian.PlayerPartyAllShrunkenFurries"),
                new DialogueElement("ModTerraGuardian.MacroFurryManPresent", "Naughty"),
                new DialogueElement("ModTerraGuardian.MacroFurryWomanPresent", "Naughty"),

                // Terra Guardians
                new DialogueElement("ModTerraGuardian.Alex", "Angry"),
                new DialogueElement("ModTerraGuardian.Blue"),
                new DialogueElement("ModTerraGuardian.Rococo"),
                new DialogueElement("ModTerraGuardian.Scaleforth"),

                /*
                // Games
                new DialogueElement("Game.DragnWash", "Naughty"),
                new DialogueElement("Game.GameHorror", "Naughty"),
                new DialogueElement("Game.GameParty"),
                new DialogueElement("Game.PawpertyDamage", "Naughty")
                */
            }
        );

        public static DialogueObject DialoguePirates = new DialogueObject(
            "DrK.Pirates",
            new List<DialogueElement>
            {
                new DialogueElement("Pirates1", "Angry"),
                new DialogueElement("Pirates2"),
                new DialogueElement("Pirates3", "Angry"),
            }
        );

        public static DialogueObject DialogueMartians = new DialogueObject(
            "DrK.Martians",
            new List<DialogueElement>
            {
                new DialogueElement("Martians1"),
                new DialogueElement("Martians2")
            }
        );

        public static DialogueObject DialogueBloodMoon = new DialogueObject(
            "DrK.BloodMoon",
            new List<DialogueElement>
            {
                new DialogueElement("BloodMoon1", "Evil"),
                new DialogueElement("BloodMoon2", "Evil"),
                new DialogueElement("BloodMoon3", "Evil")
            }
        );

        public static DialogueObject DialogueEclipse = new DialogueObject(
            "DrK.Eclipse",
            new List<DialogueElement>
            {
                new DialogueElement("Eclipse1"),
                new DialogueElement("Eclipse2"),
                new DialogueElement("Eclipse3"),
            }
        );

        public static DialogueObject DialogueParty = new DialogueObject(
            "DrK.Party",
            new List<DialogueElement>
            {
                new DialogueElement("Party1"),
                new DialogueElement("Party2"),
                new DialogueElement("Party3"),
                new DialogueElement("Party4", "Happy"),
                new DialogueElement("Party5", "Happy"),
                new DialogueElement("Party6", "Naughty"),
                new DialogueElement("Party7", "Happy")
            }
        );

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 25;
            NPCID.Sets.ExtraFramesCount[Type] = 9;
            NPCID.Sets.AttackFrameCount[Type] = 4;

            NPCID.Sets.DangerDetectRange[Type] = 700;
            NPCID.Sets.AttackType[Type] = 0;
            NPCID.Sets.AttackTime[Type] = 34;
            NPCID.Sets.AttackAverageChance[Type] = 30;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness
                .SetBiomeAffection<WhiteLatexSurfaceBiome>(AffectionLevel.Love)
                .SetBiomeAffection<WhiteLatexSurfaceJungleBiome>(AffectionLevel.Love)
                .SetBiomeAffection<WhiteLatexSurfaceDesertBiome>(AffectionLevel.Love)
                .SetBiomeAffection<WhiteLatexSurfaceSnowBiome>(AffectionLevel.Love)

                .SetBiomeAffection<OceanBiome>(AffectionLevel.Like)

                .SetBiomeAffection<BlackLatexSurfaceBiome>(AffectionLevel.Dislike)
                .SetBiomeAffection<BlackLatexSurfaceJungleBiome>(AffectionLevel.Dislike)
                .SetBiomeAffection<BlackLatexSurfaceDesertBiome>(AffectionLevel.Dislike)
                .SetBiomeAffection<BlackLatexSurfaceSnowBiome>(AffectionLevel.Dislike)

                .SetNPCAffection(NPCID.Nurse, AffectionLevel.Love)
                .SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Wizard, AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.ArmsDealer, AffectionLevel.Hate);

            //ContentSamples.NpcBestiaryRarityStars[Type] = 3; // We can override the default bestiary star count calculation by setting this.
        }

        public override int? PickEmote(Player closestPlayer, List<int> emoteList, WorldUIAnchor otherAnchor)
        {
            var changedNPC = NPC.Changed();
            emoteList = changedNPC.GetEmoteList(NPC, closestPlayer, emoteList, otherAnchor);
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
            AnimationType = NPCID.Guide;
            SpawnModBiomes = new int[] { ModContent.GetInstance<WhiteLatexSurfaceBiome>().Type };

            var changedNPC = NPC.Changed();
            changedNPC.DefaultHitEffect = true;
            changedNPC.GooType = GooType.White;
            changedNPC.CanHaveBeer = true;
            changedNPC.HatXOffset = 0;
            changedNPC.HatYOffset = -48;
            changedNPC.BeerXOffset = -6;
            changedNPC.BeerYOffset = -3;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.Scientist.Description"))
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
                "Dr K"
            };
        }

        public override string GetChat()
        {
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
                    if (BirthdayParty.PartyIsUp)
                    {
                        dialogueCurrent = DialogueParty;
                    }
                    else if (Main.bloodMoon)
                    {
                        dialogueCurrent = DialogueBloodMoon;
                    }
                    else if (Main.eclipse)
                    {
                        dialogueCurrent = DialogueEclipse;
                    }
                    break;
            }

            (string dialogueText, string emotionText) = dialogueCurrent.GetDialogue(keyWords);
            NPCPortraitSystem.UpdatePortrait(ModContent.NPCType<Scientist>(), emotionText);

            // Something funni if you use Dutch language
            //if (dialogueText.ToLower().Contains("feestje van keesje"))
            //    SoundEngine.PlaySound(Sounds.SoundFeestjeVanKeesje, Main.LocalPlayer.Center);

            return dialogueText;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button2 = Language.GetTextValue($"{ShopNamePath}.CycleShop");

            var rockets = BirthdayParty.PartyIsUp;
            var debugItems = ChangedSpecialModClientConfig.Instance.DebugItems;
            if (shopIndex == 5 && !rockets)
                shopIndex++;

            if (shopIndex == 6 && !debugItems)
                shopIndex++;

            if (shopIndex >= Shops.Count)
                shopIndex = 0;

            var currentShop = Shops[shopIndex];
            button = Language.GetTextValue($"{ShopNamePath}.{currentShop.DisplayKey}");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (firstButton)
            {
                shop = Shops[shopIndex].InternalName;
            }
            // Cycle shops
            else
            {
                var rockets = BirthdayParty.PartyIsUp;
                var debugItems = ChangedSpecialModClientConfig.Instance.DebugItems;

                shopIndex++;

                if (shopIndex == 5 && !rockets)
                    shopIndex++;

                if (shopIndex == 6 && !debugItems)
                    shopIndex++;

                if (shopIndex >= Shops.Count)
                    shopIndex = 0;
            }
        }

        public override void AddShops()
        {
            new NPCShop(Type, Shops[0].InternalName)
                .Add<Items.Placeable.LabTileBlock>()
                .Add<Items.Placeable.CautionTileBlock>()
                .Add<Items.Placeable.Furniture.LabDoor>()
                .Add<Items.Placeable.Furniture.Elevator>()
                .Add<Items.Placeable.Furniture.LabTable>()
                .Add<Items.Placeable.Furniture.IrisScanner>()
                .Add<Items.Placeable.Furniture.Fan>()
                .Add<Items.Placeable.Furniture.Locker>()
                .Add<Items.Placeable.Furniture.Generator>()
                .Add<Items.Placeable.Furniture.Cryopod>()
                .Add<Items.Placeable.Furniture.GreenOfficeChair>()
                .Add<Items.Placeable.Furniture.RedOfficeChair>()
                .Add<Items.Placeable.Furniture.BlueOfficeChair>()
                .Register();

            new NPCShop(Type, Shops[1].InternalName)
                .Add(ItemID.Bottle)
                .Add(ItemID.LesserHealingPotion)
                .Add(ItemID.LesserManaPotion)
                //.Add(ItemID.RegenerationPotion) this one is OP
                .Add(ItemID.ArcheryPotion)
                .Add(ItemID.InvisibilityPotion)
                .Add(ItemID.ShinePotion)
                .Add(ItemID.NightOwlPotion)
                .Add(ItemID.FlipperPotion)
                .Add(ItemID.IronskinPotion)
                .Register();

            new NPCShop(Type, Shops[2].InternalName)
                .Add<Items.Summons.SummonWhiteTail>()
                .Add<Items.Summons.SummonWolfKing>()
                .Add<Items.Summons.SummonBehemoth>()
                .Register();

            new NPCShop(Type, Shops[3].InternalName)
                .Add<BlackLatexSolution>()
                .Add<WhiteLatexSolution>()
                .Add<DryDirtSolution>()
                .Register();

            new NPCShop(Type, Shops[4].InternalName)
                .Add<Syringe>()
                .Add<BlackSyringe>()
                .Add<WhiteSyringe>()
                .Register();

            new NPCShop(Type, Shops[5].InternalName)
                .Add(ItemID.RedRocket)
                .Add(ItemID.GreenRocket)
                .Add(ItemID.BlueRocket)
                .Add<RocketGuide>()
                .Add<RocketEyeOfCthulu>()
                .Add<RocketEaterOfWorlds>()
                .Add<RocketSkeletron>()
                .Add<RocketWhiteTail>()
                .Add<RocketPuro>()
                .Add<RocketPrototype>()
                .Register();

            new NPCShop(Type, Shops[6].InternalName)
                .Add<DebugTeleportToLatex>()
                .Add<DebugSpawnAllNPCs>()
                .Add<DebugSpawnAllBosses>()
                .Add<DebugResetBosses>()
                .Add<DebugSpawnColin>()
                //.Add<SummonShark>()
                //.Add<SummonExperiment009>()
                .Register();
        }

        /// <summary>
        /// Change the health and mana potions sold depending on progression
        /// </summary>
        /// <param name="shopName"></param>
        /// <param name="items"></param>
		public override void ModifyActiveShop(string shopName, Item[] items)
        {
            var downedEye = NPC.downedBoss1;
            var downedWorm = NPC.downedBoss2;
            var downedSkeletron = NPC.downedBoss3;

            var guidePresent = NPC.AnyNPCs(NPCID.Guide);
            var puroPresent = NPC.AnyNPCs(ModContent.NPCType<Puro>());
            var prototypePresent = NPC.AnyNPCs(ModContent.NPCType<Prototype>());

            // Potions
            if (shopName.Contains("Second Shop"))
            {
                var healthPotion = ItemID.LesserHealingPotion;
                var manaPotion = ItemID.LesserManaPotion;
                var hardMode = Main.hardMode;
                var killedAllMechs = NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3;
                var killedMoonLord = NPC.downedMoonlord;

                if (hardMode)
                {
                    if (killedMoonLord)
                    {
                        healthPotion = ItemID.SuperHealingPotion;
                        manaPotion = ItemID.SuperManaPotion;
                    }
                    else if (killedAllMechs)
                    {
                        healthPotion = ItemID.GreaterHealingPotion;
                        manaPotion = ItemID.GreaterManaPotion;
                    }
                    else
                    {
                        healthPotion = ItemID.HealingPotion;
                        manaPotion = ItemID.ManaPotion;
                    }
                }

                if (healthPotion != ItemID.LesserHealingPotion)
                {
                    foreach (Item item in items)
                    {
                        // Skip 'air' items and null items.
                        if (item == null || item.type == ItemID.None)
                        {
                            continue;
                        }

                        if (item.type == ItemID.LesserHealingPotion)
                        {
                            item.type = healthPotion;
                        }
                        else if (item.type == ItemID.LesserManaPotion)
                        {
                            item.type = manaPotion;
                        }
                    }
                }
            }

            // Fireworks
            else if (shopName.Contains("Sixth Shop"))
            {
                // Clear the shop
                for (int i = 0; i < items.Length; i++)
                    items[i] = null;

                var itemList = new List<int>()
                {
                    ItemID.RedRocket,
                    ItemID.GreenRocket,
                    ItemID.BlueRocket,
                };

                if (guidePresent)
                    itemList.Add(ModContent.ItemType<RocketGuide>());
                if (downedEye)
                    itemList.Add(ModContent.ItemType<RocketEyeOfCthulu>());
                if (downedWorm)
                    itemList.Add(ModContent.ItemType<RocketEaterOfWorlds>());
                if (downedSkeletron)
                    itemList.Add(ModContent.ItemType<RocketSkeletron>());
                
                if (puroPresent)
                    itemList.Add(ModContent.ItemType<RocketPuro>());
                if (prototypePresent)
                    itemList.Add(ModContent.ItemType<RocketPrototype>());
                if (DownedBossSystem.DownedWhiteTail)
                    itemList.Add(ModContent.ItemType<RocketWhiteTail>());

                for (int i = 0; i < itemList.Count; i++)
                {
                    if (i >= items.Length)
                        break;

                    items[i] = new Item(itemList[i]);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.HealingPotion));
        }

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
                else
                {
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
            projType = ProjectileID.NurseSyringeHurt;
            attackDelay = 20;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }

        private void UpdateHatPosition(int frameHeight)
        {
            var changedNPC = NPC.Changed();
            var frame = NPC.frame;
            var fr = frame.Top / frameHeight;
            var hatXOffset = 0;
            var hatYOffset = -46;

            // Walking
            if (fr == 1 || fr == 7 || fr == 8 || fr == 15)
            {
                hatYOffset += 2;
            }
            // Walking (18 = sitting)
            else if (fr == 4 || fr == 5 || fr == 11 || fr == 12 || fr == 18)
            {
                hatYOffset -= 2;
            }
            // Attack frame 1
            else if (fr == 21)
            {
                hatXOffset += 2;
                hatYOffset -= 2;
            }
            // Attack frame 4
            else if (fr == 24)
            {
                hatXOffset -= 2;
                hatYOffset += 2;
            }

            changedNPC.HatYOffset = hatYOffset;
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
            else if (ChangedSpecialModClientConfig.Instance.Holidays)
            {
                switch (SeasonSystem.season)
                {
                    case SeasonalEvent.Birthday:
                        hatId = ItemID.PartyHat;
                        break;
                    case SeasonalEvent.Valentine:
                        hatId = ItemID.TopHat;
                        break;
                    case SeasonalEvent.Fiesta:
                        modHat = "Content/Items/SombreroHat";
                        break;
                    case SeasonalEvent.Oktoberfest:
                        changedNPC.HasBeer = true;
                        modHat = "Content/Items/AlpineHat";
                        break;
                    case SeasonalEvent.Halloween:
                        hatId = ItemID.GhostMask;
                        break;
                    case SeasonalEvent.XMas:
                        hatId = ItemID.TreeMask;
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

        public override bool PreAI()
        {
            AIPassive.Update(NPC);
            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
            UpdateHatPosition(frameHeight);
        }
    }
}