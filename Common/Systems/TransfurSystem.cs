using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems
{
    public enum EvolutionLines
    {
        None,
        Black,
        White,
        SquidDog,
        Shark,
        Misc,
        TownNPC
    }

    public class TransfurSystem
    {
        public static Dictionary<EvolutionLines, List<Transfur>> EvolutionsLines = new Dictionary<EvolutionLines, List<Transfur>>();


        public static void InitTransfurTypes()
        {
            if (EvolutionsLines != null && EvolutionsLines.Count > 0)
                return;

            EvolutionsLines = new Dictionary<EvolutionLines, List<Transfur>>();

            // Black
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
                speedMultiplier = 0.75f,
                damageBonus = 0.3f
            });

            EvolutionsLines.Add(EvolutionLines.Black, evolutionLine);

            // White
            evolutionLine = new List<Transfur>();
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

            // Squid Dog
            evolutionLine = new List<Transfur>();
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
                tentacleAbility = true
            });

            EvolutionsLines.Add(EvolutionLines.SquidDog, evolutionLine);

            // Others
            evolutionLine = new List<Transfur>();
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
                speedMultiplier = 2f
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<Snep>(),
                npcName = "Snep",
                nFrames = 4,
                gooType = GooType.None,
                lifeMultiplier = 1.25f,
                extraDefense = 5,
                speedMultiplier = 2f
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<Bloodstripe>(),
                npcName = "Bloodstripe",
                nFrames = 4,
                gooType = GooType.None,
                lifeMultiplier = 2f,
                extraDefense = 10,
                speedMultiplier = 0.75f,
                damageBonus = 0.3f
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<Purrpurr>(),
                npcName = "Purrpurr",
                nFrames = 4,
                gooType = GooType.None,
                lifeMultiplier = 2f,
                extraDefense = 10,
                speedMultiplier = 0.75f,
                damageBonus = 0.3f
            });

            EvolutionsLines.Add(EvolutionLines.Misc, evolutionLine);
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
