using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Utilities;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;


namespace ChangedSpecialMod.Common.Systems
{
    public enum EvolutionLines
    {
        None,
        Black,
        White,
        Misc,
        Aquatic
    }

    public class TransfurSystem
    {
        public static Dictionary<EvolutionLines, List<Transfur>> EvolutionsLines = new Dictionary<EvolutionLines, List<Transfur>>();


        public static Transfur GetTransfurByNPCType(int npcType)
        {
            InitTransfurTypes();
            var keys = EvolutionsLines.Keys.ToList();
            foreach (var key in keys)
            {
                var transfurs = EvolutionsLines[key];
                foreach (var transfur in transfurs)
                {
                    if (transfur.npcType == npcType)
                        return transfur;
                }
            }
            return null;
        }

        private static string GetNumberString(float number, bool multiplier)
        {
            var colorString = "ffffff";

            if (number < 1)
                colorString = "ff0000";
            else
                colorString = "00ff00";
            if (multiplier)
                return $"[c/{colorString}:{number.ToString()} X]";
            return $"[c/{colorString}:+{number.ToString()}]";
        }

        public static string GetDescription(int npcType, bool isItemDescription = false)
        {
            var transfur = GetTransfurByNPCType(npcType);
            if (transfur != null)
            {
                var strAbiBase = "Mods.ChangedSpecialMod.Abilities.";
                var npcName = Language.GetTextValue($"Mods.ChangedSpecialMod.NPCs.{transfur.npcName}.DisplayName");

                var description = isItemDescription ? string.Format($"{Language.GetTextValue(strAbiBase + "TransformsYouInto")}\n", npcName) : $"[c/ffeb6e:{npcName}]\n";
                if (transfur.lifeMultiplier != 1)
                    description += $"[i:{ItemID.LifeCrystal}]: {GetNumberString(transfur.lifeMultiplier, true)}\n";
                if (transfur.extraDefense != 0)
                    description += $"[i:{ItemID.CobaltShield}]: {GetNumberString(transfur.extraDefense, false)}\n";
                if (transfur.speedMultiplier != 1)
                    description += $"[i:{ItemID.HermesBoots}]: {GetNumberString(transfur.speedMultiplier, true)}\n";
                if (transfur.waterSpeedMultiplier != 1)
                    //SailfishBoots
                    description += $"[i:{ItemID.Flipper}]: {GetNumberString(transfur.waterSpeedMultiplier, true)}\n";
                if (transfur.extraMinions != 0)
                    description += $"[i:{ItemID.SlimeStaff}]: {GetNumberString(transfur.extraMinions, false)}\n";

                // Ability
                var strAbilty = Language.GetTextValue("Mods.ChangedSpecialMod.Keybinds.TransfurAttack.DisplayName");
                if (transfur.npcType == ModContent.NPCType<SquidDog>())
                    description += $"{strAbilty}: {Language.GetTextValue(strAbiBase + "TentacleWhip")}\n";
                if (transfur.npcType == ModContent.NPCType<Purrpurr>())
                    description += $"{strAbilty}: {Language.GetTextValue(strAbiBase + "MingCat")}\n";

                // Biome bonus
                if (transfur.bonusDesert || transfur.bonusSnow || transfur.bonusOcean)
                {
                    description += $"{Language.GetTextValue(strAbiBase + "IncreasedLifeRegen")} ";
                    if (!isItemDescription)
                        description += "\n";
                    var biomeName = "Forest";
                    if (transfur.bonusDesert)
                        biomeName = ShopHelper.BiomeNameByKey("Desert");
                    else if (transfur.bonusSnow)
                        biomeName = ShopHelper.BiomeNameByKey("Snow");
                    else if (transfur.bonusOcean)
                        biomeName = ShopHelper.BiomeNameByKey("Ocean");

                    description += string.Format($"{Language.GetTextValue(strAbiBase + "WhileInBiome")}\n", biomeName);
                }
                if (transfur.waterBreathing)
                    description += $"{Language.GetTextValue(strAbiBase + "WaterBreathing")}\n";
                if (transfur.waterBreathingCalamity && ModSupportSystem.modCalamity != null)
                    description += $"{Language.GetTextValue(strAbiBase + "WaterBreathingCalamity")}\n";

                if (isItemDescription)
                    description = description.TrimEnd('\r', '\n');

                return description;
            }
            return "";
        }

