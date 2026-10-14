using ChangedSpecialMod.Common.Configs;
using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items.Weapons;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
	public class Bloodstripe : ModNPC
	{
        public static string retroTexturePath = "ChangedSpecialMod/Content/NPCs/BloodstripeRetro";
        public override void SetStaticDefaults() 
        {
			Main.npcFrameCount[Type] = 4;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f,
                Scale = 1 / NPC.scale,
                PortraitScale = 1 / NPC.scale
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

		public override void SetDefaults() 
        {
			NPC.width = 25;
			NPC.height = 68;
            NPC.damage = 40;
            NPC.defense = 12;
            NPC.lifeMax = 300;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath5;//1
			NPC.value = ChangedUtils.GetNPCValue(gold: 1);
            NPC.knockBackResist = 0.1f;
			NPC.aiStyle = NPCAIStyleID.Fighter;
            NPC.npcSlots = 3;
            NPC.rarity = 2;
            AIType = NPCID.GoblinScout;
			AnimationType = NPCID.Zombie;
            //SpawnModBiomes = new int[] { ModContent.GetInstance<CityRuinsSurfaceBiome>().Type };
            
            Banner = Type;
            BannerItem = ModContent.ItemType<Items.Placeable.Banners.BloodstripeBanner>();
            ItemID.Sets.KillsToBanner[BannerItem] = 25;

            var changedNPC = NPC.Changed();
            //changedNPC.BaseScaleMultiplier = 0.7f;
            changedNPC.AdjustStatScaling(NPC);
            changedNPC.SetNPCName(NPC);
            changedNPC.HatYOffset = -28;
            changedNPC.GooType = GooType.None;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;

            changedNPC.ChangeHatPosition(ItemID.FlowerBoyHat, new int[] { 0, 14 });
            changedNPC.ChangeHatPosition(ItemID.BuccaneerBandana, new int[] { 6, 8 });
            changedNPC.DoOnSpawnExtra = true;
            changedNPC.GooColor = new Color(246, 73, 103);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TigerPaw>(), 5));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!ChangedUtils.CanSpawn(SpawnRequirement.WolfKing) || NPC.AnyNPCs(ModContent.NPCType<Bloodstripe>()))
                return 0;

            var spawnTileType = spawnInfo.SpawnTileType;
            if (TileID.Sets.CrimsonCountCollection.Contains(spawnTileType))
                return 0.15f;

            return 0;
        }

        private void UpdateHatPosition(int frameHeight)
        {
            int frameNumber = NPC.frame.Y / frameHeight;
            var changedNPC = NPC.Changed();
            var hatYOffset = -28;

            if (frameNumber == 2)
            {
                hatYOffset -= 2;
            }

            changedNPC.HatYOffset = hatYOffset;
        }

        public override void FindFrame(int frameHeight)
        {
            UpdateHatPosition(frameHeight);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var changedNPC = NPC.Changed();
            changedNPC.PostDrawExtra(NPC, spriteBatch, screenPos, drawColor);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.RemoveAt(2);
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new NPCPortraitInfoElement(3),
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.Bloodstripe.Description")),
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var changedNPC = NPC.Changed();
            var drawRetro = ChangedSpecialModClientConfig.Instance.RetroVisualsNPCs;

            if (drawRetro)
            {
                var texture = TextureAssets.Npc[Type].Value;
                if (ModContent.RequestIfExists(retroTexturePath, out Asset<Texture2D> retroTexture))
                    texture = retroTexture.Value;

                Vector2 drawPos = NPC.Center - screenPos;
                drawPos.Y += NPC.gfxOffY;
                drawColor *= (1 - NPC.shimmerTransparency);

                SpriteEffects effects = NPC.direction == 1
                    ? SpriteEffects.FlipHorizontally
                    : SpriteEffects.None;

                var nFrames = Main.npcFrameCount[Type];
                changedNPC.BaseScaleMultiplier = 0.7f;

                // Height normal: 72
                // Height retro: 99

                Rectangle frame = new Rectangle(0, NPC.frame.Top / 72 * 99, 61, 99);

                spriteBatch.Draw(
                    texture,
                    drawPos,
                    frame,
                    drawColor,
                    NPC.rotation,
                    new Vector2(texture.Size().X / 2, texture.Size().Y / nFrames / 2),
                    NPC.scale * changedNPC.BaseScaleMultiplier,
                    effects,
                    0f
                );

                return false;
            }
            else
            {
                changedNPC.BaseScaleMultiplier = 1f;
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
    }
}
