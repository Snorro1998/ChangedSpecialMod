using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Content.Items.Mounts;
using ChangedSpecialMod.Content.Items.Placeable.Banners;
using ChangedSpecialMod.Content.Items.Weapons;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
	public class FlyingDarkLatex : ModNPC
	{
        public override void SetStaticDefaults() 
        {
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Slimer];
            NPCID.Sets.ShimmerTransformToNPC[NPC.type] = ModContent.NPCType<WhiteKnight>();
        }

		public override void SetDefaults() 
        {
            NPC.width = 18;
			NPC.height = 45;
            NPC.damage = 25;
            NPC.defense = 10;
            NPC.lifeMax = 70; //320
            NPC.HitSound = SoundID.NPCHit1; //SoundID.NPCHit6;
            NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = NPCAIStyleID.Bat;
			NPC.noGravity = true;
            Banner = Type;
            BannerItem = ModContent.ItemType<FlyingDarkLatexBanner>();
            AnimationType = NPCID.Slimer;
            SpawnModBiomes = new int[] { ModContent.GetInstance<BlackLatexSurfaceBiome>().Type };

            var changedNPC = NPC.Changed();
            changedNPC.AdjustStatScaling(NPC);
            changedNPC.SetNPCName(NPC);
            changedNPC.GooType = GooType.Black;
            changedNPC.ElementType = ElementType.Wind;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.DoOnSpawnExtra = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Remove the default portrait, otherwise you get two of them
            bestiaryEntry.Info.RemoveAt(2);
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                //  Add the new portrait with the modified rarity
                new NPCPortraitInfoElement(3),
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.FlyingDarkLatex.Description")),
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FlyingDarkLatexMountItem>(), 20));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            var ChangedGlobalNPC = NPC.Changed();
            return ChangedUtils.GetSurfaceSpawnChance(spawnInfo, ChangedGlobalNPC, NPC.type);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var changedNPC = NPC.Changed();

            var frame = NPC.frame;

            // Adjust hat postion when sitting
            // 129 = frame height
            // 18 = sitting frame
            var fr = frame.Top / 65;
            if (fr == 0)
            {
                changedNPC.HatYOffset = -22;//-22
            }
            else if (fr == 1 || fr == 3)
            {
                changedNPC.HatYOffset = -26;//-24
            }
            else if (fr == 2)
            {
                changedNPC.HatYOffset = -30;//-28
            }

            NPC.Changed().PostDrawExtra(NPC, spriteBatch, screenPos, drawColor);
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }

        public override void AI()
        {
            base.AI();
            var ChangedGlobalNPC = NPC.Changed();
            ChangedGlobalNPC.HatXOffset = (float)(Math.Sin(NPC.rotation) * -22 * NPC.spriteDirection); //0
            ChangedGlobalNPC.HatYOffset = (float)(Math.Cos(NPC.rotation) * -22);
        }
    }
}
