using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.EmoteBubbles;
using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Content.Projectiles;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
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

    public enum Category
    {
        None,
        Goop,
        Cub,
        Adult,
        Flying,
        Fast,
        Beefy
    }

    public enum ElementType
    {
        None,
        Water,
        Wind
    }

    public enum HatType
    {
        None,
        All,
        Rain,
        Party,
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
        };

        public void ChangeHatPosition(int hatId, int[] offset)
        {
            var tmpHat = NewHats.Where(x => x.HatId == hatId).FirstOrDefault();
            if (tmpHat != null)
            {
                tmpHat.Offset = offset;
            }
        }

        public void RemoveHat(int hatId)
        {
            NewHats = NewHats.Where(x => x.HatId != hatId).ToList();
        }

        public void AddHat(HatStruct hat)
        {
            NewHats.Add(hat);
        }

        public void RemoveHatsFromType(HatType hatType)
        {
            NewHats = NewHats.Where(x => x.HType != hatType).ToList();
        }

        public void RemoveAllHats()
        {
            NewHats = new List<HatStruct>();
        }

        public void SetHalloweenHatsForBlackLatex()
        {
            RemoveHat(ItemID.GhostMask);
        }

        public void SetHalloweenHatsForWhiteLatex()
        {
            RemoveHat(ItemID.WitchHat);
        }

        public float scaleNormal { get; set; } = 1f;
        public float scaleBig { get; set; } = 1.2f;
        public float scaleHuge { get; set; } = 1.4f;
        public float scaleGiant { get; set; } = 1.6f;//1.8
        public float scaleBehemoth { get; set; } = 2.5f;

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

        public override bool InstancePerEntity => true;

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

        public List<int> GetEmoteList(NPC npc, Player closestPlayer, List<int> emoteList)
        {
            // Chance to add Changed emotes to the list of options and will always do it when living in a Changed biome
            var chance = ChangedUtils.InChangedBiome(closestPlayer) ? 1 : 2;
            var isPuro = npc.type == ModContent.NPCType<Puro>();
            var isDrK = npc.type == ModContent.NPCType<Scientist>();
            var isPrototype = npc.type == ModContent.NPCType<Prototype>();
            var isChangedNPC = isPuro || isDrK || isPrototype;

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

            if (ChangedUtils.CanSpawnStrongLatex())
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
            {
                emoteList.AddRange(changedEmotes);
            }
            /*
            if (isChangedNPC)
            {
                emoteList.Clear();
                emoteList = changedEmotes;
            }
            else if (Main.rand.NextBool(chance))
            {
                emoteList.AddRange(changedEmotes);
            }
            */

            emoteList.AddRange(changedEmotes);

            return emoteList;
        }

        // This works for vanilla NPCs but somehow not for Changed NPCs
        public override int? PickEmote(NPC npc, Player closestPlayer, List<int> emoteList, WorldUIAnchor otherAnchor)
        {
            emoteList = GetEmoteList(npc, closestPlayer, emoteList);
            return base.PickEmote(npc, closestPlayer, emoteList, otherAnchor);
        }

        public Dictionary<string, string> GetChatKeyWords()
        {
            var keyWords = new Dictionary<string, string>() { };

            // NPCs present
            var npc = NPC.FindFirstNPC(NPCID.Dryad);
            if (npc >= 0) keyWords.Add("NameDryad", Main.npc[npc].GivenName);
            npc = NPC.FindFirstNPC(NPCID.Guide);
            if (npc >= 0) keyWords.Add("NameGuide", Main.npc[npc].GivenName);
            npc = NPC.FindFirstNPC(NPCID.Merchant);
            if (npc >= 0) keyWords.Add("NameMerchant", Main.npc[npc].GivenName);
            npc = NPC.FindFirstNPC(NPCID.DD2Bartender);
            if (npc >= 0) keyWords.Add("NameTavernKeep", Main.npc[npc].GivenName);
            npc = NPC.FindFirstNPC(NPCID.Nurse);
            if (npc >= 0) keyWords.Add("NameNurse", Main.npc[npc].GivenName);
            npc = NPC.FindFirstNPC(NPCID.Angler);
            if (npc >= 0) keyWords.Add("NameAngler", Main.npc[npc].GivenName);
            npc = NPC.FindFirstNPC(NPCID.BestiaryGirl);
            if (npc >= 0) keyWords.Add("NameZoologist", Main.npc[npc].GivenName);
            npc = NPC.FindFirstNPC(ModContent.NPCType<Puro>());
            if (npc >= 0) keyWords.Add("Puro", "");
            npc = NPC.FindFirstNPC(ModContent.NPCType<Scientist>());
            if (npc >= 0) keyWords.Add("DrK", "");
            npc = NPC.FindFirstNPC(ModContent.NPCType<Prototype>());
            if (npc >= 0) keyWords.Add("Prototype", "");

            // Bosses slain
            if (DownedBossSystem.DownedBehemoth) keyWords.Add("Behemoth", "");


            var player = Main.LocalPlayer;
            if (player != null)
            {
                var changedPlayer = player.ChangedPlayer();

                keyWords.Add("NamePlayer", player.name);

                var hasNormalBook = player.inventory.Any(item => item.type == ItemID.Book);
                
                var hasBookOfSkulls = player.inventory.Any(item => item.type == ItemID.BookofSkulls);
                var hasWaterBolt = player.inventory.Any(item => item.type == ItemID.WaterBolt);
                var hasDemonScythe = player.inventory.Any(item => item.type == ItemID.DemonScythe);

                var hasCrystalStorm = player.inventory.Any(item => item.type == ItemID.CrystalStorm);
                var hasCursedFlames = player.inventory.Any(item => item.type == ItemID.CursedFlames);
                var hasGoldenShower = player.inventory.Any(item => item.type == ItemID.GoldenShower);
                var hasRazorblade = player.inventory.Any(item => item.type == ItemID.RazorbladeTyphoon);
                var hasMagnetSphere = player.inventory.Any(item => item.type == ItemID.MagnetSphere);
                var hasLunarFlare = player.inventory.Any(item => item.type == ItemID.LunarFlareBook);

                var hasAnyBook = (hasNormalBook || hasBookOfSkulls || hasWaterBolt || hasDemonScythe || hasCrystalStorm || hasCursedFlames ||
                    hasGoldenShower || hasRazorblade || hasMagnetSphere || hasLunarFlare);

                var hasCubTransform = changedPlayer.transfurIndex == TransfurType.BlackCub || changedPlayer.transfurIndex == TransfurType.WhiteCub;

                if (player.inventory.Any(item => item.type == ModContent.ItemType<Orange>()))
                    keyWords.Add("PlayerHasOrange", "");
                if (hasAnyBook)
                    keyWords.Add("PlayerHasBook", "");
                if (hasBookOfSkulls)
                    keyWords.Add("PlayerHasBookOfSkulls", "");
                if (hasWaterBolt)
                    keyWords.Add("PlayerHasWaterBolt", "");
                if (hasGoldenShower)
                    keyWords.Add("PlayerHasGoldenShower", "");
                var wearingPartyHat = player.armor.Any(item => item.type == ItemID.PartyHat);
                if (!wearingPartyHat)
                    keyWords.Add("PlayerHasNoPartyHat", "");
                if (ChangedUtils.PlayerIsWearingBalloon(player))
                    keyWords.Add("PlayerIsWearingBalloon", "");
                if (ChangedUtils.PlayerIsWearingWeddingDress(player))
                    keyWords.Add("PlayerIsWearingWeddingDress", "");

                if (hasCubTransform)
                    keyWords.Add("TransfurCub", "");

                // Iterate projectile list
                foreach (var projectile in Main.ActiveProjectiles)
                {
                    if (projectile.type == ModContent.ProjectileType<PurrpurrStaffProjectile>() && projectile.owner == Main.myPlayer)
                    {
                        keyWords.Add("PlayerHasPurrpurr", "");
                        break;
                    }
                }
            }

            // Number of crimson blocks in the world
            if (WorldGen.tBlood > 0)
            {
                keyWords.Add("Crimson", "");
            }

            // Number of corruption blocks in the world
            if (WorldGen.tEvil > 0)
            {
                keyWords.Add("Corruption", "");
            }

            // Number of hallow blocks in the world
            if (WorldGen.tGood > 0)
            {
                keyWords.Add("Hallow", "");
            }

            if (Main.IsItRaining)
            {
                keyWords.Add("Rain", "");
            }

            if (Main.IsItStorming)
            {
                keyWords.Add("Thunder", "");
            }

            if (ChangedUtils.IsItWindy())
            {
                keyWords.Add("Windy", "");
            }

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
                keyWords.Add(seasonKeyWord, "");
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

            //var fullName = $"{sizeDescription} {npcName}";
            // For debugging: replace the name with the stats of the monster
            //if (debugShowStats)
            //   fullName = $"scale={scale}|def={npc.defense}|dmg={npc.damage}|kb={npc.knockBackResist}";
            npc.GivenName = $"{sizeDescription} {npcName}";
        }

        public string GetSizeDescription(NPC npc)
        {
            float scale = npc.scale / BaseScaleMultiplier;
            string sizeKey = null;

            if (scale >= scaleBehemoth)
                sizeKey = "Behemoth";
            else if (scale >= scaleGiant)
                sizeKey = "Giant";
            else if (scale >= scaleHuge)
                sizeKey = "Huge";
            else if (scale >= scaleBig)
                sizeKey = "Big";

            string sizeDescription = sizeKey != null
                ? Language.GetTextValue($"Mods.ChangedSpecialMod.SizeDescription.{sizeKey}")
                : string.Empty;

            return sizeDescription;
        }

        public void AdjustStatScaling(NPC npc)
        {
            var player = Main.LocalPlayer;

            // We only take our bosses and the ones required for progression. 
            // We also take the goblin and pirate events
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
                DownedBossSystem.DownedBehemoth     // Behemoth
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

        public void PickHat()
        {
            var player = Main.LocalPlayer;
            Drunk = ChangedUtils.IsDrunk(player);
            var WornHatType = HatType.None;

            if (Drunk)
                WornHatType = HatType.All;
            else if (BirthdayParty.PartyIsUp)
                WornHatType = HatType.Party;
            else if (Main.IsItRaining)
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

        public void OnSpawnExtra(NPC npc = null)
        {
            PickHat();
            if (npc != null && SeasonSystem.season == SeasonalEvent.Valentine && Main.rand.NextBool(3))
            {
                npc.AddBuff(BuffID.Lovestruck, 60 * 20);
            }
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
            var xOffset = ChangedNPC.BeerXOffset;//HatXOffset + vals[0];
            var yOffset = ChangedNPC.BeerYOffset;// HatYOffset + vals[1];
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
                {
                    direction *= -1;
                }

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
            }

            if (changedNPC.HasBeer)
            {
                DrawBeerGlass(npc, spriteBatch, screenPos, drawColor);
            }
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            base.OnHitPlayer(npc, target, hurtInfo);
            var ChangedGlobalNPC = npc.Changed();
            if (ChangedGlobalNPC == null || !ChangedGlobalNPC.DefaultOnHitPlayer)
                return;
            if (hurtInfo.Damage > 0)
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
    }
}
