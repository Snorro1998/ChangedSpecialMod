using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs.TownPets
{
    [AutoloadHead]
    public class WhiteLatexCubTownPet : ModNPC
    {
        private static ITownNPCProfile NPCProfile;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 27;
            NPCID.Sets.ExtraFramesCount[Type] = 20;
            NPCID.Sets.AttackFrameCount[Type] = 0;
            NPCID.Sets.DangerDetectRange[Type] = 250;
            NPCID.Sets.AttackType[Type] = -1; 
            NPCID.Sets.AttackTime[Type] = -1;
            NPCID.Sets.AttackAverageChance[Type] = 1;
            NPCID.Sets.HatOffsetY[Type] = -2;
            NPCID.Sets.ShimmerTownTransform[Type] = false;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.ExtraTextureCount[Type] = 0;
            NPCID.Sets.NPCFramingGroup[Type] = 8;
            NPCID.Sets.IsTownPet[Type] = true;
            NPCID.Sets.CannotSitOnFurniture[Type] = false;
            //NPCID.Sets.TownNPCBestiaryPriority.Add(Type);
            NPCID.Sets.PlayerDistanceWhilePetting[Type] = 32;
            NPCID.Sets.IsPetSmallForPetting[Type] = true;

            /*
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                Velocity = 0.25f,
            };
            */

            //NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            NPCProfile = new WhiteLatexCubTownPetProfile();
            ChangedUtils.HideFromBestiary(this);
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 20;
            NPC.height = 20;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0.5f;
            NPC.housingCategory = 1;
            AnimationType = NPCID.TownBunny;
        }

        /*
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.WhiteLatexCubTownPet.Description")),
            });
        }
        */

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            return TownPetSystem.boughtWhiteLatexCubPet;
        }

        public override ITownNPCProfile TownNPCProfile()
        {
            return NPCProfile;
        }

        public readonly List<string> NameList = new()
        {
            "Arabella",     // My rabbit
            "Bolt",         // Bolt
            "Dori",         // Crayon Shin Chan
            "Fel",          // Campfire cooking in another world
            "Flops",        // My rabbit
            "Lea",
            "Marshmallow",
            "Pimpi",
            "Sebas",        // Outland Wanderer
            "Skoll",        // Norse Mythology
            "Snowball"
        };

        public override List<string> SetNPCNameList()
        {
            return NameList;
        }

        public override string GetChat()
        {
            return ":D";
        }

        public override bool PreAI()
        {
            if (NPC.ai[0] == 5f)
            {
                DrawOffsetY = -10;
            }
            else
            {
                DrawOffsetY = 0;
            }

            return base.PreAI();
        }

        public override void ChatBubblePosition(ref Vector2 position, ref SpriteEffects spriteEffects)
        {
            if (NPC.ai[0] == 5f)
            {
                position.Y -= 18f;
            }
        }

        public override void PartyHatPosition(ref Vector2 position, ref SpriteEffects spriteEffects)
        {
            int frame = NPC.frame.Y / NPC.frame.Height;
            int xOffset = 0;
            position.X += xOffset * NPC.spriteDirection;

            int yOffset = -2;
            switch (frame)
            {
                case 2:
                case 3:
                case 5:
                case 6:
                    yOffset -= 2;
                    break;
                case 11:
                case 15:
                    yOffset += 4;
                    break;
                case 12:
                case 13:
                case 14:
                    yOffset += 8;
                    break;
                default:
                    break;
            }
            position.Y += yOffset;

            if (NPC.ai[0] == 5f)
            {
                position.Y += -10;
            }
        }
    }

    public class WhiteLatexCubTownPetProfile : ITownNPCProfile
    {
        private static readonly string filePath = "ChangedSpecialMod/Content/NPCs/TownPets/WhiteLatexCubTownPet";
        private readonly Asset<Texture2D> variant0 = ModContent.Request<Texture2D>(filePath);
        private readonly int headIndex0 = ModContent.GetModHeadSlot($"{filePath}_Head");

        public int RollVariation()
        {
            return 0;
        }

        public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

        public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
        {
            return variant0;
        }

        public int GetHeadTextureIndex(NPC npc)
        {
            return headIndex0;
        }
    }
}
