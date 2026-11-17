using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles.Fireworks
{
    public abstract class BaseFireworksProjectile : ModProjectile
    {
        private Color[] pixels = null;
        private int width = 1;
        private int height = 1;

        public virtual string PatternName => "Prototype";
        public virtual int ColorRand => 30;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 45;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.velocity.X = 0;
            Projectile.velocity.Y = -8;

            try
            {
                //Content/NPCs/SweeperItem

                ChangedSpecialMod.Instance.RequestAssetIfExists<Texture2D>($"Assets/Textures/FireworksPatterns/{PatternName}", out ReLogic.Content.Asset<Texture2D> texture);
                if (texture != null)
                {
                    var tex = texture.Value;
                    pixels = new Color[tex.Width * tex.Height];

                    Main.QueueMainThreadAction(() =>
                    {
                        tex.GetData(pixels);
                        width = tex.Width;
                        height = tex.Height;
                    });
                }
            }
            catch
            {

            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            if (pixels != null)
            {
                var spacing = 8; //16
                var pos = Projectile.Center;
                var fireworksType = DustID.FireworksRGB;

                for (int y = 0; y < height; y++)
                {
                    var yPos = y - height / 2;
                    for (int x = 0; x < width; x++)
                    {
                        var xPos = x - width / 2;
                        Color color = pixels[y * width + x];
                        if (color.A == 255)
                        {
                            var pos2 = pos + new Vector2(xPos * spacing, yPos * spacing);
                            var randOffset = new Vector2(Main.rand.NextFloat(-spacing / 2, spacing / 2), Main.rand.NextFloat(-spacing / 4, spacing / 4));
                            pos2 += randOffset;

                            var distVector = pos2 - pos;
                            distVector /= spacing;

                            color.R = (byte)Math.Clamp(color.R + Main.rand.Next(-ColorRand, ColorRand), 0, 255);
                            color.G = (byte)Math.Clamp(color.G + Main.rand.Next(-ColorRand, ColorRand), 0, 255);
                            color.B = (byte)Math.Clamp(color.B + Main.rand.Next(-ColorRand, ColorRand), 0, 255);

                            var dust = Dust.NewDustPerfect(pos, fireworksType, distVector, 0, color, 1);
                            //dust.alpha += Main.rand.Next(128);
                        }
                    }
                }
            }   
        }

        public override bool PreDraw(ref Color lightColor)
        {
            ChangedUtils.DrawProjectileCentered(Projectile, lightColor);
            return false;
        }
    }

    public class RocketWhiteTailProjectile : BaseFireworksProjectile
    {
        public override string PatternName => "WhiteTail";
        public override int ColorRand => 20;
    }

    public class RocketPrototypeProjectile : BaseFireworksProjectile
    {
        public override string PatternName => "Prototype";
    }

    public class RocketEyeOfCthuluProjectile : BaseFireworksProjectile
    {
        public override string PatternName => "EOC";
    }

    public class RocketEaterOfWorldsProjectile : BaseFireworksProjectile
    {
        public override string PatternName => "EOW";
    }

    public class RocketGuideProjectile : BaseFireworksProjectile
    {
        public override string PatternName => "Guide";
    }
}
