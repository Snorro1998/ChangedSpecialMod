using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items.Food;
using ChangedSpecialMod.Content.Items.Mounts;
using ChangedSpecialMod.Content.Items.Placeable.Banners;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
	public class LatexMoth : ModNPC
	{
        public override void SetStaticDefaults() 
        {
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Slimer];
        }

		public override void SetDefaults() 
        {
            NPC.width = 18;
			NPC.height = 45;
            NPC.damage = 25;
            NPC.defense = 10;
            NPC.lifeMax = 70;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = NPCAIStyleID.Bat;
			NPC.noGravity = true;
            //Banner = Type;
            //BannerItem = ModContent.ItemType<FlyingDarkLatexBanner>();
            AnimationType = NPCID.Slimer;
            SpawnModBiomes = new int[] { ModContent.GetInstance<WhiteLatexUndergroundBiome>().Type };

            var changedNPC = NPC.Changed();
            changedNPC.AdjustStatScaling(NPC);
            changedNPC.SetNPCName(NPC);
            changedNPC.GooType = GooType.White;
            changedNPC.spawnDepth = SpawnDepth.Cave;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.DoOnSpawnExtra = true;
            changedNPC.RemoveAllHats();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.LatexMoth.Description")),
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LatexMothMountItem>(), 20));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            var changedNPC = NPC.Changed();
            return ChangedUtils.GetSurfaceSpawnChance(spawnInfo, changedNPC, NPC.type);
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

        private void UpdateHatPosition()
        {
            var changedNPC = NPC.Changed();
            changedNPC.HatXOffset = (float)(Math.Sin(NPC.rotation) * -22 * NPC.spriteDirection); //0
            changedNPC.HatYOffset = (float)(Math.Cos(NPC.rotation) * -22);
        }

        public override void AI()
        {
            base.AI();
            UpdateHatPosition();
        }
    }
}