        private static void AddEvoLineBlack()
        {
            var evolutionLine = new List<Transfur>();
            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<BlackGoop>(),
                npcName = "BlackGoop",
                nFrames = 4,
                gooType = GooType.Black,
                lifeMultiplier = 0.6f,
                speedMultiplier = 0.75f,
                speedMultiplierAirborn = 2f,
                jumpSpeedMultiplier = 1.75f,
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<DarkLatexCub>(),
                npcName = "DarkLatexCub",
                nFrames = 4,
                gooType = GooType.Black,
                lifeMultiplier = 0.8f,
                speedMultiplier = 1.2f,
                jumpHeightMultiplier = 1.5f
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<MaleDarkLatex>(),
                npcName = "MaleDarkLatex",
                gooType = GooType.Black,
                lifeMultiplier = 1.25f,
                extraDefense = 5,
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<WingedDarkLatex>(),
                npcName = "WingedDarkLatex",
                nFrames = 4,
                gooType = GooType.Black,
                lifeMultiplier = 1.25f,
                extraDefense = 5,
                speedMultiplier = 2f
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<Wendigo>(),
                npcName = "Wendigo",
                nFrames = 4,
                gooType = GooType.Black,
                lifeMultiplier = 2f,
                extraDefense = 10,
                speedMultiplier = 0.8f,
                damageBonus = 0.3f,
                baseScaleMultiplier = 0.7f
            });

