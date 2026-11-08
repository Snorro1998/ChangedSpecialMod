using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items.Placeable.Banners;
using ChangedSpecialMod.Content.NPCs.AIStyles;
using ChangedSpecialMod.Content.Walls;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
	public class MaleDarkLatex : ModNPC
	{
        public override void SetStaticDefaults() 
        {
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Zombie];
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
			NPC.width = 18;
			NPC.height = 35;
            NPC.damage = 30;
            NPC.defense = 12;
            NPC.lifeMax = 80;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
            NPC.aiStyle = -1;
            AIType = NPCID.None;
            AnimationType = NPCID.Zombie;
            /*
            NPC.aiStyle = NPCAIStyleID.Fighter;
			AIType = NPCID.GoblinScout;
			AnimationType = NPCID.Zombie;
            */
            SpawnModBiomes = new int[] { ModContent.GetInstance<BlackLatexSurfaceBiome>().Type };

            Banner = Type;
            BannerItem = ModContent.ItemType<DarkLatexBanner>();

            var changedNPC = NPC.Changed();
            changedNPC.AdjustStatScaling(NPC);
            changedNPC.SetNPCName(NPC);
            changedNPC.SetHalloweenHatsForBlackLatex();
            changedNPC.GooType = GooType.Black;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.CanHaveBeer = true;
            changedNPC.BeerXOffset = -16;
            changedNPC.DoOnSpawnExtra = true;
            changedNPC.spawnDepth = SpawnDepth.Everywhere;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.MaleDarkLatex.Description")),
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            var changedNPC = NPC.Changed();
            return ChangedUtils.GetSurfaceSpawnChance(spawnInfo, changedNPC, NPC.type);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC.Changed().PostDrawExtra(NPC, spriteBatch, screenPos, drawColor);
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }

        public override void OnSpawn(IEntitySource source)
        {
            var position = NPC.position.ToTileCoordinates();
            var tile = Framing.GetTileSafely(position);
            // Make him asleep if he spawns in the lab
            if (tile.WallType == ModContent.WallType<LabWall>())
            {
                NPC.ai[0] = 10;
            }
        }

        public override void AI()
        {
            AIFighter.Update(NPC);
        }

        public override bool? CanFallThroughPlatforms()
        {
            if (!NPC.HasValidTarget) return false;
            else return Main.player[NPC.target].Bottom.Y > NPC.Bottom.Y + 32;
        }
    }
}
