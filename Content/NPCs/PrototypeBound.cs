using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;


namespace ChangedSpecialMod.Content.NPCs
{
    public class PrototypeBound : ModNPC
    {
        public double imageSpeed = 10D;
        public int imageIndex = 0;
        public int[] animation = new int[] { 0, 1, 2, 1 };
        public int ImageLength { get { return animation.Length; } }
        public bool Loop = true;
        public double imageCounter = 0D;

        private Color lightColor = Color.Blue;

        // Disco
        private float rgbSpeed = 1.0f / (2 * 60.0f);
        private float rgbTime = 0;
        private Color rgbColorFrom = Color.Red;
        private Color rgbColorTo = Color.Blue;

        public override string Texture => "ChangedSpecialMod/Content/NPCs/PrototypeBound_Base";

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 3;
            NPCID.Sets.TownCritter[NPC.type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() { Hide = true };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.townNPC = true;
            NPC.dontTakeDamage = true;
            NPC.aiStyle = NPCAIStyleID.FaceClosestPlayer;
            NPC.damage = 0;
            NPC.defense = 25;
            NPC.lifeMax = 10000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.rarity = 1;
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                imageIndex = 0;
            }
            else
            {
                imageCounter += imageSpeed;
                if (imageCounter >= ImageLength * 60)
                {
                    if (Loop)
                    {
                        imageCounter %= ImageLength * 60;
                    }
                    else
                    {
                        imageCounter = ImageLength * 60 - 1;
                    }
                }

                var arrayIndex = (int)(imageCounter / 60D);
                imageIndex = animation[arrayIndex];
            }

            NPC.frame.Y = imageIndex * frameHeight;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D textureBase = Mod.Assets.Request<Texture2D>("Content/NPCs/PrototypeBound_Base").Value;
            Texture2D textureLight = Mod.Assets.Request<Texture2D>("Content/NPCs/PrototypeBound_Light").Value;
            Vector2 drawPos = NPC.Top - screenPos;
            drawPos.Y += NPC.gfxOffY;
            drawColor *= (1 - NPC.shimmerTransparency);

            SpriteEffects effects = NPC.direction == 1
                ? SpriteEffects.FlipHorizontally
                : SpriteEffects.None;

            var nFrames = Main.npcFrameCount[Type];

            // Body
            spriteBatch.Draw(
                textureBase,
                drawPos,
                NPC.frame,
                drawColor,
                NPC.rotation,
                new Vector2(textureBase.Size().X / 2, textureBase.Size().Y / nFrames / 2),
                NPC.scale,
                effects,
                0f
            );

            // Lights
            spriteBatch.Draw(
                textureLight,
                drawPos,
                NPC.frame,
                lightColor,
                NPC.rotation,
                new Vector2(textureBase.Size().X / 2, textureBase.Size().Y / nFrames / 2),
                NPC.scale,
                effects,
                0f
            );

            return false;
        }

        private void UpdateLightColor()
        {
            // Light blue
            lightColor = new Color(0, 160, 255);

            if (BirthdayParty.PartyIsUp)
            {
                rgbTime += rgbSpeed;
                if (rgbTime >= 1)
                {
                    rgbTime = 0;
                    rgbColorFrom = rgbColorTo;
                    rgbColorTo = ChangedUtils.Choose(Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Purple, Color.White);
                }

                lightColor = Color.Lerp(rgbColorFrom, rgbColorTo, rgbTime);
            }

            else if (Main.bloodMoon)
            {
                lightColor = Color.Crimson;
            }
            else if (Main.eclipse)
            {
                lightColor = Color.Orange;
            }

            float speed = 2f;
            float amplitude = 0.15f;
            float mean = 0.65f;
            float alpha = (float)Math.Sin(Main.GlobalTimeWrappedHourly * speed) * amplitude + mean;
            lightColor *= alpha;
            lightColor *= (1 - NPC.shimmerTransparency);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            UpdateLightColor();
            Lighting.AddLight(NPC.Center, lightColor.ToVector3());
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;

        public override string GetChat()
        {
            NPCPortraitSystem.UpdatePortrait(ModContent.NPCType<Prototype>(), "Talk");
            return ":D";
        }

        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.homeless = false;
                NPC.homeTileX = -1;
                NPC.homeTileY = -1;
                NPC.netUpdate = true;
            }

            if (NPC.wet)
                NPC.life = 250;

            foreach (var player in Main.player)
            {
                if (!player.active)
                    continue;

                if (player.talkNPC == NPC.whoAmI)
                {
                    Rescue();
                    player.SetTalkNPC(NPC.whoAmI); //Refresh dialogue options
                    return;
                }
            }
        }

        public void Rescue()
        {
            NPC.Transform(ModContent.NPCType<Prototype>());
            NPC.dontTakeDamage = false;
        }
    }
}