            EvolutionsLines.Add(EvolutionLines.Black, evolutionLine);
        }

        private static void AddEvoLineWhite()
        {
            var evolutionLine = new List<Transfur>();
            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<WhiteGoop>(),
                npcName = "WhiteGoop",
                nFrames = 4,
                gooType = GooType.White,
                lifeMultiplier = 0.6f,
                speedMultiplier = 0.75f,
                speedMultiplierAirborn = 2f,
                jumpSpeedMultiplier = 1.75f,
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<WhiteLatexCub>(),
                npcName = "WhiteLatexCub",
                nFrames = 3,
                gooType = GooType.White,
                lifeMultiplier = 0.8f,
                speedMultiplier = 1.2f,
                jumpHeightMultiplier = 1.5f
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<WhiteKnight>(),
                npcName = "WhiteKnight",
                gooType = GooType.White,
                lifeMultiplier = 1.25f,
                extraDefense = 5,
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<WhiteLatexTaur>(),
                npcName = "WhiteLatexTaur",
                nFrames = 13,
                gooType = GooType.White,
                lifeMultiplier = 1.25f,
                extraDefense = 5,
                speedMultiplier = 2f
            });

            EvolutionsLines.Add(EvolutionLines.White, evolutionLine);
        }

        private static void AddEvoLineAquatic()
        {
            var evolutionLine = new List<Transfur>();
            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<SquidDog>(),
                npcName = "SquidDog",
                nFrames = 4,
                gooType = GooType.None,
                lifeMultiplier = 1.25f,
                extraDefense = 5,
                ignoreWater = true,
                waterBreathing = true,
                waterBreathingCalamity = true,
                tentacleAbility = true
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<TigerShark>(),
                npcName = "TigerShark",
                nFrames = 4,
                gooType = GooType.None,
                lifeMultiplier = 1.25f,
                extraDefense = 5,
                ignoreWater = true,
                waterBreathing = true,
                bonusOcean = true,
                rotateInWater = true,
                waterSpeedMultiplier = 2,
                baseScaleMultiplier = 0.7f
            });

            EvolutionsLines.Add(EvolutionLines.Aquatic, evolutionLine);
        }

        private static void AddEvoLineMisc()
        {
            var evolutionLine = new List<Transfur>();
            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<GermanShepherd>(),
                npcName = "GermanShepherd",
                nFrames = 4,
                gooType = GooType.None,
                lifeMultiplier = 1.25f,
                extraDefense = 5
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<Lion>(),
                npcName = "Lion",
                nFrames = 4,
                gooType = GooType.None,
                lifeMultiplier = 1.25f,
                extraDefense = 5,
                speedMultiplier = 2f,
                bonusDesert = true
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<Snep>(),
                npcName = "Snep",
                nFrames = 4,
                gooType = GooType.None,
                lifeMultiplier = 1.25f,
                extraDefense = 5,
                speedMultiplier = 2f,
                bonusSnow = true
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<Bloodstripe>(),
                npcName = "Bloodstripe",
                nFrames = 4,
                gooType = GooType.None,
                lifeMultiplier = 2f,
                extraDefense = 10,
                speedMultiplier = 0.8f,
                damageBonus = 0.3f,
                baseScaleMultiplier = 0.7f
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<Purrpurr>(),
                npcName = "Purrpurr",
                nFrames = 4,
                gooType = GooType.None,
                lifeMultiplier = 2f,
                extraDefense = 10,
                extraMinions = 1,
                speedMultiplier = 0.8f,
                damageBonus = 0.3f,
                baseScaleMultiplier = 0.9f
            });

            EvolutionsLines.Add(EvolutionLines.Misc, evolutionLine);
        }

        public static void InitTransfurTypes()
        {
            if (EvolutionsLines != null && EvolutionsLines.Count > 0)
                return;

            EvolutionsLines = new Dictionary<EvolutionLines, List<Transfur>>();

            AddEvoLineBlack();
            AddEvoLineWhite();
            AddEvoLineMisc();
            AddEvoLineAquatic();





            /*
            // Town NPCs
            evolutionLine = new List<Transfur>();

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<Puro>(),
                texturePath = $"{baseTexturePath}Puro",
                gooType = GooType.Black,
                lifeMultiplier = 1.25f,
                extraDefense = 5,
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<Scientist>(),
                texturePath = $"{baseTexturePath}Scientist",
                gooType = GooType.White,
                lifeMultiplier = 1.25f,
                extraDefense = 5,
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<Prototype>(),
                texturePath = $"{baseTexturePath}Prototype",
                gooType = GooType.None,
                lifeMultiplier = 1.25f,
                extraDefense = 5,
            });

            EvolutionsLines.Add(EvolutionLines.TownNPC, evolutionLine);
            */
        }

        public static void UntransfurPlayer(int playerIndex)
        {
            Main.player[playerIndex]
                .GetModPlayer<ChangedSpecialModPlayer>()
                .NpcType = -1;
        }

        public static void SetTransfurFromNPCType(int playerIndex, int npcType)
        {
            Main.player[playerIndex]
                .GetModPlayer<ChangedSpecialModPlayer>()
                .NpcType = npcType;
        }

        public static void SyncTransfur(int playerIndex, int toClient = -1)
        {
            var changedPlayer = Main.player[playerIndex].ChangedPlayer();

            ModPacket packet = ModContent.GetInstance<ChangedSpecialMod>().GetPacket();

            packet.Write((byte)ChangedSpecialMod.MessageType.SyncTransfurPlayer);
            packet.Write((byte)playerIndex);
            packet.Write(changedPlayer.NpcType);

            packet.Send(toClient);
        }


        public static void TransfurEffect(NPC npc)
        {
            if (!Main.dedServ)
            {
                var changedNPC = npc.Changed();
                if (changedNPC == null)
                    return;

                var dustType = changedNPC.GooType == GooType.Black ? DustID.Asphalt : DustID.SnowSpray;
                var nParticles = 40;
                for (int i = 0; i < nParticles; i++)
                {
                    var dust = Dust.NewDustDirect(npc.Center, npc.width, npc.height, dustType, 0, 0, 1);
                    dust.velocity.X += Main.rand.NextFloat(-0.05f, 0.05f);
                    dust.velocity.Y += Main.rand.NextFloat(-0.05f, 0.05f);
                }
            }

            AudioSystem.PlayTransfurSound(npc.Center);
        }
    }
}
