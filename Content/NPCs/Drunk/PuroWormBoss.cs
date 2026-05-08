using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
    // These three class showcase usage of the WormHead, WormBody and WormTail classes from Worm.cs
    internal class PuroWormHead : WormHead
    {
        public override string Texture => "ChangedSpecialMod/Content/NPCs/Drunk/PuroWormHead";
        public override int BodyType => ModContent.NPCType<PuroWormBody>();
        public override int TailType => ModContent.NPCType<PuroWormTail>();

        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f,
                Scale = 1 / NPC.scale * 1.25f,
                PortraitScale = 1 / NPC.scale * 1.25f,
                Position = new Vector2(0, 32)
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            // Head is 10 defense, body 20, tail 30.
            NPC.CloneDefaults(NPCID.DiggerHead);
            NPC.npcSlots = 5;
            NPC.width = 79;
            NPC.height = 79;
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
            changedNPC.ElementType = ElementType.None;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.HitEffectScale = 3;
            changedNPC.RemoveAllHats();
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
            if (!ChangedUtils.IsDrunk(spawnInfo.Player))
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
        internal static void CommonWormInit(Worm worm)
        {
            // These two properties handle the movement of the worm
            worm.MoveSpeed = 5.5f;
            worm.Acceleration = 0.045f;
        }

        /*
        private int attackCounter;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(attackCounter);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attackCounter = reader.ReadInt32();
        }

        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (attackCounter > 0)
                {
                    attackCounter--; // tick down the attack counter.
                }
            }
        }
        */

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.Next(3) == 0)
            {
                Color color = new Color(Main.rand.Next(255), Main.rand.Next(255), Main.rand.Next(255));
                SoundEngine.PlaySound(Assets.Sounds.SoundNom, NPC.Center);
                CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), color, "Mmm orange", true);
            }
            base.OnHitPlayer(target, hurtInfo);
        }

        // This draws the npc sprite in segments of 32 so the lightning doesn't get messed up when it enters the ground.
        // This is a problem in vanilla, but you don't notice it because all worms are small and thin.
        public static bool CommonPreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;

            Texture2D mainTexture = TextureAssets.Npc[npc.type].Value;
            Rectangle drawingFrame = npc.frame;

            int segmentSize = (int)(16 / npc.scale);

            // World draw center
            Vector2 drawCenter = npc.Center - screenPos + new Vector2(0f, npc.gfxOffY);

            // Frame center (used as rotation pivot)
            Vector2 frameCenter = new Vector2(drawingFrame.Width / 2f, drawingFrame.Height / 2f);

            for (int y = 0; y < drawingFrame.Height; y += segmentSize)
            {
                for (int x = 0; x < drawingFrame.Width; x += segmentSize)
                {
                    int segmentWidth = Math.Min(segmentSize, drawingFrame.Width - x);
                    int segmentHeight = Math.Min(segmentSize, drawingFrame.Height - y);

                    Rectangle segmentFrame = new Rectangle(
                        drawingFrame.X + x,
                        drawingFrame.Y + y,
                        segmentWidth,
                        segmentHeight
                    );

                    // Segment center in local frame space
                    Vector2 segmentCenter = new Vector2(
                        x + segmentWidth / 2f,
                        y + segmentHeight / 2f
                    );

                    // Offset from frame center -> rotated
                    Vector2 localOffset = (segmentCenter - frameCenter) * npc.scale;
                    Vector2 rotatedOffset = localOffset.RotatedBy(npc.rotation);

                    Vector2 drawPositionInWorld = drawCenter + rotatedOffset + screenPos;
                    Color tmpColor = Lighting.GetColor((int)drawPositionInWorld.X / 16, (int)(drawPositionInWorld.Y / 16f));

                    spriteBatch.Draw(
                        mainTexture,
                        drawCenter + rotatedOffset,
                        segmentFrame,
                        npc.GetAlpha(tmpColor),
                        npc.rotation,
                        new Vector2(segmentWidth / 2f, segmentHeight / 2f),
                        npc.scale,
                        spriteEffects,
                        0f
                    );
                }
            }

            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return CommonPreDraw(NPC, spriteBatch, screenPos, drawColor);
        }
    }

    internal class PuroWormBody : WormBody
    {
        public override string Texture => "ChangedSpecialMod/Content/NPCs/Drunk/PuroWormBody";
        public override void SetStaticDefaults()
        {
            ChangedUtils.HideFromBestiary(this);
            NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<PuroWormHead>();
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DiggerBody);
            NPC.width = 79;
            NPC.height = 79;
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
            changedNPC.ElementType = ElementType.None;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.HitEffectScale = 3;

            // Extra body parts should use the same Banner value as the main ModNPC.
            //Banner = ModContent.NPCType<PuroWormHead>();
        }

        public override void Init()
        {
            PuroWormHead.CommonWormInit(this);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return PuroWormHead.CommonPreDraw(NPC, spriteBatch, screenPos, drawColor);
        }
    }

    internal class PuroWormTail : WormTail
    {
        public override string Texture => "ChangedSpecialMod/Content/NPCs/Drunk/PuroWormTail";
        public override void SetStaticDefaults()
        {
            ChangedUtils.HideFromBestiary(this);
            NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<PuroWormHead>();
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DiggerTail);
            NPC.width = 79;
            NPC.height = 79;
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
            changedNPC.ElementType = ElementType.None;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.HitEffectScale = 3;

            // Extra body parts should use the same Banner value as the main ModNPC.
            //Banner = ModContent.NPCType<ExampleWormHead>();
        }

        public override void Init()
        {
            PuroWormHead.CommonWormInit(this);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return PuroWormHead.CommonPreDraw(NPC, spriteBatch, screenPos, drawColor);
        }
    }
}
