using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;


namespace ChangedSpecialMod.Content.NPCs.TownPets
{
    [AutoloadHead]
    public class DarkLatexCubTownPet : ModNPC
    {
        private static ITownNPCProfile NPCProfile;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 27; // The number of frames our sprite has.
            NPCID.Sets.ExtraFramesCount[Type] = 20; // The number of frames after the walking frames.
            NPCID.Sets.AttackFrameCount[Type] = 0; // Town Pets don't have any attacking frames.
            NPCID.Sets.DangerDetectRange[Type] = 250; // How far away the NPC will detect danger. Measured in pixels.
            NPCID.Sets.AttackType[Type] = -1; // Town Pets do not attack. The default for this set is -1, so it is safe to remove this line if you wish.
            NPCID.Sets.AttackTime[Type] = -1; // Town Pets do not attack. The default for this set is -1, so it is safe to remove this line if you wish.
            NPCID.Sets.AttackAverageChance[Type] = 1;  // Town Pets do not attack. The default for this set is 1, so it is safe to remove this line if you wish.
            NPCID.Sets.HatOffsetY[Type] = -2; // An offset for where the party hat sits on the sprite.
            NPCID.Sets.ShimmerTownTransform[Type] = false; // Town Pets don't have a Shimmer variant.
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true; // But they are still immune to Shimmer.
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true; // And Confused.
            NPCID.Sets.ExtraTextureCount[Type] = 0; // Even though we have several variation textures, we don't use this set. The default for this set is 0, so it is safe to remove this line if you wish.
            NPCID.Sets.NPCFramingGroup[Type] = 8; // How the party hat is animated to match the walking animation. Town Cat = 4, Town Dog = 5, Town Bunny = 6, Town Slimes = 7, No offset = 8

            NPCID.Sets.IsTownPet[Type] = true; // Our NPC is a Town Pet
            NPCID.Sets.CannotSitOnFurniture[Type] = false; // True by default which means they cannot sit in chairs. True means they can sit on furniture like the Town Cat.
           // NPCID.Sets.TownNPCBestiaryPriority.Add(Type); // Puts our NPC with all of the other Town NPCs.
            NPCID.Sets.PlayerDistanceWhilePetting[Type] = 32; // Distance the player stands from the Town Pet to pet.
            NPCID.Sets.IsPetSmallForPetting[Type] = true; // If set to true, the player's arm will be angled down while petting.

            /*
            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                Velocity = 0.25f, // Draws the NPC in the bestiary as if its walking +0.25 tiles in the x direction
            };
            */

            //NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPCProfile = new DarkLatexCubTownPetProfile();
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
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.DarkLatexCubTownPet.Description")),
            });
        }
        */

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            return TownPetSystem.boughtBlackLatexCubPet;
        }

        public override ITownNPCProfile TownNPCProfile()
        {
            return NPCProfile;
        }

        public readonly List<string> NameList = new()
        {
            "Boaz",         // A friend's poodle
            "Blaidd",       // Elden Ring
            "Claude",       // Claude Abras
            "Coal",
            "Duncan",       // A friend's labrador
            "Fenris",       // Norse Mythology (Fenrir) 
            "Furbles",      // Furby
            "Gromit",       // Wallace and Gromit
            "Haruki",       // Tennis Ace
            "Hati",         // Norse Mythology
            "Kurama",       // Naruto
            "Lothar",       // Outland Wanderer
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

            int yOffset = 0;
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

    public class DarkLatexCubTownPetProfile : ITownNPCProfile
    {
        private static readonly string filePath = "ChangedSpecialMod/Content/NPCs/TownPets/DarkLatexCubTownPet";
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
