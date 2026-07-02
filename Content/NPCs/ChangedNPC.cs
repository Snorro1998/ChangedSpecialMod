using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Common.Configs;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.EmoteBubbles;
using ChangedSpecialMod.Content.Items.Food;
using ChangedSpecialMod.Content.Items.Licenses;
using ChangedSpecialMod.Content.Projectiles;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
    public static class SizeScale
    {
        public static float scaleNormal = 1f;
        public static float scaleBig = 1.2f;
        public static float scaleHuge = 1.4f;
        public static float scaleGiant = 1.6f;
        public static float scaleBehemoth = 2.5f;
    }

    public enum GooType
    {
        Invalid,
        None,
        Black,
        White,
        // Black and White can also spawn in the city ruins. These can only spawn in the black or white latex zones
        BlackOnly,
        WhiteOnly
    }

    public enum ElementType
    {
        None,
        Water,
        Wind
    }

    public enum SpawnRequirement
    {
        None,
        WhiteTail,
        WolfKing,
        Behemoth,
        Hardmode
    }

    public enum SpawnDepth
    {
        Everywhere,
        Surface,
        Cave,
    }


    public enum HatType
    {
        None,
        All,
        Rain,
        Party,
        Underground,
        Fiesta,
        Silly,
        Valentine,
        OktoberFest,
        Halloween,
        XMas
    };

    public class StatMultipliers
    {
        public double HpPerBoss;
        public double DmgPerBoss;
        public double DefPerBoss;
        public double KbPerBoss;
        public double SizePerBoss;

        public StatMultipliers(double hpPerBoss, double dmgPerBoss, double defPerBoss, double kbPerBoss, double sizePerBoss, int bossCount) 
        {
            HpPerBoss = Math.Pow(hpPerBoss, 1.0 / bossCount);
            DmgPerBoss = Math.Pow(dmgPerBoss, 1.0 / bossCount);
            DefPerBoss = Math.Pow(defPerBoss, 1.0 / bossCount);
            KbPerBoss = kbPerBoss / bossCount;
            SizePerBoss = sizePerBoss / bossCount;
        }
    }

    public class HatStruct
    {
        public int HatId = -1;
        public HatType HType = HatType.None;
        public string ModHatTexture = null;
        public int[] Offset = new int[] { 0, 0 };

        public HatStruct(int hatId, HatType hType, int[] offset, string modHatTexture = null)
        {
            HatId = hatId;
            HType = hType;
            Offset = offset;
            ModHatTexture = modHatTexture;
        }
    }

    public partial class ChangedNPC : GlobalNPC
    {
        public List<HatStruct> NewHats = new List<HatStruct>
        {
            // Rain
            new HatStruct(ItemID.RainHat, HatType.Rain, new int[] { 0, 0 }),
            new HatStruct(ItemID.UmbrellaHat, HatType.Rain, new int[] { 0, -4 }),

            // Underground
            new HatStruct(ItemID.MiningHelmet, HatType.Underground, new int[] { 0, 2 }),

            // Party
            new HatStruct(ItemID.PartyHat, HatType.Party, new int[] { 3, -3 }),

            // Silly
            new HatStruct(ItemID.FlowerBoyHat, HatType.Silly, new int[] { 0, 8 }),
            new HatStruct(ItemID.MagicHat, HatType.Silly, new int[] { 0, -1 }),
            new HatStruct(ItemID.Fez, HatType.Silly, new int[] { 2, -1 }),
            new HatStruct(ItemID.BadgersHat, HatType.Silly, new int[] {0, -1 }),
            new HatStruct(ItemID.RedHat, HatType.Silly, new int[] {0, -1 }),

            new HatStruct(ItemID.GraduationCapMaroon, HatType.Silly, new int[] { 0, 1 }),
            new HatStruct(ItemID.GraduationCapBlue, HatType.Silly, new int[] { 0, 1 }),

            new HatStruct(ItemID.BuccaneerBandana, HatType.Silly, new int[] { 4, 4 }),
            //new HatStruct(ItemID.GraduationCapBlack, HatType.Silly, new int[] { 0, -1 }),

            new HatStruct(ItemID.WizardHat, HatType.Silly, new int[] { 1, -1 }),
            new HatStruct(ItemID.WizardsHat, HatType.Silly, new int[] { 1, -1 }),

            //new HatStruct(-1, HatType.Silly,new int[] { 0, 8 }, "Content/Items/Sunglasses"),

            // Valentine
            // Heart hairpin is too big so we do a regular heart
            new HatStruct(ItemID.TheBrideHat, HatType.Valentine, new int[] { 2, 4 }),
            new HatStruct(ItemID.Heart, HatType.Valentine, new int[] { 4, -1 }),
            new HatStruct(ItemID.TopHat, HatType.Valentine, new int[] { 0, -1 }),
            new HatStruct(ItemID.GarlandHat, HatType.Valentine, new int[] {0, -1 }),

            // Oktoberfest
            new HatStruct(ItemID.BallaHat, HatType.OktoberFest, new int[] { 0, 0 }),
            new HatStruct(-1, HatType.OktoberFest,new int[] { 1, 0 }, "Content/Items/AlpineHat"),

            // Halloween
            new HatStruct(ItemID.JackOLanternMask, HatType.Halloween, new int[] { 0, 1 }),
            new HatStruct(ItemID.GhostMask, HatType.Halloween, new int[] { 0, 8 }),
            new HatStruct(ItemID.WitchHat, HatType.Halloween, new int[] { 2, 0 }),

            // Xmas
            new HatStruct(ItemID.SantaHat, HatType.XMas, new int[] { 3, -2 }),
            new HatStruct(ItemID.SnowHat, HatType.XMas, new int[] { 0, 0 }),
            new HatStruct(ItemID.TreeMask, HatType.XMas, new int[] { 3, 0 }),

            // Fiesta
            new HatStruct(-1, HatType.Fiesta,new int[] { 1, -2 }, "Content/Items/SombreroHat"),
        };
        public void RemoveHat(int hatId) => NewHats = NewHats.Where(x => x.HatId != hatId).ToList();
        public void AddHat(HatStruct hat) => NewHats.Add(hat);
        public void RemoveHatsFromType(HatType hatType) => NewHats = NewHats.Where(x => x.HType != hatType).ToList();
        public void RemoveAllHats() => NewHats = new List<HatStruct>();
        public void SetHalloweenHatsForBlackLatex() => RemoveHat(ItemID.GhostMask);
        public void SetHalloweenHatsForWhiteLatex() => RemoveHat(ItemID.WitchHat);

        public void ChangeHatPosition(int hatId, int[] offset)
        {
            var tmpHat = NewHats.Where(x => x.HatId == hatId).FirstOrDefault();
            if (tmpHat != null)
            {
                tmpHat.Offset = offset;
            }
        }

        // If the NPC is already big, we can change this value to make statscaling increase his size less
        public float BaseScaleMultiplier { get; set; } = 1.0f;

        // Hat
        public HatStruct CurrentHat = null;
        public bool MirrorHat = false;

        // Hat offset for NPC
        public float HatXOffset = 0;
        public float HatYOffset = -22;

        public bool CanHaveBeer = false;
        // Random change if he will actually carry beer. Needs CanHaveBeer set to true
        public bool HasBeer = false;
        public float BeerXOffset = 0;
        public float BeerYOffset = 0;

        public GooType GooType = GooType.Invalid;
        public ElementType ElementType = ElementType.None;
        public bool IsFish = false;

        public bool DefaultOnHitPlayer = false;
        public bool DefaultHitEffect = false;
        public float HitEffectScale = 1.0f;

        public bool Drunk = false;
        public bool DoOnSpawnExtra = false;

        public int EvolveType = -1;
        public bool CanEvolve = false;

        // Spawn params
        public SpawnRequirement spawnRequirement = SpawnRequirement.None;
        public SpawnDepth spawnDepth = SpawnDepth.Surface;

        public override bool InstancePerEntity => true;

        /*
        public override void SetDefaults(NPC entity)
        {
            base.SetDefaults(entity);
            var changedNPC = entity.Changed();
            var modLiquidNPC = entity.ModLiquid();

            if (changedNPC == null || modLiquidNPC == null)
                return;

            var liquidID = -1;

            if (changedNPC.GooType == GooType.Black)
                liquidID = LiquidLoader.LiquidType<BlackLatexLiquid>();
            else if (changedNPC.GooType == GooType.White)
                liquidID = LiquidLoader.LiquidType<WhiteLatexLiquid>();

            if (liquidID != -1)
                modLiquidNPC.moddedLiquidMovementSpeed[liquidID - LiquidID.Count] = 1.5f;
        }
        */

        // Debug method to visualize a position by spamming confetti particles at it
        public void VisualizePosition(NPC npc, int xPos, int yPos)
        {
            var dustPos = new Vector2(xPos, yPos);

            for (int i = 0; i < 10; i++)
            {
                int dustType = Main.rand.Next(139, 143);
                var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, dustType);
                dust.velocity.X += Main.rand.NextFloat(-0.05f, 0.05f);
                dust.velocity.Y += Main.rand.NextFloat(-0.05f, 0.05f);
                dust.scale *= 1f + Main.rand.NextFloat(-0.03f, 0.03f);
            }
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            // Disable all vanilla spawns if a Changed Boss is alive
            if (NPC.AnyNPCs(ModContent.NPCType<WhiteTail>()) || NPC.AnyNPCs(ModContent.NPCType<WolfKing>()) || NPC.AnyNPCs(ModContent.NPCType<Behemoth>()))
            {
                pool[0] = 0f;
            }

            // Disables vanilla spawns if in any Changed biome and nothing special is going on
            if (ChangedUtils.InChangedBiome(spawnInfo.Player) && ChangedUtils.ShouldDisableVanillaSpawn(spawnInfo.Player))
            {
                pool[0] = 0f;
            }
        }
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (ChangedUtils.InChangedBiome(player))
            {
                //defaultspawnrate = 600
                //defaultmaxspawns = 5

                // Only change the rates if the lab is not near any of those biomes
                if (!player.ZoneJungle && !player.ZoneCorrupt && !player.ZoneCrimson && !player.ZoneMeteor)
                {
                    spawnRate = (int)(spawnRate * 0.7f);    // Decrease delay between spawns 0.7
                    maxSpawns = (int)(maxSpawns * 1.3f);    // Increase maximum amount of monsters 1.3
                }

                // In vanilla, monster spawn rates are greatly reduced if you are in a town
                // Here we do the opposite if you converted a lab into housing
                if (player.townNPCs > 2)
                {
                    spawnRate = (int)(spawnRate * 0.7f);
                    maxSpawns = (int)(maxSpawns * 1.5f);
                }
            }
        }

        public override void ModifyShop(NPCShop shop)
        {
            int npcType = shop.NpcType;

            Condition conditionBlackLatexCubPet = new Condition(Language.GetText("Conditions.NightDayFullMoon"), () => DownedBossSystem.DownedWolfKing);
            Condition conditionWhiteLatexCubPet = new Condition(Language.GetText("Conditions.NightDayFullMoon"), () => DownedBossSystem.DownedBehemoth);

            if (npcType == NPCID.BestiaryGirl)
            {
                shop.InsertAfter(ItemID.LicenseBunny, ModContent.ItemType<BlackLatexCubLicense>(), conditionBlackLatexCubPet);
                shop.InsertAfter(ItemID.LicenseBunny, ModContent.ItemType<WhiteLatexCubLicense>(), conditionWhiteLatexCubPet);
            }
        }

        public List<int> GetEmoteList(NPC npc, Player closestPlayer, List<int> emoteList)
        {
            if (!ChangedSpecialModClientConfig.Instance.NPCsCanUseChangedEmotes)
                return emoteList;

            var chance = 3;
            var isPuro = npc.type == ModContent.NPCType<Puro>();
            var isDrK = npc.type == ModContent.NPCType<Scientist>();
            var isPrototype = npc.type == ModContent.NPCType<Prototype>();
            var isColin = npc.type == ModContent.NPCType<Colin>();
            var isChangedNPC = isPuro || isDrK || isPrototype || isColin;

            //if (isColin)
            //    return new List<int> { EmoteID.DebuffSilence };

            List<int> changedEmotes = new List<int>
            {
                ModContent.EmoteBubbleType<BlackGoopEmote>(),
                ModContent.EmoteBubbleType<BlackLatexEmote>(),
                ModContent.EmoteBubbleType<SquidDogEmote>(),
                ModContent.EmoteBubbleType<SweeperEmote>(),
                ModContent.EmoteBubbleType<WhiteGoopEmote>()
            };

            // Only Puro can do the orange emote
            if (isPuro)
            {
                for (int i = 0; i < 3; i++)
                    changedEmotes.Add(ModContent.EmoteBubbleType<OrangeEmote>());
            }

            foreach (var tmpNpc in Main.npc)
            {
                if (!tmpNpc.active)
                    continue;
                // Add DrK if he is present and the NPC talking is not him
                if (tmpNpc.type == ModContent.NPCType<Scientist>() && !isDrK)
                    changedEmotes.Add(ModContent.EmoteBubbleType<DrKEmote>());
                // Add Prototype if he is present and the NPC talking is not him
                else if (tmpNpc.type == ModContent.NPCType<Prototype>() && !isPrototype)
                    changedEmotes.Add(ModContent.EmoteBubbleType<PrototypeEmote>());
            }

            if (ChangedUtils.CanSpawn(SpawnRequirement.WolfKing))
            {
                changedEmotes.Add(ModContent.EmoteBubbleType<BloodStripeEmote>());
                changedEmotes.Add(ModContent.EmoteBubbleType<PurrpurrEmote>());
            }

            // Add White Tail if he has been defeated
            if (DownedBossSystem.DownedWhiteTail)
                changedEmotes.Add(ModContent.EmoteBubbleType<WhiteTailEmote>());

            if (DownedBossSystem.DownedWolfKing)
                changedEmotes.Add(ModContent.EmoteBubbleType<WolfKingEmote>());

            if (DownedBossSystem.DownedBehemoth)
                changedEmotes.Add(ModContent.EmoteBubbleType<BehemothEmote>());

            if (isChangedNPC || Main.rand.NextBool(chance))
                emoteList.AddRange(changedEmotes);

            return emoteList;
        }

        public override int? PickEmote(NPC npc, Player closestPlayer, List<int> emoteList, WorldUIAnchor otherAnchor)
        {
            emoteList = GetEmoteList(npc, closestPlayer, emoteList);
            return base.PickEmote(npc, closestPlayer, emoteList, otherAnchor);
        }

        private string GetDialogueTextNPC(string talkingNPC, string subject, Dictionary<string, string> keywords = null)
        {
            var path = $"Mods.ChangedSpecialMod.ExtraDialogue.{talkingNPC}.NPC.{subject}";
            if (Language.Exists(path))
            {
                var result = Language.GetTextValue(path);
                if (keywords != null)
                {
                    var keys = keywords.Keys.ToList();
                    foreach (var key in keys)
                        result = result.Replace("{" + key + "}", $"{keywords[key]}");
                }
                return result;
            }
            return null;
        }

        private string GetDialogueTextTransfur(string talkingNPC, string subject, Dictionary<string, string> keywords = null)
        {
            var path = $"Mods.ChangedSpecialMod.ExtraDialogue.{talkingNPC}.Transfur.{subject}";
            if (Language.Exists(path))
            {
                var result = Language.GetTextValue(path);
                if (keywords != null)
                {
                    var keys = keywords.Keys.ToList();
                    foreach (var key in keys)
                        result = result.Replace("{" + key + "}", $"{keywords[key]}");
                }
                return result;
            }
            return null;
        }

        private string GetChatTransfurAdult(int npcType, Transfur transfur, bool puroPresent, bool drkPresent)
        {
            string chat = null;
            switch (npcType)
            {
                case NPCID.DyeTrader:
                    chat = "No! Don't come any closer! I will never get the stains out of my robe if you touch it with your slimy paws!";
                    break;
                case NPCID.SantaClaus:
                    chat = "Hohoho-oh no!";
                    break;
                case NPCID.Stylist:
                    chat = "Sorry, I can't help you. This is not a dog grooming salon.";
                    break;
                case NPCID.WitchDoctor:
                    chat = "Leave, abomination, before I lose my temper.";
                    break;
                case NPCID.Wizard:
                    if (transfur.gooType == GooType.Black && puroPresent)
                        chat = "Well, hi there, Puro! What can I do for you today?";
                    else if (transfur.gooType == GooType.White && drkPresent)
                        chat = "Well, hi there, doctor K! What can I do for you today?";
                    else
                        chat = "Well, hi there! What can I do for you today?";
                    break;
            }
            return chat;
        }

        public override void GetChat(NPC npc, ref string chat)
        {
            if (!ChangedSpecialModClientConfig.Instance.ExtraDialogue)
                return;

            var player = Main.LocalPlayer;
            var changedPlayer = player?.ChangedPlayer();

            if (changedPlayer == null)
                return;

            var npcIdentifiers = GetNPCIdentifiers();
            if (!npcIdentifiers.ContainsKey(npc.type))
                return;

            var npcIdentifier = npcIdentifiers[npc.type];
            var keywords = GetChatKeyWords();

            var transfur = changedPlayer.TransfurTypeCurrent;
            string tmpChat = null;

            if (transfur != null)
            {
                bool HasTransfur(string transfurName)
                {
                    return keywords.ContainsKey($"Transfur{transfurName}");
                }

                var isBlackAdult = transfur.npcType == ModContent.NPCType<MaleDarkLatex>() || transfur.npcType == ModContent.NPCType<FemaleDarkLatex>();
                var isWhiteAdult = transfur.npcType == ModContent.NPCType<WhiteKnight>();
                var isAdult = isBlackAdult || isWhiteAdult;

                var isSquidDog = transfur.npcType == ModContent.NPCType<SquidDog>();

                if (HasTransfur("BlackGoop"))
                    tmpChat = GetDialogueTextTransfur(npcIdentifier, "BlackGoop");
                else if (HasTransfur("WhiteGoop"))
                    tmpChat = GetDialogueTextTransfur(npcIdentifier, "WhiteGoop");

                else if (HasTransfur("BlackCub"))
                {
                    if (npc.type == NPCID.TaxCollector && !keywords.ContainsKey("HasDogLicense"))
                        tmpChat = GetDialogueTextTransfur(npcIdentifier, "BlackCubNoLicense", keywords);
                    else
                        tmpChat = GetDialogueTextTransfur(npcIdentifier, "BlackCub", keywords);
                }
                else if (HasTransfur("WhiteCub"))
                {
                    if (npc.type == NPCID.TaxCollector && !keywords.ContainsKey("HasDogLicense"))
                        tmpChat = GetDialogueTextTransfur(npcIdentifier, "WhiteCubNoLicense", keywords);
                    else
                        tmpChat = GetDialogueTextTransfur(npcIdentifier, "WhiteCub", keywords);
                }

                /*
                else if (isAdult)
                    tmpChat = GetChatTransfurAdult(npc.type, transfur, puroPresent, drkPresent);
                */
                else if (isSquidDog)
                    tmpChat = GetDialogueTextTransfur(npcIdentifier, "SquidDog");
                
            }
            else if (Main.rand.NextBool(8))
            {
                string randomChat = null;
                List<string> chatOptions = new List<string>();

                var npcNames = new List<string>()
                {
                    "Puro",
                    "Prototype",
                    "Scientist"
                };

                foreach (var npcName in npcNames)
                {
                    if (!keywords.ContainsKey($"{npcName}Present"))
                        continue;
                    randomChat = GetDialogueTextNPC(npcIdentifier, npcName, keywords);
                    if (randomChat != null)
                        chatOptions.Add(randomChat);
                }

                if (chatOptions.Count > 0)
                    tmpChat = chatOptions[Main.rand.Next(chatOptions.Count)];
            }

            if (tmpChat != null && tmpChat != "None")
                chat = tmpChat;
        }

        private Dictionary<int, string> GetNPCIdentifiers()
        {
            var npcIdentifiers = new Dictionary<int, string>()
            {
                // Vanilla
                { NPCID.Angler,         "Angler" },
                { NPCID.BestiaryGirl,   "Zoologist" },
                { NPCID.Clothier,       "Clothier" },
                { NPCID.Cyborg,         "Cyborg" },
                { NPCID.DD2Bartender,   "TavernKeep" },
                { NPCID.Demolitionist,  "Demolitionist" },
                { NPCID.Dryad,          "Dryad" },
                { NPCID.DyeTrader,      "DyeTrader" },
                { NPCID.Golfer,         "Golfer" },
                { NPCID.Guide,          "Guide" },
                { NPCID.Mechanic,       "Mechanic" },
                { NPCID.Merchant,       "Merchant" },
                { NPCID.Nurse,          "Nurse" },
                { NPCID.Painter,        "Painter" },
                { NPCID.PartyGirl,      "PartyGirl" },
                { NPCID.Pirate,         "Pirate" },
                { NPCID.SantaClaus,     "SantaClaus" },
                { NPCID.Stylist,        "Stylist" },
                { NPCID.TaxCollector,   "TaxCollector" },
                { NPCID.WitchDoctor,    "WitchDoctor" },
                { NPCID.Wizard,         "Wizard" },

                // Changed
                { ModContent.NPCType<Puro>(), "Puro" },
                { ModContent.NPCType<Prototype>(), "Prototype" },
                { ModContent.NPCType<Scientist>(), "Scientist" },
            };

            void AddModdedNPC(Mod mod, string identifier, string localizationName)
            {
                if (mod.TryFind<ModNPC>(identifier, out ModNPC tmpNPC))
                    npcIdentifiers.Add(tmpNPC.Type, localizationName);
            }

            // These are not used at the moment, but can be if you add entries in the localization
            var modThorium = ModSupportSystem.modThorium;
            if (modThorium != null)
            {
                AddModdedNPC(modThorium, "Cobbler", "ThoriumCobbler");
                AddModdedNPC(modThorium, "DesertAcolyte", "ThoriumDesertAcolyte");
                AddModdedNPC(modThorium, "Cook", "ThoriumCook");
                AddModdedNPC(modThorium, "ConfusedZombie", "ThoriumConfusedZombie");
                AddModdedNPC(modThorium, "Blacksmith", "ThoriumBlacksmith");
                AddModdedNPC(modThorium, "Tracker", "ThoriumTracker");
                AddModdedNPC(modThorium, "Diverman", "ThoriumDiverman");
                AddModdedNPC(modThorium, "Druid", "ThoriumDruid");
                AddModdedNPC(modThorium, "Spiritualist", "ThoriumSpiritualist");
                AddModdedNPC(modThorium, "WeaponMaster", "ThoriumWeaponMaster");
            }

            var modCalamity = ModSupportSystem.modCalamity;
            if (modCalamity != null)
            {
                AddModdedNPC(modCalamity, "DILF", "CalamityArchmage");
                AddModdedNPC(modCalamity, "THIEF", "CalamityBandit");
                AddModdedNPC(modCalamity, "SEAHOE", "CalamitySeaKing");
                AddModdedNPC(modCalamity, "WITCH", "CalamityBrimstoneWitch");
            }

            var modCoralite = ModSupportSystem.modCoralite;
            if (modCoralite != null)
            {
                AddModdedNPC(modCoralite, "CrystalRobot", "CoroliteCrystalRobot");
                AddModdedNPC(modCoralite, "ElfRanger", "CoroliteElfRanger");
            }

            return npcIdentifiers;
        }

        public Dictionary<string, string> GetChatKeyWords()
        {
            var keyWords = new Dictionary<string, string>() { };
            var npcIdentifiers = GetNPCIdentifiers();

            void AddIf(bool condition, string key, string value = "")
            {
                if (condition)
                    keyWords.Add(key, value);
            }

            // Check which NPCs on the list are present and add keywords for them
            var npcIDs = npcIdentifiers.Keys.ToList();
            foreach (var npcID in npcIDs)
            {
                var npcIndex = NPC.FindFirstNPC(npcID);
                if (npcIndex != -1)
                {
                    var npcIdentifier = npcIdentifiers[npcID];
                    var tmpNPC = Main.npc[npcIndex];
                    keyWords.Add($"Name{npcIdentifier}", tmpNPC.GivenName);
                    keyWords.Add($"{npcIdentifier}Present", string.Empty);
                }
            }

            // Bosses slain
            if (DownedBossSystem.DownedBehemoth) keyWords.Add("Behemoth", string.Empty);

            var player = Main.LocalPlayer;
            if (player != null)
            {
                var changedPlayer = player.ChangedPlayer();
                keyWords.Add("NamePlayer", player.name);

                var hasNormalBook = player.HasItem(ItemID.Book);
                var hasBookOfSkulls = player.HasItem(ItemID.BookofSkulls);
                var hasWaterBolt = player.HasItem(ItemID.WaterBolt);
                var hasDemonScythe = player.HasItem(ItemID.DemonScythe);

                var hasCrystalStorm = player.HasItem(ItemID.CrystalStorm);
                var hasCursedFlames = player.HasItem(ItemID.CursedFlames);
                var hasGoldenShower = player.HasItem(ItemID.GoldenShower);
                var hasRazorblade = player.HasItem(ItemID.RazorbladeTyphoon);
                var hasMagnetSphere = player.HasItem(ItemID.MagnetSphere);
                var hasLunarFlare = player.HasItem(ItemID.LunarFlareBook);

                var hasAnyBook = (hasNormalBook || hasBookOfSkulls || hasWaterBolt || hasDemonScythe || hasCrystalStorm || hasCursedFlames ||
                    hasGoldenShower || hasRazorblade || hasMagnetSphere || hasLunarFlare);

                var orangeItem = player.inventory.FirstOrDefault(x => x.type == ModContent.ItemType<Orange>());
                if (orangeItem != null)
                {
                    keyWords.Add("PlayerHasOrange", string.Empty);
                    if (orangeItem.stack >= 20)
                        keyWords.Add("PlayerHasManyOranges", string.Empty);
                }

                AddIf(hasAnyBook, "PlayerHasBook");
                AddIf(hasBookOfSkulls, "PlayerHasBookOfSkulls");
                AddIf(hasWaterBolt, "PlayerHasWaterBolt");
                AddIf(hasGoldenShower, "PlayerHasGoldenShower");

                var wearingPartyHat = player.armor.Any(item => item.type == ItemID.PartyHat);
                if (!wearingPartyHat)
                    keyWords.Add("PlayerHasNoPartyHat", string.Empty);
                if (ChangedUtils.PlayerIsWearingBalloon(player))
                    keyWords.Add("PlayerIsWearingBalloon", string.Empty);
                if (ChangedUtils.PlayerIsWearingWeddingDress(player))
                    keyWords.Add("PlayerIsWearingWeddingDress", string.Empty);

                AddIf(NPC.boughtDog, "HasDogLicense");

                // Transfur
                var playerTransfur = changedPlayer.TransfurTypeCurrent;

                if (playerTransfur != null)
                {
                    var transfurBlackGoop = playerTransfur.npcType == ModContent.NPCType<BlackGoop>();
                    var transfurWhiteGoop = playerTransfur.npcType == ModContent.NPCType<WhiteGoop>();
                    var transfurBlackCub = playerTransfur.npcType == ModContent.NPCType<DarkLatexCub>();
                    var transfurWhiteCub = playerTransfur.npcType == ModContent.NPCType<WhiteLatexCub>();

                    if (transfurBlackGoop)
                    {
                        keyWords.Add("TransfurGoop", string.Empty);
                        keyWords.Add("TransfurBlackGoop", string.Empty);
                    }
                    else if (transfurWhiteGoop)
                    {
                        keyWords.Add("TransfurGoop", string.Empty);
                        keyWords.Add("TransfurWhiteGoop", string.Empty);
                    }
                    else if (transfurBlackCub)
                    {
                        keyWords.Add("TransfurCub", string.Empty);
                        keyWords.Add("TransfurBlackCub", string.Empty);
                    }
                    else if (transfurWhiteCub)
                    {
                        keyWords.Add("TransfurCub", string.Empty);
                        keyWords.Add("TransfurWhiteCub", string.Empty);
                    }
                }

                // Iterate projectile list
                foreach (var projectile in Main.ActiveProjectiles)
                {
                    if (projectile.type == ModContent.ProjectileType<PurrpurrStaffProjectile>() && projectile.owner == Main.myPlayer)
                    {
                        keyWords.Add("PlayerHasPurrpurr", string.Empty);
                        break;
                    }
                }
            }

            // World evil
            AddIf(WorldGen.tBlood > 0, "Crimson");
            AddIf(WorldGen.tEvil > 0, "Corruption");
            AddIf(WorldGen.tGood > 0, "Hallow");

            // Weather
            AddIf(Main.IsItRaining, "Rain");
            AddIf(Main.IsItStorming, "Thunder");
            AddIf(ChangedUtils.IsItWindy(), "Windy");

            // Seasons
            string seasonKeyWord = null;
            switch (SeasonSystem.season)
            {
                case SeasonalEvent.Valentine:
                    seasonKeyWord = "Valentine";
                    break;
                case SeasonalEvent.Easter:
                    seasonKeyWord = "Easter";
                    break;
                case SeasonalEvent.Oktoberfest:
                    seasonKeyWord = "Oktoberfest";
                    break;
                case SeasonalEvent.Halloween:
                    seasonKeyWord = "Halloween";
                    break;
                case SeasonalEvent.XMas:
                    seasonKeyWord = "Xmas";
                    break;
            }
            if (seasonKeyWord != null)
            {
                keyWords.Add(seasonKeyWord, string.Empty);
            }

            return keyWords;
        }

        public void SetNPCName(NPC npc, string overwriteName = null)
        {
            // Default fallback
            string npcName = "Latex Beast";

            // Check if NPC is a ModNPC (vanilla NPCs don't have ModNPC)
            if (npc.ModNPC != null)
            {
                string typeName = npc.ModNPC.GetType().Name;
                string key = $"Mods.ChangedSpecialMod.NPCs.{typeName}.DisplayName";
                string localized = Language.GetTextValue(key);

                if (!string.IsNullOrEmpty(localized))
                    npcName = localized;
            }

            if (overwriteName != null)
                npcName = overwriteName;

            var scale = npc.scale / BaseScaleMultiplier;
            var sizeDescription = GetSizeDescription(npc);
            npc.GivenName = $"{sizeDescription} {npcName}";
        }

        public string GetSizeDescription(NPC npc)
        {
            float scale = npc.scale / BaseScaleMultiplier;
            string sizeKey = null;

            if (scale >= SizeScale.scaleBehemoth)
                sizeKey = "Behemoth";
            else if (scale >= SizeScale.scaleGiant)
                sizeKey = "Giant";
            else if (scale >= SizeScale.scaleHuge)
                sizeKey = "Huge";
            else if (scale >= SizeScale.scaleBig)
                sizeKey = "Big";

            string sizeDescription = sizeKey != null
                ? Language.GetTextValue($"Mods.ChangedSpecialMod.SizeDescription.{sizeKey}")
                : string.Empty;

            return sizeDescription;
        }

        public void AdjustStatScaling(NPC npc)
        {
            if (!ChangedSpecialModClientConfig.Instance.Evolution)
            {
                npc.scale = BaseScaleMultiplier;
                return;
            }

            var player = Main.LocalPlayer;

            // TODO add these when created
            // DownedBossSystem.DownedShark
            // Squid Dog
            // Hyena

            var preHardmodeBossesDowned = new bool[]
            {
                NPC.downedSlimeKing,                // King Slime
                NPC.downedBoss1,                    // Eye of Chtulu
                NPC.downedBoss2,                    // Brain or EOF
                NPC.downedBoss3,                    // Skeletron
                NPC.downedDeerclops,                // Deerclops
                NPC.downedQueenBee,                 // Queen Bee
                
                DownedBossSystem.DownedWhiteTail,   // White tail
                DownedBossSystem.DownedWolfKing,    // Wolf King
                DownedBossSystem.DownedBehemoth,    // Behemoth
            };

            var earlyHardmodeBossesDowned = new bool[]
            {
                NPC.downedQueenSlime,               // Queen Slime
                NPC.downedMechBoss1,                // Destroyer
                NPC.downedMechBoss2,                // Twins
                NPC.downedMechBoss3,                // Skeleton Prime
            };

            var hardmodeBossesDowned = new bool[]
            {
                NPC.downedPlantBoss,                // Plantera
                NPC.downedEmpressOfLight,           // Empress of Light
                NPC.downedGolemBoss,                // Golem
                NPC.downedFishron,                  // Duke Fishron
            };

            var moonEventDowned = new bool[]
            {
                NPC.downedAncientCultist,           // Cultist
                NPC.downedTowerNebula,              // Nebula pillar
                NPC.downedTowerSolar,               // Solar pillar
                NPC.downedTowerStardust,            // Stardust pillar
                NPC.downedTowerVortex,              // Vortex pillar
            };

            var unusedDowned = new bool[]
            {
                NPC.downedGoblins,                  // Goblin army
                NPC.downedPirates,                  // Pirate invasion
                NPC.downedMartians,                 // Martian madness
                NPC.downedFrost                     // Frost legion - You can only do this during xmas
            };

            // Prehardmode
            var preHardmodeMultipliers = new StatMultipliers(2.0, 1.3, 1.5, 0.1, 0.2, preHardmodeBossesDowned.Length);      //160
            var enterHardmodeMultipliers = new StatMultipliers(2.5, 1.6, 1.8, 0.05, 0.1, 1);                                //400

            // Hardmode
            var earlyHardModeMultipliers = new StatMultipliers(1.5, 1.1, 1.1, 0.1, 0.1, earlyHardmodeBossesDowned.Length);   //600
            var hardmodeMultipliers = new StatMultipliers((1D + 1D / 3D), 1.2, 1.1, 1.0, 0.05, hardmodeBossesDowned.Length); //800

            // Lunar
            var moonEventMultipliers = new StatMultipliers(1.25, 1.1, 1.1, 1.0, 0.05, moonEventDowned.Length);               //1000   
            var moonLordMultipliers = new StatMultipliers(1.2, 1.1, 1.1, 1.0, 0.1, 1);                                       //1200

            double tmpLifeMax = npc.lifeMax;
            double tmpDmg = npc.damage;
            double tmpDef = npc.defense;
            double tmpKnockbackResistance = 1.0 - npc.knockBackResist;
            double tmpSize = 1.0;

            for (int i = 0; i < preHardmodeBossesDowned.Length; i++)
            {
                if (preHardmodeBossesDowned[i])
                {
                    tmpLifeMax *= preHardmodeMultipliers.HpPerBoss;
                    tmpDmg *= preHardmodeMultipliers.DmgPerBoss;
                    tmpDef *= preHardmodeMultipliers.DefPerBoss;
                    tmpKnockbackResistance += preHardmodeMultipliers.KbPerBoss;
                    tmpSize += preHardmodeMultipliers.SizePerBoss;
                }
            }

            // Wall of Flesh
            if (Main.hardMode)
            {
                tmpLifeMax *= enterHardmodeMultipliers.HpPerBoss;
                tmpDmg *= enterHardmodeMultipliers.DmgPerBoss;
                tmpDef *= enterHardmodeMultipliers.DefPerBoss;
                tmpKnockbackResistance += enterHardmodeMultipliers.KbPerBoss;
                tmpSize += enterHardmodeMultipliers.SizePerBoss;
            }

            for (int i = 0; i < earlyHardmodeBossesDowned.Length; i++)
            {
                if (earlyHardmodeBossesDowned[i])
                {
                    tmpLifeMax *= earlyHardModeMultipliers.HpPerBoss;
                    tmpDmg *= earlyHardModeMultipliers.DmgPerBoss;
                    tmpDef *= earlyHardModeMultipliers.DefPerBoss;
                    tmpKnockbackResistance += earlyHardModeMultipliers.KbPerBoss;
                    tmpSize += earlyHardModeMultipliers.SizePerBoss;
                }
            }

            for (int i = 0; i < hardmodeBossesDowned.Length; i++)
            {
                if (hardmodeBossesDowned[i])
                {
                    tmpLifeMax *= hardmodeMultipliers.HpPerBoss;
                    tmpDmg *= hardmodeMultipliers.DmgPerBoss;
                    tmpDef *= hardmodeMultipliers.DefPerBoss;
                    tmpKnockbackResistance += hardmodeMultipliers.KbPerBoss;
                    tmpSize += hardmodeMultipliers.SizePerBoss;
                }
            }

            // Cultist and pillars
            for (int i = 0; i < moonEventDowned.Length; i++)
            {
                if (moonEventDowned[i])
                {
                    tmpLifeMax *= moonEventMultipliers.HpPerBoss;
                    tmpDmg *= moonEventMultipliers.DmgPerBoss;
                    tmpDef *= moonEventMultipliers.DefPerBoss;
                    tmpKnockbackResistance += moonEventMultipliers.KbPerBoss;
                    tmpSize += moonEventMultipliers.SizePerBoss;
                }
            }

            // Moon Lord
            if (NPC.downedMoonlord)
            {
                tmpLifeMax *= moonLordMultipliers.HpPerBoss;
                tmpDmg *= moonLordMultipliers.DmgPerBoss;
                tmpDef *= moonLordMultipliers.DefPerBoss;
                tmpKnockbackResistance += moonLordMultipliers.KbPerBoss;
                tmpSize += moonLordMultipliers.SizePerBoss;
            }

            npc.lifeMax = (int)tmpLifeMax;
            var remainder = npc.lifeMax % 5;
            if (remainder > 0) npc.lifeMax += 5 - remainder;
            npc.damage = (int)tmpDmg;
            npc.defense = (int)tmpDef;
            npc.knockBackResist = (float)Math.Max(1.0f - tmpKnockbackResistance, 0.0f);
            npc.scale *= (float)tmpSize * BaseScaleMultiplier;
        }

        public void PickHat(NPC npc)
        {
            var player = Main.LocalPlayer;
            Drunk = ChangedUtils.IsDrunk(player);
            var WornHatType = HatType.None;

            if (Drunk)
                WornHatType = HatType.All;
            else if (BirthdayParty.PartyIsUp)
                WornHatType = HatType.Party;
            // Don't wear a rain hat if underground
            else if (Main.IsItRaining && npc.Center.Y < Main.worldSurface * 16)
                WornHatType = HatType.Rain;
            else if (SeasonSystem.season == SeasonalEvent.Birthday)
                WornHatType = HatType.Party;
            else if (SeasonSystem.season == SeasonalEvent.Valentine)
                WornHatType = HatType.Valentine;
            else if (SeasonSystem.season == SeasonalEvent.Oktoberfest)
            {
                // add a chance?
                HasBeer = CanHaveBeer;
                WornHatType = HatType.OktoberFest;
            }
            else if (SeasonSystem.season == SeasonalEvent.Halloween)
                WornHatType = HatType.Halloween;
            else if (SeasonSystem.season == SeasonalEvent.XMas)
                WornHatType = HatType.XMas;
            else if (Main.rand.NextBool(30) && npc.Center.Y > Main.worldSurface)
                WornHatType = HatType.Underground;

            var hatOptions = new List<HatStruct> { };
            if (WornHatType == HatType.All)
                hatOptions = NewHats;
            else if (WornHatType != HatType.None)
                hatOptions = NewHats.Where(x => x.HType == WornHatType).ToList();

            if (hatOptions.Any())
            {
                var hat = hatOptions[Main.rand.Next(0, hatOptions.Count)];
                CurrentHat = hat;
            }
        }

        public void RemoveHat()
        {
            CurrentHat = null;
        }

        public void SetHat(HatStruct hat)
        {
            CurrentHat = hat;
        }


        public void SetHat(int hatId)
        {
            var hat = NewHats.FirstOrDefault(x => x.HatId == hatId);
            if (hat != null)
                CurrentHat = hat;
        }

        public void SetHat(string modHatTexture)
        {
            var hat = NewHats.FirstOrDefault(x => x.ModHatTexture == modHatTexture);
            if (hat != null)
                CurrentHat = hat;
        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            var changedNPC = npc.Changed();
            if (changedNPC != null && changedNPC.DoOnSpawnExtra)
                OnSpawnExtra(npc);
            base.OnSpawn(npc, source);

        }

        public void OnSpawnExtra(NPC npc)
        {
            PickHat(npc);
            if (SeasonSystem.season == SeasonalEvent.Valentine && Main.rand.NextBool(3))
                npc.AddBuff(BuffID.Lovestruck, 60 * 20);
        }

        public void DrawBeerGlass(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var glassTexture = Mod.Assets.Request<Texture2D>("Assets/Textures/Beer").Value;
            var ChangedNPC = npc.Changed();
            // Base NPC draw position (matches vanilla)
            Vector2 drawPos = npc.Center - screenPos;

            // Remove gfxOffY so it doesn't get applied twice
            drawPos.Y += npc.gfxOffY;

            var vals = CurrentHat.Offset;

            // Adjust for head position
            var xOffset = ChangedNPC.BeerXOffset;
            var yOffset = ChangedNPC.BeerYOffset;
            Vector2 hatOffset = new Vector2(-npc.spriteDirection * xOffset * npc.scale, yOffset * npc.scale);

            SpriteEffects effects = npc.spriteDirection == -1
                ? SpriteEffects.FlipHorizontally
                : SpriteEffects.None;

            spriteBatch.Draw(
                glassTexture,
                drawPos + hatOffset,
                null,
                drawColor,
                npc.rotation,
                glassTexture.Size() / 2f,
                npc.scale / ChangedNPC.BaseScaleMultiplier,
                effects,
                0f
            );
        }

        public void PostDrawExtra(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var changedNPC = npc.Changed();
            if (CurrentHat != null)
            {
                //Content/Items/AlpineHat
                Texture2D hatTexture = null;
                if (CurrentHat.ModHatTexture != null)
                    hatTexture = Mod.Assets.Request<Texture2D>(CurrentHat.ModHatTexture).Value;
                else
                    hatTexture = ModContent.Request<Texture2D>("Terraria/Images/Item_" + CurrentHat.HatId).Value;

                // Base NPC draw position (matches vanilla)
                Vector2 drawPos = npc.Center - screenPos;

                // Remove gfxOffY so it doesn't get applied twice
                drawPos.Y += npc.gfxOffY;

                var vals = CurrentHat.Offset;

                // Adjust for head position
                var xOffset = HatXOffset + vals[0];
                var yOffset = HatYOffset + vals[1];

                var direction = npc.spriteDirection;
                if (changedNPC.MirrorHat)
                    direction *= -1;

                Vector2 hatOffset = new Vector2(-direction * xOffset * npc.scale, yOffset * npc.scale);

                SpriteEffects effects = direction == -1
                    ? SpriteEffects.FlipHorizontally
                    : SpriteEffects.None;

                spriteBatch.Draw(
                    hatTexture,
                    drawPos + hatOffset,
                    null,
                    drawColor,
                    npc.rotation,
                    hatTexture.Size() / 2f,
                    npc.scale / changedNPC.BaseScaleMultiplier,
                    effects,
                    0f
                );

                if (CurrentHat.HatId == ItemID.MiningHelmet)
                {
                    Lighting.AddLight(npc.Center, 1f, 1f, 1f);
                }
            }

            if (changedNPC.HasBeer)
                DrawBeerGlass(npc, spriteBatch, screenPos, drawColor);
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            base.OnHitPlayer(npc, target, hurtInfo);
            var changedNPC = npc.Changed();
            if (changedNPC == null || !changedNPC.DefaultOnHitPlayer)
                return;
            if (hurtInfo.Damage > 0 && ChangedSpecialModClientConfig.Instance.TransfurSound && !ChangedSpecialModClientConfig.Instance.NPCsCanTransfurPlayer)
            {
                AudioSystem.PlaySoundWithProbability(Sounds.SoundTransfur, npc.Center, 3);
            }
        }

        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {
            base.HitEffect(npc, hit);
            var changedNPC = npc.Changed();
            if (changedNPC == null || !changedNPC.DefaultHitEffect)
                return;

            var dustType = DustID.Asphalt;
            if (changedNPC.GooType == GooType.White || changedNPC.GooType == GooType.WhiteOnly)
                dustType = DustID.SnowBlock;

            var nParticles = 10;
            if (npc.life <= 0) nParticles = 40;
            for (int i = 0; i < nParticles; i++)
            {
                var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, dustType, 0, 0, 1, Color.White);
                dust.velocity.X += Main.rand.NextFloat(-0.05f, 0.05f);
                dust.velocity.Y += Main.rand.NextFloat(-0.05f, 0.05f);
                dust.scale *= npc.scale * changedNPC.HitEffectScale + Main.rand.NextFloat(-0.03f, 0.03f);
            }
        }

        private void DoRandomEmote(NPC npc)
        {
            var chancedNPC = npc.Changed();
            if (chancedNPC != null && chancedNPC.GooType != GooType.Invalid && npc.HasValidTarget && npc.HasBuff(BuffID.Lovestruck))
            {
                int emoteLove = 0;
                int emoteKiss = 88;
                var emoteId = ChangedUtils.Choose(emoteLove, emoteKiss);
                var player = Main.player[npc.target];
                if (player.Distance(npc.Center) < 160 && Main.rand.NextBool(200))
                {
                    EmoteBubble.NewBubble(emoteId, new WorldUIAnchor(npc), 90);
                }
            }
        }

        public void CheckForEvolution(NPC npc)
        {
            if (npc == null || Main.netMode == NetmodeID.MultiplayerClient)
                return;

            var changedNPC = npc.Changed();
            
            if (changedNPC == null || !changedNPC.CanEvolve)
                return;

            foreach (var tmpNpc in Main.npc)
            {
                if (tmpNpc == null || !tmpNpc.active || tmpNpc.type != npc.type || tmpNpc.whoAmI == npc.whoAmI)
                    continue;

                if (npc.Distance(tmpNpc.Center) < 16)
                {
                    TransfurSystem.TransfurEffect(npc);
                    var npcIndex = NPC.NewNPC(new EntitySource_WorldEvent(), (int)npc.Center.X, (int)npc.Bottom.Y, changedNPC.EvolveType, 0, 0);

                    if (Main.netMode == NetmodeID.Server && npcIndex != -1)
                        NetMessage.SendData(MessageID.SyncNPC, number: npcIndex);

                    ChangedUtils.DespawnNPC(tmpNpc);
                    ChangedUtils.DespawnNPC(npc);
                }
            }
        }

        public override bool CanBeHitByNPC(NPC npc, NPC attacker)
        {
            var changedNPC = npc.Changed();
            var attackerChangedNPC = attacker.Changed();
            if (changedNPC != null && attackerChangedNPC != null)
                return true;

            if (changedNPC.GooType != GooType.Invalid && changedNPC.GooType == attackerChangedNPC.GooType)
                return false;
            return true;
        }

        public override void AI(NPC npc)
        {
            base.AI(npc);
            DoRandomEmote(npc);
            CheckForEvolution(npc);
        }
    }
}
