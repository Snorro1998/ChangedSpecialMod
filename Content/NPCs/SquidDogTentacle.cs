using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.NPCs.Drunk;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
    // These three class showcase usage of the WormHead, WormBody and WormTail classes from Worm.cs
    internal class SquidDogTentacleHead : WormHead2
    {
        public override string Texture => "ChangedSpecialMod/Content/NPCs/SquidDogTentacleHead";
        public override int BodyType => ModContent.NPCType<SquidDogTentacleBody>();
        public override int TailType => ModContent.NPCType<SquidDogTentacleTail>();

        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f,
                Scale = 1 / NPC.scale,
                PortraitScale = 1 / NPC.scale,
                Position = new Vector2(0, 32)
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            // Head is 10 defense, body 20, tail 30.
            NPC.CloneDefaults(NPCID.DiggerHead);
            NPC.npcSlots = 5;
            NPC.width = 58;
            NPC.height = 58;
            NPC.aiStyle = -1;
            NPC.npcSlots = 3;
            NPC.lifeMax = 200;
            NPC.damage = 30;
            NPC.defense = 10;
            NPC.scale = 1;
            SpawnModBiomes = new int[] { ModContent.GetInstance<ZDrunkBiome>().Type };

            var changedNPC = NPC.Changed();
            changedNPC.BaseScaleMultiplier = 1;
            changedNPC.AdjustStatScaling(NPC);
            changedNPC.SetNPCName(NPC);
            changedNPC.GooType = GooType.Black;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.HitEffectScale = 3;
            changedNPC.RemoveAllHats();
            changedNPC.spawnDepth = SpawnDepth.Everywhere;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.PuroWormHead.Description")),
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.townNPCs > 0 || !ChangedUtils.IsDrunk(spawnInfo.Player))
                return 0;
            var changedNPC = NPC.Changed();
            return ChangedUtils.GetSurfaceSpawnChance(spawnInfo, changedNPC, NPC.type);
        }

        public override void Init()
        {
            // Set the segment variance
            // If you want the segment length to be constant, set these two properties to the same value
            MinSegmentLength = 6;
            MaxSegmentLength = 12;

            CommonWormInit(this);
        }

        // This method is invoked from ExampleWormHead, ExampleWormBody and ExampleWormTail
        internal static void CommonWormInit(Worm2 worm)
        {
            // These two properties handle the movement of the worm
            worm.MoveSpeed = 5.5f;
            worm.Acceleration = 0.045f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.Next(3) == 0)
            {
                Color color = new Color(Main.rand.Next(255), Main.rand.Next(255), Main.rand.Next(255));
                AudioSystem.PlayNomSound(NPC.Center);
                //SoundEngine.PlaySound(Assets.Sounds.SoundNom, NPC.Center);
                var msg = Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.Dialogue.PuroWorm.Dialogue1");
                CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), color, msg, true);
            }
            base.OnHitPlayer(target, hurtInfo);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.ai[2] > 300)
            {
                NPC.rotation = NPC.rotation;
            }

            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
    }

    internal class SquidDogTentacleBody : WormBody2
    {
        public override int BodyType => ModContent.NPCType<SquidDogTentacleBody>();

        public override int TailType => ModContent.NPCType<SquidDogTentacleTail>();

        public override string Texture => "ChangedSpecialMod/Content/NPCs/SquidDogTentacleBody";
        public override void SetStaticDefaults()
        {
            ChangedUtils.HideFromBestiary(this);
            NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<PuroWormHead>();
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DiggerBody);
            NPC.width = 58;
            NPC.height = 36;
            NPC.aiStyle = -1;
            NPC.npcSlots = 0;
            NPC.damage = 25;
            NPC.defense = 12;
            NPC.scale = 1;
            SpawnModBiomes = new int[] { ModContent.GetInstance<ZDrunkBiome>().Type };

            var changedNPC = NPC.Changed();
            changedNPC.BaseScaleMultiplier = 1;
            changedNPC.AdjustStatScaling(NPC);
            changedNPC.SetNPCName(NPC);
            changedNPC.GooType = GooType.Black;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.HitEffectScale = 3;

            // Extra body parts should use the same Banner value as the main ModNPC.
            //Banner = ModContent.NPCType<PuroWormHead>();
        }

        public override void Init()
        {
            SquidDogTentacleHead.CommonWormInit(this);
        }

        /*
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return PuroWormHead.CommonPreDraw(NPC, spriteBatch, screenPos, drawColor);
        }
        */
    }

    internal class SquidDogTentacleTail : WormTail2
    {
        public override string Texture => "ChangedSpecialMod/Content/NPCs/SquidDogTentacleTail";

        public override int BodyType => ModContent.NPCType<SquidDogTentacleBody>();

        public override int TailType => ModContent.NPCType<SquidDogTentacleTail>();

        public override void SetStaticDefaults()
        {
            ChangedUtils.HideFromBestiary(this);
            NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<PuroWormHead>();
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DiggerTail);
            NPC.width = 58;
            NPC.height = 36;
            NPC.aiStyle = -1;
            NPC.npcSlots = 0;
            NPC.damage = 20;
            NPC.defense = 8;
            NPC.scale = 1;
            SpawnModBiomes = new int[] { ModContent.GetInstance<ZDrunkBiome>().Type };

            var changedNPC = NPC.Changed();
            changedNPC.BaseScaleMultiplier = 1;
            changedNPC.AdjustStatScaling(NPC);
            changedNPC.SetNPCName(NPC);
            changedNPC.GooType = GooType.Black;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.HitEffectScale = 3;

            // Extra body parts should use the same Banner value as the main ModNPC.
            //Banner = ModContent.NPCType<ExampleWormHead>();
        }

        public override void Init()
        {
            SquidDogTentacleHead.CommonWormInit(this);
        }

        /*
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return PuroWormHead.CommonPreDraw(NPC, spriteBatch, screenPos, drawColor);
        }
        */
    }
}
