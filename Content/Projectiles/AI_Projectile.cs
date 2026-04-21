using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Content.Projectiles;
using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Content.Tiles.Furniture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.GameContent.Golf;
using Terraria.GameContent.UI;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.Utilities;


namespace ChangedSpecialMod.Content.Projectiles
{
    public static class AI_Projectile
    {
        /*
        public static void DrawProj_DrawNormalProjs(Projectile proj, float polePosX, float polePosY, Vector2 mountedCenter, ref Color projectileColor)
        {
            int num136 = 0;
            int num137 = 0;

            float num138 = (float)(TextureAssets.Projectile[proj.type].Width() - proj.width) * 0.5f + (float)proj.width * 0.5f;
           
            ProjectileLoader.DrawOffset(proj, ref num137, ref num136, ref num138);
            SpriteEffects dir = SpriteEffects.None;
            if (proj.spriteDirection == -1)
            {
                dir = SpriteEffects.FlipHorizontally;
            }


            
            {
                {
                    Vector2 vector82 = proj.position + new Vector2(proj.width, proj.height) / 2f + Vector2.UnitY * proj.gfxOffY - Main.screenPosition;
                    Texture2D texture2D4 = TextureAssets.Projectile[proj.type].Value;
                    Rectangle rectangle22 = texture2D4.Frame(1, Main.projFrames[proj.type], 0, proj.frame);

                    if (proj.type == 962)
                    {
                        int verticalFrames = 4;
                        int frameY3 = proj.frame / Main.projFrames[proj.type];
                        int frameX2 = proj.frame % Main.projFrames[proj.type];
                        rectangle22 = texture2D4.Frame(Main.projFrames[proj.type], verticalFrames, frameX2, frameY3);
                    }
                    Color color94 = proj.GetAlpha(projectileColor);
                    
                    Vector2 origin25 = rectangle22.Size() / 2f;

                    if (proj.type == 963 && proj.localAI[1] >= 0f)
                    {
                        float num338 = proj.localAI[1];
                        float num339 = 1f - num338;
                        Color color95 = proj.GetAlpha(new Color(255, 220, 220)) * num339 * num339 * 0.8f;
                        color95.A = 0;
                        short num340 = 536;
                        Projectile.LoadProjectile(num340);
                        Texture2D value88 = TextureAssets.Projectile[num340].Value;
                        Vector2 origin26 = value88.Size() * new Vector2(0.5f, 1f);
                        float num341 = 9f;
                        float num342 = proj.velocity.ToRotation();
                        if (proj.velocity.Length() < 0.1f)
                        {
                            num342 = ((proj.direction == 1) ? 0f : ((float)Math.PI));
                        }
                        Vector2 value89 = (num342 + (float)Math.PI / 2f).ToRotationVector2();
                        for (int num343 = 0; (float)num343 < num341; num343++)
                        {
                            float num344 = ((num343 % 2 != 0) ? 1 : (-1));
                            float num345 = ((float)num343 + 1f) * num344 * 0.2f * (0.2f + 2f * num338) + num342 + (float)Math.PI / 2f;
                            float num346 = Utils.Remap(Vector2.Dot(num345.ToRotationVector2(), value89), -1f, 1f, 0f, 1f);
                            float num347 = proj.scale * (0.15f + 0.6f * (float)Math.Sin(Main.GlobalTimeWrappedHourly + (float)num343 * 0.739f)) * num346;
                            Main.EntitySpriteDraw(value88, vector82 + proj.rotation.ToRotationVector2().RotatedBy((float)Math.PI * 2f * (1f / num341) * (float)num343 + Main.GlobalTimeWrappedHourly) * 4f * proj.scale, null, color95 * num346, num345, origin26, new Vector2(num347 * 1.5f, num347), SpriteEffects.None);
                        }
                    }
                    if (proj.type == 962)
                    {
                        float num348 = Utils.Remap(proj.ai[0], 0f, 30f, 1f, 0f);
                        Color color96 = proj.GetAlpha(Color.White) * num348 * num348 * 0.3f;
                        color96.A = 0;
                        for (int num349 = 0; num349 < 4; num349++)
                        {
                            Main.EntitySpriteDraw(texture2D4, vector82 + proj.rotation.ToRotationVector2().RotatedBy((float)Math.PI / 2f * (float)num349) * 2f * proj.scale, rectangle22, color96, proj.rotation, origin25, proj.scale, dir);
                        }
                    }
                    if (proj.type == 855)
                    {
                        float y21 = (Main.GlobalTimeWrappedHourly % 6f / 6f * ((float)Math.PI * 2f)).ToRotationVector2().Y;
                        float num350 = y21 * 0.3f + 0.7f;
                        Color color97 = color94 * num350 * 0.3f;
                        for (int num351 = 0; num351 < 4; num351++)
                        {
                            float x13 = 0f;
                            float y22 = 0f;
                            switch (num351)
                            {
                                case 0:
                                    x13 = 4f;
                                    break;
                                case 1:
                                    x13 = -4f;
                                    break;
                                case 2:
                                    y22 = 4f;
                                    break;
                                case 3:
                                    y22 = -4f;
                                    break;
                            }
                            Vector2 vector83 = new Vector2(x13, y22).RotatedBy(proj.rotation) * y21;
                            Main.spriteBatch.Draw(texture2D4, vector82 + vector83, rectangle22, color97, proj.rotation, rectangle22.Size() / 2f, 1f, SpriteEffects.None, 0f);
                        }
                    }
                    else if (proj.type == 908)
                    {
                        PlayerTitaniumStormBuffTextureContent playerTitaniumStormBuff = TextureAssets.RenderTargets.PlayerTitaniumStormBuff;
                        vector82 += (Main.GlobalTimeWrappedHourly * 8f + (float)proj.whoAmI).ToRotationVector2() * 4f;
                        playerTitaniumStormBuff.Request();
                        if (playerTitaniumStormBuff.IsReady)
                        {
                            texture2D4 = playerTitaniumStormBuff.GetTarget();
                        }
                        rectangle22 = texture2D4.Frame(Main.projFrames[proj.type], 1, proj.frame);
                        origin25 = rectangle22.Size() / 2f;
                    }
                    else if (proj.type == 764)
                    {
                        Projectile.DrawProjWithStarryTrail(proj, projectileColor, dir);
                    }
                    else if (proj.type == 856)
                    {
                        Projectile.DrawProjWithStarryTrail(proj, projectileColor, dir);
                    }
                    else if (proj.type == 857)
                    {
                        Projectile.DrawProjWithStarryTrail(proj, projectileColor, dir);
                        color94 = Color.White * proj.Opacity * 0.9f;
                        color94.A /= 2;
                        rectangle22 = texture2D4.Frame(15, 1, proj.frame);
                        origin25 = rectangle22.Size() / 2f;
                        Main.DrawPrettyStarSparkle(proj.Opacity, dir, vector82, color94, proj.GetFirstFractalColor(), proj.localAI[0], 15f, 30f, 30f, 45f, 0f, new Vector2(5f, 2f), Vector2.One);
                    }
                    else if (proj.type == 539)
                    {
                        if (proj.ai[0] >= 210f)
                        {
                            float num352 = proj.ai[0] - 210f;
                            num352 /= 20f;
                            if (num352 > 1f)
                            {
                                num352 = 1f;
                            }
                            Main.EntitySpriteDraw(TextureAssets.Extra[46].Value, vector82, null, new Color(255, 255, 255, 128) * num352, proj.rotation, new Vector2(17f, 22f), proj.scale, dir);
                        }
                    }
                    else if (proj.type == 773)
                    {
                        origin25.Y = rectangle22.Height - 12;
                    }
                    else if (proj.type == 866)
                    {
                        origin25.X += 14f;
                    }
                    else if (proj.type == 759)
                    {
                        origin25.Y = rectangle22.Height - 2;
                        origin25.X += (dir.HasFlag(SpriteEffects.FlipHorizontally) ? 1 : (-1));
                    }
                    else if (proj.type == 758)
                    {
                        vector82.Y += proj.height / 2;
                        origin25 = rectangle22.Size() * new Vector2(0.5f, 1f);
                        origin25.Y -= 4f;
                    }
                    else if (proj.type == 951)
                    {
                        vector82.Y += proj.height / 2;
                        vector82.Y -= (float)rectangle22.Height * 0.5f;
                        vector82.Y += 4f;
                        origin25 = rectangle22.Size() * new Vector2(0.5f, 0.5f);
                    }
                    else if (proj.type == 833)
                    {
                        if (proj.frame != 8)
                        {
                            vector82.Y += proj.height / 2;
                            origin25 = rectangle22.Size() * new Vector2(0.5f, 1f);
                            origin25.Y -= 4f;
                            origin25.X -= 7 * dir.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt();
                        }
                    }
                    else if (proj.type == 834 || proj.type == 835)
                    {
                        if (proj.frame != 10)
                        {
                            vector82.Y += proj.height / 2;
                            origin25 = rectangle22.Size() * new Vector2(0.5f, 1f);
                            origin25.Y -= 4f;
                            origin25.X -= 2 * dir.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt();
                        }
                    }
                    else if (proj.type == 715 || proj.type == 716 || proj.type == 717 || proj.type == 718)
                    {
                        rectangle22 = texture2D4.Frame(3);
                        origin25 = rectangle22.Size() / 2f;
                        int num353 = (int)proj.ai[0];
                        Vector2 origin27 = new Vector2(rectangle22.Width / 2, 0f);
                        Vector2 vector84 = proj.Size / 2f;
                        Color celeb2Color = proj.GetCeleb2Color();
                        celeb2Color.A = 127;
                        celeb2Color *= 0.8f;
                        Rectangle value90 = rectangle22;
                        value90.X += value90.Width * 2;
                        for (int num354 = proj.oldPos.Length - 1; num354 > 0; num354--)
                        {
                            Vector2 vector85 = proj.oldPos[num354] + vector84;
                            if (!(vector85 == vector84))
                            {
                                Vector2 value91 = proj.oldPos[num354 - 1] + vector84;
                                float num355 = proj.oldRot[num354];
                                Vector2 scale14 = new Vector2(Vector2.Distance(vector85, value91) / (float)rectangle22.Width, 1f);
                                Color color98 = celeb2Color * (1f - (float)num354 / (float)proj.oldPos.Length);
                                switch (num353)
                                {
                                    case 2:
                                        {
                                            Vector2 vector86 = num355.ToRotationVector2();
                                            int num356 = num354 + proj.timeLeft;
                                            if (num356 < 0)
                                            {
                                                num356 += 20 * (num356 / -20) + 20;
                                            }
                                            num356 %= 20;
                                            float num357 = 0f;
                                            scale14 *= 0.6f;
                                            switch (num356)
                                            {
                                                case 1:
                                                    num357 = 1f;
                                                    break;
                                                case 2:
                                                    num357 = 2f;
                                                    break;
                                                case 3:
                                                    num357 = 3f;
                                                    break;
                                                case 4:
                                                    num357 = 2f;
                                                    break;
                                                case 5:
                                                    num357 = 1f;
                                                    break;
                                                case 7:
                                                    num357 = -1f;
                                                    break;
                                                case 8:
                                                    num357 = -2f;
                                                    break;
                                                case 9:
                                                    num357 = -3f;
                                                    break;
                                                case 10:
                                                    num357 = -2f;
                                                    break;
                                                case 11:
                                                    num357 = -1f;
                                                    break;
                                            }
                                            vector85 += vector86 * num357 * 4f;
                                            break;
                                        }
                                    case 5:
                                        scale14 *= 0.5f;
                                        break;
                                }
                                Main.EntitySpriteDraw(texture2D4, vector85 - Main.screenPosition, value90, color98, num355, origin27, scale14, dir);
                            }
                        }
                    }
                    else if (proj.type == 663 || proj.type == 665 || proj.type == 667)
                    {
                        vector82 = proj.Bottom + Vector2.UnitY * proj.gfxOffY - Main.screenPosition;
                        origin25 = rectangle22.Size() * new Vector2(0.5f, 1f);
                        origin25.Y -= 2f;
                        origin25.Y -= 2f;
                    }
                    else if (proj.type == 691 || proj.type == 692 || proj.type == 693)
                    {
                        vector82 = proj.Bottom + Vector2.UnitY * proj.gfxOffY - Main.screenPosition;
                        origin25 = rectangle22.Size() * new Vector2(0.5f, 1f);
                        origin25.Y -= 2f;
                        origin25.Y -= 2f;
                    }
                    else if (proj.type == 677 || proj.type == 678 || proj.type == 679)
                    {
                        if (proj.spriteDirection == -1)
                        {
                            dir ^= SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                        }
                        Texture2D value92 = TextureAssets.Extra[83].Value;
                        if (proj.type == 678)
                        {
                            value92 = TextureAssets.Extra[84].Value;
                        }
                        if (proj.type == 679)
                        {
                            value92 = TextureAssets.Extra[85].Value;
                        }
                        Vector2 position22 = proj.Bottom + Vector2.UnitY * proj.gfxOffY - Main.screenPosition;
                        Vector2 origin28 = value92.Size() * new Vector2(0.5f, 1f);
                        origin28.Y -= 2f;
                        Main.EntitySpriteDraw(value92, position22, null, color94, 0f, origin28, 1f, dir & SpriteEffects.FlipHorizontally);
                        origin25.X += dir.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt();
                        vector82.Y += 1f;
                        vector82.Y += 2f;
                        if (proj.type == 678)
                        {
                            vector82.Y += -4f;
                        }
                        if (proj.type == 679)
                        {
                            vector82.Y -= 2f;
                            if (!dir.HasFlag(SpriteEffects.FlipVertically))
                            {
                                origin25.Y += 4f;
                            }
                            else
                            {
                                origin25.Y -= 4f;
                            }
                            origin25.X += dir.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt() * 4;
                        }
                    }
                    else if (proj.type == 602)
                    {
                        origin25.X = rectangle22.Width - 6;
                        origin25.Y -= 1f;
                        rectangle22.Height -= 2;
                    }
                    else if (proj.type == 589)
                    {
                        rectangle22 = texture2D4.Frame(5, 1, (int)proj.ai[1]);
                        origin25 = rectangle22.Size() / 2f;
                    }
                    else if (proj.type == 590)
                    {
                        if (proj.ai[2] == 1f && proj.frame < 3)
                        {
                            proj.frame = 3;
                        }
                        rectangle22 = texture2D4.Frame(6, 1, proj.frame);
                        origin25 = rectangle22.Size() / 2f;
                    }
                    else if (proj.type == 836)
                    {
                        rectangle22 = texture2D4.Frame(4, 1, proj.frame);
                        origin25 = rectangle22.Size() / 2f;
                    }
                    else if (proj.type == 650 || proj.type == 882 || proj.type == 888 || proj.type == 894 || proj.type == 895 || proj.type == 896 || proj.type == 898 || proj.type == 901 || proj.type == 957)
                    {
                        origin25.Y -= 4f;
                    }
                    else if (proj.type == 623)
                    {
                        if (!Main.gamePaused && proj.ai[0] == 2f)
                        {
                            vector82 += Main.rand.NextVector2Circular(2f, 2f);
                        }
                        if (Main.CurrentDrawnEntityShader == 0)
                        {
                            color94.A /= 2;
                        }
                    }
                    else if (proj.type >= 625 && proj.type <= 628)
                    {
                        color94.A /= 2;
                    }
                    else if (proj.type == 644)
                    {
                        Color color99 = Main.hslToRgb(proj.ai[0], 1f, 0.5f).MultiplyRGBA(new Color(255, 255, 255, 0));
                        Main.EntitySpriteDraw(texture2D4, vector82, rectangle22, color99, proj.rotation, origin25, proj.scale * 2f, dir);
                        Main.EntitySpriteDraw(texture2D4, vector82, rectangle22, color99, 0f, origin25, proj.scale * 2f, dir);
                        if (proj.ai[1] != -1f && proj.Opacity > 0.3f)
                        {
                            Vector2 vector87 = Main.projectile[(int)proj.ai[1]].Center - proj.Center;
                            Vector2 vector88 = new Vector2(1f, vector87.Length() / (float)texture2D4.Height);
                            float rotation29 = vector87.ToRotation() + (float)Math.PI / 2f;
                            float value93 = MathHelper.Distance(30f, proj.localAI[1]) / 20f;
                            value93 = MathHelper.Clamp(value93, 0f, 1f);
                            if (value93 > 0f)
                            {
                                Main.EntitySpriteDraw(texture2D4, vector82 + vector87 / 2f, rectangle22, color99 * value93, rotation29, origin25, vector88, dir);
                                Main.EntitySpriteDraw(texture2D4, vector82 + vector87 / 2f, rectangle22, color94 * value93, rotation29, origin25, vector88 / 2f, dir);
                            }
                        }
                    }
                    else if (proj.type == 658)
                    {
                        Color color100 = Main.hslToRgb(0.136f, 1f, 0.5f).MultiplyRGBA(new Color(255, 255, 255, 0));
                        Main.EntitySpriteDraw(texture2D4, vector82, rectangle22, color100, 0f, origin25, new Vector2(1f, 5f) * proj.scale * 2f, dir);
                    }
                    Main.EntitySpriteDraw(texture2D4, vector82, rectangle22, color94, proj.rotation, origin25, proj.scale, dir);
                    if (proj.type == 896)
                    {
                        Texture2D value94 = TextureAssets.GlowMask[278].Value;
                        Color color101 = new Color(150, 150, 150, 100);
                        for (int num358 = 0; num358 < 2; num358++)
                        {
                            Vector2 position23 = vector82 + new Vector2((float)Main.rand.Next(-10, 11) * 0.1f, (float)Main.rand.Next(-10, 11) * 0.1f);
                            Main.EntitySpriteDraw(value94, position23, rectangle22, color101, proj.rotation, origin25, proj.scale, dir);
                        }
                        Main.EntitySpriteDraw(value94, vector82, rectangle22, Color.White, proj.rotation, origin25, proj.scale, dir);
                    }
                    if (proj.type == 889)
                    {
                        Texture2D value95 = TextureAssets.GlowMask[276].Value;
                        Color color102 = Color.White * (int)Main.mouseTextColor;
                        Main.EntitySpriteDraw(value95, vector82, rectangle22, color102, proj.rotation, origin25, proj.scale, dir);
                        if (!proj.isAPreviewDummy)
                        {
                            for (int num359 = 0; num359 < 4; num359++)
                            {
                                int num360 = 28;
                                int num361 = 7 + num359;
                                float num362 = 100f;
                                bool flag37 = num361 == 8;
                                Rectangle value96 = texture2D4.Frame(1, Main.projFrames[proj.type], 0, num361);
                                Vector2 value97 = vector82;
                                Vector2 vector89 = vector82;
                                SpriteEffects effects5 = SpriteEffects.None;
                                float num363 = 0f;
                                float num364 = Main.GlobalTimeWrappedHourly * 2f;
                                switch (num359)
                                {
                                    case 1:
                                        num364 += (float)Math.PI / 2f;
                                        break;
                                    case 2:
                                        num364 += (float)Math.PI;
                                        break;
                                    case 3:
                                        num364 += 4.712389f;
                                        break;
                                }
                                num364 *= 3f;
                                num363 = num364;
                                vector89 += num364.ToRotationVector2() * num360;
                                if (proj.localAI[0] == num362)
                                {
                                    Main.EntitySpriteDraw(texture2D4, vector89, value96, color94, num363, origin25, proj.scale, effects5);
                                    if (flag37)
                                    {
                                        Main.EntitySpriteDraw(value95, vector89, value96, color102, num363, origin25, proj.scale, effects5);
                                    }
                                    continue;
                                }
                                Vector2 vector90 = new Vector2(num360, -16f) + proj.velocity * 1.5f;
                                float num365 = 4f;
                                float num366 = -0.35f;
                                switch (num359)
                                {
                                    case 1:
                                        vector90.X *= -1f;
                                        effects5 = SpriteEffects.FlipHorizontally;
                                        num366 = 0.35f;
                                        num365 = -3f;
                                        break;
                                    case 2:
                                        vector90.Y = 16f;
                                        num366 = 0.35f;
                                        num365 = 2f;
                                        break;
                                    case 3:
                                        vector90.X *= -1f;
                                        vector90.Y = 16f;
                                        effects5 = SpriteEffects.FlipHorizontally;
                                        num366 = -0.35f;
                                        num365 = -1f;
                                        break;
                                }
                                vector90 += (Main.GlobalTimeWrappedHourly * num365).ToRotationVector2() * 4f;
                                value97 += vector90;
                                float num367 = proj.localAI[0] / num362;
                                value97 = Vector2.Lerp(value97, vector89, num367);
                                num363 = ((num367 > 0.5f) ? num364 : num366);
                                Main.EntitySpriteDraw(texture2D4, value97, value96, color94, num363, origin25, proj.scale, effects5);
                                if (flag37)
                                {
                                    Main.EntitySpriteDraw(value95, value97, value96, color102, num363, origin25, proj.scale, effects5);
                                }
                            }
                        }
                    }
                    if (proj.type == 885 && !proj.isAPreviewDummy)
                    {
                        for (int num368 = 0; num368 < 2; num368++)
                        {
                            SpriteEffects effects6 = SpriteEffects.None;
                            int num369 = -30;
                            if (num368 == 1)
                            {
                                num369 = 30;
                                effects6 = SpriteEffects.FlipHorizontally;
                            }
                            int num370 = (int)proj.localAI[0];
                            if (proj.frame == 6)
                            {
                                num370 = 0;
                            }
                            else if (num368 == 1)
                            {
                                num370 = 2 - num370;
                            }
                            num370 += 7;
                            Rectangle value98 = texture2D4.Frame(1, Main.projFrames[proj.type], 0, num370);
                            Vector2 position24 = vector82 + new Vector2(num369, 0f);
                            Vector2 vector91 = (Main.GlobalTimeWrappedHourly * 2f).ToRotationVector2() * 4f;
                            vector91 += proj.velocity * -1.5f;
                            Vector2 vector92 = (Main.GlobalTimeWrappedHourly * 1f).ToRotationVector2() * 3f;
                            if (num368 == 1)
                            {
                                position24 += vector91 + vector92;
                            }
                            else
                            {
                                position24 -= vector91;
                            }
                            Main.EntitySpriteDraw(texture2D4, position24, value98, color94, 0f, origin25, proj.scale, effects6);
                        }
                    }
                    if (proj.type == 535)
                    {
                        for (int num371 = 0; num371 < 1000; num371++)
                        {
                            if (Main.projectile[num371].active && Main.projectile[num371].owner == proj.owner && Main.projectile[num371].type == 536)
                            {
                                Projectile.DrawProj(num371);
                            }
                        }
                    }
                    else if (proj.type == 715 || proj.type == 716 || proj.type == 717 || proj.type == 718)
                    {
                        rectangle22.X += rectangle22.Width;
                        Color celeb2Color2 = proj.GetCeleb2Color();
                        celeb2Color2.A = 80;
                        Main.EntitySpriteDraw(texture2D4, vector82, rectangle22, celeb2Color2, proj.rotation, origin25, proj.scale, dir);
                    }
                    else if (proj.type == 702)
                    {
                        Texture2D value99 = TextureAssets.Flames[5].Value;
                        Vector2 origin29 = value99.Size() / 2f;
                        Vector2 vector93 = new Vector2(5 * proj.spriteDirection, -10f).RotatedBy(proj.rotation);
                        ulong seed2 = (ulong)(proj.localAI[0] / 4f);
                        for (int num372 = 0; num372 < 5; num372++)
                        {
                            Color color103 = new Color(100, 100, 100, 0);
                            float x14 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
                            float y23 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.35f;
                            Main.EntitySpriteDraw(value99, vector82 + vector93 + new Vector2(x14, y23), null, color103, proj.rotation, origin29, 1f, dir);
                        }
                    }
                    else if (proj.type == 663 || proj.type == 665 || proj.type == 667)
                    {
                        Texture2D value100 = TextureAssets.GlowMask[221].Value;
                        switch (proj.type)
                        {
                            case 665:
                                value100 = TextureAssets.GlowMask[222].Value;
                                break;
                            case 667:
                                value100 = TextureAssets.GlowMask[223].Value;
                                break;
                        }
                        float num373 = (proj.localAI[0] / 100f * ((float)Math.PI * 2f)).ToRotationVector2().X * 1f + 1f;
                        Color color104 = new Color(140, 100, 40, 0) * (num373 / 4f + 0.5f) * 1f;
                        for (float num374 = 0f; num374 < 4f; num374 += 1f)
                        {
                            Main.EntitySpriteDraw(value100, vector82 + (num374 * ((float)Math.PI / 2f)).ToRotationVector2() * num373, rectangle22, color104, proj.rotation, origin25, proj.scale, dir);
                        }
                    }
                    else if (proj.type == 644)
                    {
                        Main.EntitySpriteDraw(texture2D4, vector82, rectangle22, color94, 0f, origin25, proj.scale, dir);
                    }
                    else if (proj.type == 963)
                    {
                        int num375 = Main.player[proj.owner].ownedProjectileCounts[970] - 1;
                        int num376 = (num375 + 3) % 3;
                        int num377 = num375 / 3;
                        Vector3 vector94 = Main.rgbToHsl(new Color(250, 150, 180));
                        vector94 = new Vector3(0f, 1f, 0.6f);
                        if (num377 == 1)
                        {
                            vector94 = Main.rgbToHsl(Color.HotPink);
                            vector94.Z += 0.1f;
                            vector94.X -= 0.05f;
                        }
                        vector94.X = (vector94.X - (float)num377 * 0.13f + 1f) % 1f;
                        Color oldColor = Main.hslToRgb(vector94);
                        Color color105 = Lighting.GetColor((int)(proj.Center.X / 16f), (int)(proj.Center.Y / 16f), oldColor);
                        rectangle22.X += rectangle22.Width * (1 + num376);
                        Main.EntitySpriteDraw(texture2D4, vector82, rectangle22, color105, proj.rotation, origin25, proj.scale, dir);
                    }
                    else if (proj.type == 773 && proj.velocity.Length() == 0f)
                    {
                        Main.EntitySpriteDraw(TextureAssets.GlowMask[266].Value, color: Color.White * (int)Main.mouseTextColor, position: vector82, sourceRectangle: rectangle22, rotation: proj.rotation, origin: origin25, scale: proj.scale, effects: dir);
                    }
                    else if (proj.type == 658)
                    {
                        Main.EntitySpriteDraw(texture2D4, vector82, rectangle22, color94, 0f, origin25, new Vector2(1f, 8f) * proj.scale, dir);
                    }
                    else if (proj.type == 602)
                    {
                        texture2D4 = TextureAssets.Extra[60].Value;
                        Color color107 = color94;
                        color107.A = 0;
                        color107 *= 0.3f;
                        origin25 = texture2D4.Size() / 2f;
                        Main.EntitySpriteDraw(texture2D4, vector82, null, color107, proj.rotation - (float)Math.PI / 2f, origin25, proj.scale, dir);
                        texture2D4 = TextureAssets.Extra[59].Value;
                        color107 = color94;
                        color107.A = 0;
                        color107 *= 0.13f;
                        origin25 = texture2D4.Size() / 2f;
                        Main.EntitySpriteDraw(texture2D4, vector82, null, color107, proj.rotation - (float)Math.PI / 2f, origin25, proj.scale * 0.9f, dir);
                    }
                    else if (proj.type == 539)
                    {
                        Main.EntitySpriteDraw(TextureAssets.GlowMask[140].Value, vector82, rectangle22, new Color(255, 255, 255, 0), proj.rotation, origin25, proj.scale, dir);
                    }
                    else if (proj.type == 613)
                    {
                        Main.EntitySpriteDraw(TextureAssets.GlowMask[189].Value, vector82, rectangle22, new Color(128 - proj.alpha / 2, 128 - proj.alpha / 2, 128 - proj.alpha / 2, 0), proj.rotation, origin25, proj.scale, dir);
                    }
                    else if (proj.type == 614)
                    {
                        Main.EntitySpriteDraw(TextureAssets.GlowMask[190].Value, vector82, rectangle22, new Color(128 - proj.alpha / 2, 128 - proj.alpha / 2, 128 - proj.alpha / 2, 0), proj.rotation, origin25, proj.scale, dir);
                    }
                    else if (proj.type == 574)
                    {
                        Main.EntitySpriteDraw(TextureAssets.GlowMask[148].Value, vector82, rectangle22, new Color(255, 255, 255, 0), proj.rotation, origin25, proj.scale, dir);
                    }
                    else if (proj.type == 691 || proj.type == 692 || proj.type == 693)
                    {
                        Texture2D value102 = TextureAssets.GlowMask[235].Value;
                        switch (proj.type)
                        {
                            case 692:
                                value102 = TextureAssets.GlowMask[236].Value;
                                break;
                            case 693:
                                value102 = TextureAssets.GlowMask[237].Value;
                                break;
                        }
                        Main.EntitySpriteDraw(value102, vector82, rectangle22, new Color(255, 255, 255, 127), proj.rotation, origin25, proj.scale, dir);
                    }
                    else if (proj.type == 590)
                    {
                        Main.EntitySpriteDraw(TextureAssets.GlowMask[168].Value, vector82, rectangle22, new Color(127 - proj.alpha / 2, 127 - proj.alpha / 2, 127 - proj.alpha / 2, 0), proj.rotation, origin25, proj.scale, dir);
                    }
                    else if (proj.type == 623 || (proj.type >= 625 && proj.type <= 628))
                    {
                        if (Main.player[proj.owner].ghostFade != 0f)
                        {
                            float num378 = Main.player[proj.owner].ghostFade * 5f;
                            for (float num379 = 0f; num379 < 4f; num379 += 1f)
                            {
                                Main.EntitySpriteDraw(texture2D4, vector82 + Vector2.UnitY.RotatedBy(num379 * ((float)Math.PI * 2f) / 4f) * num378, rectangle22, color94 * 0.1f, proj.rotation, origin25, proj.scale, dir);
                            }
                        }
                        if (proj.type == 623 && proj.ai[0] == 2f && proj.frame >= 14)
                        {
                            Projectile.DrawProj_StardustGuardianPunching(proj);
                        }
                    }
                    else if (proj.type == 643)
                    {
                        float num380 = (float)Math.Cos((float)Math.PI * 2f * (proj.localAI[0] / 60f)) + 3f + 3f;
                        for (float num381 = 0f; num381 < 4f; num381 += 1f)
                        {
                            Main.EntitySpriteDraw(texture2D4, vector82 + Vector2.UnitY.RotatedBy(num381 * ((float)Math.PI / 2f)) * num380, rectangle22, color94 * 0.2f, proj.rotation, origin25, proj.scale, dir);
                        }
                    }
                    else if (proj.type == 650)
                    {
                        int num382 = (int)(proj.localAI[0] / ((float)Math.PI * 2f));
                        float f = proj.localAI[0] % ((float)Math.PI * 2f) - (float)Math.PI;
                        float num383 = (float)Math.IEEERemainder(proj.localAI[1], 1.0);
                        if (num383 < 0f)
                        {
                            num383 += 1f;
                        }
                        int num384 = (int)Math.Floor(proj.localAI[1]);
                        float num385 = 1f;
                        float num386 = 5f;
                        num385 = 1f + (float)num384 * 0.02f;
                        if ((float)num382 == 1f)
                        {
                            num386 = 7f;
                        }
                        Vector2 vector95 = f.ToRotationVector2() * num383 * num386 * proj.scale;
                        texture2D4 = TextureAssets.Extra[66].Value;
                        Main.EntitySpriteDraw(texture2D4, vector82 + vector95, null, color94, proj.rotation, texture2D4.Size() / 2f, num385, SpriteEffects.None);
                    }
                    return;
                }

                if (Main.projFrames[proj.type] > 1)
                {
                    int num408 = TextureAssets.Projectile[proj.type].Height() / Main.projFrames[proj.type];
                    int y27 = num408 * proj.frame;
                    if (proj.type == 111)
                    {
                        int r = Main.player[proj.owner].shirtColor.R;
                        int g = Main.player[proj.owner].shirtColor.G;
                        int b = Main.player[proj.owner].shirtColor.B;
                        projectileColor = Lighting.GetColor(oldColor: new Color((byte)r, (byte)g, (byte)b), x: (int)((double)proj.position.X + (double)proj.width * 0.5) / 16, y: (int)(((double)proj.position.Y + (double)proj.height * 0.5) / 16.0));
                        Main.EntitySpriteDraw(TextureAssets.Projectile[proj.type].Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY), new Rectangle(0, y27, TextureAssets.Projectile[proj.type].Width(), num408), proj.GetAlpha(projectileColor), proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale, dir);
                        return;
                    }
                    Color alpha12 = proj.GetAlpha(projectileColor);
                    if (proj.type == 211 && Main.CurrentDrawnEntityShader != 0)
                    {
                        alpha12.A = 127;
                    }
                    if (proj.type == 344)
                    {
                        float num409 = MathHelper.Min(60f, proj.ai[0]) / 2f;
                        for (float num410 = 0.9f; num410 > 0f; num410 -= 0.25f)
                        {
                            Vector2 vector106 = num410 * (proj.velocity * 0.33f) * num409;
                            Main.EntitySpriteDraw(TextureAssets.Projectile[proj.type].Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY) - vector106, new Rectangle(0, y27, TextureAssets.Projectile[proj.type].Width(), num408 - 1), alpha12 * (1f - num410) * 0.75f, proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale * (0.4f + (1f - num410) * 0.6f), dir);
                        }
                    }
                    if (proj.type == 920 || proj.type == 921)
                    {
                        for (float num411 = 0.25f; num411 < 1f; num411 += 0.5f)
                        {
                            Vector2 vector107 = num411 * proj.velocity * 4f;
                            Main.EntitySpriteDraw(TextureAssets.Projectile[proj.type].Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY) - vector107, new Rectangle(0, y27, TextureAssets.Projectile[proj.type].Width(), num408 - 1), alpha12 * (1f - num411) * 0.75f, proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale, dir);
                        }
                    }
                    Main.EntitySpriteDraw(TextureAssets.Projectile[proj.type].Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY), new Rectangle(0, y27, TextureAssets.Projectile[proj.type].Width(), num408 - 1), alpha12, proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale, dir);
                    if (proj.type == 966)
                    {
                        Vector2 vector108 = new Vector2(0f, -30f);
                        vector108.Y += -1f + (float)(Math.Cos((float)(int)Main.mouseTextColor / 255f * ((float)Math.PI * 2f) * 2f) * 2.0);
                        Vector2 vector109 = new Vector2(-1f, -1f);
                        float num412 = 3f;
                        Vector2 vector110 = proj.Center + vector108;
                        Vector2 vector111 = proj.Center;
                        int num413 = (int)proj.ai[1];
                        if (num413 >= 0)
                        {
                            if (Main.npc[num413].active)
                            {
                                vector111 = Main.npc[num413].Center;
                            }
                            else
                            {
                                num413 = -1;
                            }
                        }
                        if (num413 <= -1)
                        {
                            Player player5 = Main.player[proj.owner];
                            vector111 = ((!player5.dead) ? player5.Center : (vector110 + new Vector2(2f, 0f)));
                        }
                        vector109 += (vector111 - vector110).SafeNormalize(Vector2.Zero) * num412;
                        Main.EntitySpriteDraw(TextureAssets.Projectile[proj.type].Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY) + vector108, new Rectangle(0, num408, TextureAssets.Projectile[proj.type].Width(), num408 - 1), alpha12, proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale, dir);
                        Main.EntitySpriteDraw(TextureAssets.Projectile[proj.type].Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY) + vector108 + vector109, new Rectangle(0, num408 * 2, TextureAssets.Projectile[proj.type].Width(), num408 - 1), alpha12, proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale, dir);
                    }
                    ModProjectile modProjectile = proj.ModProjectile;
                    if (modProjectile != null && ModContent.RequestIfExists(modProjectile.GlowTexture, out Asset<Texture2D> glowTexture, AssetRequestMode.ImmediateLoad))
                    {
                        Main.EntitySpriteDraw(glowTexture.Value, proj.position - Main.screenPosition + new Vector2(num138 + (float)num137, (float)(proj.height / 2) + proj.gfxOffY), new Rectangle(0, y27, TextureAssets.Projectile[proj.type].Width(), num408 - 1), new Color(250, 250, 250, proj.alpha), proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale, dir);
                    }
                    if (proj.type == 335)
                    {
                        Main.EntitySpriteDraw(TextureAssets.Projectile[proj.type].Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY), new Rectangle(0, y27, TextureAssets.Projectile[proj.type].Width(), num408 - 1), new Color(100, 100, 100, 0), proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale, dir);
                    }
                    if (proj.type == 897 || proj.type == 899)
                    {
                        int num414 = 279;
                        if (proj.type == 899)
                        {
                            num414 = 281;
                        }
                        Main.EntitySpriteDraw(TextureAssets.GlowMask[num414].Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY), new Rectangle(0, y27, TextureAssets.Projectile[proj.type].Width(), num408 - 1), Color.White, proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale, dir);
                    }
                    if (proj.type == 891)
                    {
                        float num415 = Utils.WrappedLerp(0.6f, 1f, (float)((int)Main.timeForVisualEffects % 100) / 100f);
                        Main.EntitySpriteDraw(color: new Color(num415, num415, num415, 150f), texture: TextureAssets.GlowMask[277].Value, position: new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY), sourceRectangle: new Rectangle(0, y27, TextureAssets.Projectile[proj.type].Width(), num408 - 1), rotation: proj.rotation, origin: new Vector2(num138, proj.height / 2 + num136), scale: proj.scale, effects: dir);
                    }
                    if (proj.type == 595)
                    {
                        Player player6 = Main.player[proj.owner];
                        if (player6.active && player6.body == 208)
                        {
                            for (float num416 = 0f; num416 <= 1f; num416 += 0.2f)
                            {
                                Color underShirtColor = player6.underShirtColor;
                                underShirtColor.A = (byte)(120f * (1f - num416 * 0.5f));
                                Main.EntitySpriteDraw(TextureAssets.Projectile[proj.type].Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY), new Rectangle(0, y27, TextureAssets.Projectile[proj.type].Width(), num408 - 1), underShirtColor, proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale * MathHelper.Lerp(0.8f, 1.3f, num416), dir);
                            }
                        }
                    }
                    if (proj.type == 387)
                    {
                        Main.EntitySpriteDraw(TextureAssets.EyeLaserSmall.Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY), new Rectangle(0, y27, TextureAssets.Projectile[proj.type].Width(), num408), new Color(255, 255, 255, 0), proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale, dir);
                    }
                    if (proj.type != 525 && proj.type != 960)
                    {
                        return;
                    }
                    int num417 = Main.TryInteractingWithMoneyTrough(proj);
                    if (num417 == 0)
                    {
                        return;
                    }
                    int num418 = (projectileColor.R + projectileColor.G + projectileColor.B) / 3;
                    if (num418 > 10)
                    {
                        int num419 = 94;
                        if (proj.type == 960)
                        {
                            num419 = 244;
                        }
                        Color selectionGlowColor = Colors.GetSelectionGlowColor(num417 == 2, num418);
                        Main.EntitySpriteDraw(TextureAssets.Extra[num419].Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY), new Rectangle(0, y27, TextureAssets.Projectile[proj.type].Width(), num408 - 1), selectionGlowColor, proj.rotation, new Vector2(num138, proj.height / 2 + num136), 1f, dir);
                    }
                    return;
                }


                if (proj.type == 434)
                {
                    Vector2 vector112 = new Vector2(proj.ai[0], proj.ai[1]);
                    Vector2 v = proj.position - vector112;
                    float num420 = (float)Math.Sqrt(v.X * v.X + v.Y * v.Y);
                    new Vector2(4f, num420);
                    float rotation30 = v.ToRotation() + (float)Math.PI / 2f;
                    Vector2 vector113 = Vector2.Lerp(proj.position, vector112, 0.5f);
                    Color red = Color.Red;
                    red.A = 0;
                    Color white6 = Color.White;
                    red *= proj.localAI[0];
                    white6 *= proj.localAI[0];
                    float num421 = (float)Math.Sqrt(proj.damage / 50);
                    Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, vector113 - Main.screenPosition, new Rectangle(0, 0, 1, 1), red, rotation30, Vector2.One / 2f, new Vector2(2f * num421, num420 + 8f), dir);
                    Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, vector113 - Main.screenPosition, new Rectangle(0, 0, 1, 1), red, rotation30, Vector2.One / 2f, new Vector2(4f * num421, num420), dir);
                    Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, vector113 - Main.screenPosition, new Rectangle(0, 0, 1, 1), white6, rotation30, Vector2.One / 2f, new Vector2(2f * num421, num420), dir);
                    return;
                }
                if (proj.type == 94 && proj.ai[1] > 6f)
                {
                    for (int num422 = 0; num422 < 10; num422++)
                    {
                        Color alpha13 = proj.GetAlpha(projectileColor);
                        float num423 = (float)(9 - num422) / 9f;
                        alpha13.R = (byte)((float)(int)alpha13.R * num423);
                        alpha13.G = (byte)((float)(int)alpha13.G * num423);
                        alpha13.B = (byte)((float)(int)alpha13.B * num423);
                        alpha13.A = (byte)((float)(int)alpha13.A * num423);
                        float num424 = (float)(9 - num422) / 9f;
                        Main.EntitySpriteDraw(TextureAssets.Projectile[proj.type].Value, new Vector2(proj.oldPos[num422].X - Main.screenPosition.X + num138 + (float)num137, proj.oldPos[num422].Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY), new Rectangle(0, 0, TextureAssets.Projectile[proj.type].Width(), TextureAssets.Projectile[proj.type].Height()), alpha13, proj.rotation, new Vector2(num138, proj.height / 2 + num136), num424 * proj.scale, dir);
                    }
                }
                if (proj.type == 301)
                {
                    for (int num425 = 0; num425 < 10; num425++)
                    {
                        Color alpha14 = proj.GetAlpha(projectileColor);
                        float num426 = (float)(9 - num425) / 9f;
                        alpha14.R = (byte)((float)(int)alpha14.R * num426);
                        alpha14.G = (byte)((float)(int)alpha14.G * num426);
                        alpha14.B = (byte)((float)(int)alpha14.B * num426);
                        alpha14.A = (byte)((float)(int)alpha14.A * num426);
                        float num427 = (float)(9 - num425) / 9f;
                        Main.EntitySpriteDraw(TextureAssets.Projectile[proj.type].Value, new Vector2(proj.oldPos[num425].X - Main.screenPosition.X + num138 + (float)num137, proj.oldPos[num425].Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY), new Rectangle(0, 0, TextureAssets.Projectile[proj.type].Width(), TextureAssets.Projectile[proj.type].Height()), alpha14, proj.rotation, new Vector2(num138, proj.height / 2 + num136), num427 * proj.scale, dir);
                    }
                }
                if (proj.type == 323 && proj.alpha == 0)
                {
                    for (int num428 = 1; num428 < 8; num428++)
                    {
                        float num429 = proj.velocity.X * (float)num428;
                        float num430 = proj.velocity.Y * (float)num428;
                        Color alpha15 = proj.GetAlpha(projectileColor);
                        float num431 = 0f;
                        if (num428 == 1)
                        {
                            num431 = 0.7f;
                        }
                        if (num428 == 2)
                        {
                            num431 = 0.6f;
                        }
                        if (num428 == 3)
                        {
                            num431 = 0.5f;
                        }
                        if (num428 == 4)
                        {
                            num431 = 0.4f;
                        }
                        if (num428 == 5)
                        {
                            num431 = 0.3f;
                        }
                        if (num428 == 6)
                        {
                            num431 = 0.2f;
                        }
                        if (num428 == 7)
                        {
                            num431 = 0.1f;
                        }
                        alpha15.R = (byte)((float)(int)alpha15.R * num431);
                        alpha15.G = (byte)((float)(int)alpha15.G * num431);
                        alpha15.B = (byte)((float)(int)alpha15.B * num431);
                        alpha15.A = (byte)((float)(int)alpha15.A * num431);
                        Main.EntitySpriteDraw(TextureAssets.Projectile[proj.type].Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137 - num429, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY - num430), new Rectangle(0, 0, TextureAssets.Projectile[proj.type].Width(), TextureAssets.Projectile[proj.type].Height()), alpha15, proj.rotation, new Vector2(num138, proj.height / 2 + num136), num431 + 0.2f, dir);
                    }
                }
                if (proj.type == 117 && proj.ai[0] > 3f)
                {
                    for (int num432 = 1; num432 < 5; num432++)
                    {
                        float num433 = proj.velocity.X * (float)num432;
                        float num434 = proj.velocity.Y * (float)num432;
                        Color alpha16 = proj.GetAlpha(projectileColor);
                        float num435 = 0f;
                        if (num432 == 1)
                        {
                            num435 = 0.4f;
                        }
                        if (num432 == 2)
                        {
                            num435 = 0.3f;
                        }
                        if (num432 == 3)
                        {
                            num435 = 0.2f;
                        }
                        if (num432 == 4)
                        {
                            num435 = 0.1f;
                        }
                        alpha16.R = (byte)((float)(int)alpha16.R * num435);
                        alpha16.G = (byte)((float)(int)alpha16.G * num435);
                        alpha16.B = (byte)((float)(int)alpha16.B * num435);
                        alpha16.A = (byte)((float)(int)alpha16.A * num435);
                        Main.EntitySpriteDraw(TextureAssets.Projectile[proj.type].Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137 - num433, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY - num434), new Rectangle(0, 0, TextureAssets.Projectile[proj.type].Width(), TextureAssets.Projectile[proj.type].Height()), alpha16, proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale, dir);
                    }
                }
                if (proj.bobber)
                {
                    if (proj.ai[1] > 0f && proj.ai[0] == 1f)
                    {
                        int num436 = (int)proj.ai[1];
                        Vector2 center5 = proj.Center;
                        float rotation31 = proj.rotation;
                        Vector2 vector114 = center5;
                        float num437 = polePosX - vector114.X;
                        float num438 = polePosY - vector114.Y;
                        rotation31 = (float)Math.Atan2(num438, num437);
                        if (proj.velocity.X > 0f)
                        {
                            dir = SpriteEffects.None;
                            rotation31 = (float)Math.Atan2(num438, num437);
                            rotation31 += 0.785f;
                            if (proj.ai[1] == 2342f)
                            {
                                rotation31 -= 0.785f;
                            }
                        }
                        else
                        {
                            dir = SpriteEffects.FlipHorizontally;
                            rotation31 = (float)Math.Atan2(0f - num438, 0f - num437);
                            rotation31 -= 0.785f;
                            if (proj.ai[1] == 2342f)
                            {
                                rotation31 += 0.785f;
                            }
                        }
                        Main.instance.LoadItem(num436);
                        Texture2D value112 = TextureAssets.Item[num436].Value;
                        Rectangle value113 = value112.Frame();
                        if (ItemID.Sets.IsFood[num436] && Main.itemAnimations[num436] != null)
                        {
                            value113 = Main.itemAnimations[num436].GetFrame(value112, 0);
                        }
                        Main.EntitySpriteDraw(value112, new Vector2(center5.X - Main.screenPosition.X, center5.Y - Main.screenPosition.Y), value113, projectileColor, rotation31, new Vector2(value113.Width / 2, value113.Height / 2), proj.scale, dir);
                    }
                    else if (proj.ai[0] <= 1f)
                    {
                        Main.EntitySpriteDraw(TextureAssets.Projectile[proj.type].Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY), new Rectangle(0, 0, TextureAssets.Projectile[proj.type].Width(), TextureAssets.Projectile[proj.type].Height()), proj.GetAlpha(projectileColor), proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale, dir);
                        if (proj.glowMask != -1)
                        {
                            Texture2D value114 = TextureAssets.GlowMask[proj.glowMask].Value;
                            Color newColor5 = Color.White;
                            if (proj.type == 993)
                            {
                                newColor5 = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
                            }
                            Main.EntitySpriteDraw(value114, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY), new Rectangle(0, 0, value114.Width, value114.Height), proj.GetAlpha(newColor5), proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale, dir);
                        }
                    }
                }
                else
                {
                    if (proj.ownerHitCheck && Main.player[proj.owner].gravDir == -1f)
                    {
                        if (Main.player[proj.owner].direction == 1)
                        {
                            dir = SpriteEffects.FlipHorizontally;
                        }
                        else if (Main.player[proj.owner].direction == -1)
                        {
                            dir = SpriteEffects.None;
                        }
                    }
                    Main.EntitySpriteDraw(TextureAssets.Projectile[proj.type].Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY), new Rectangle(0, 0, TextureAssets.Projectile[proj.type].Width(), TextureAssets.Projectile[proj.type].Height()), proj.GetAlpha(projectileColor), proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale, dir);
                    if (proj.glowMask != -1)
                    {
                        Main.EntitySpriteDraw(TextureAssets.GlowMask[proj.glowMask].Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY), new Rectangle(0, 0, TextureAssets.Projectile[proj.type].Width(), TextureAssets.Projectile[proj.type].Height()), new Color(250, 250, 250, proj.alpha), proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale, dir);
                    }
                    if (proj.type == 473)
                    {
                        Main.EntitySpriteDraw(TextureAssets.Projectile[proj.type].Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY), new Rectangle(0, 0, TextureAssets.Projectile[proj.type].Width(), TextureAssets.Projectile[proj.type].Height()), new Color(255, 255, 0, 0), proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale, dir);
                    }
                    if (proj.type >= 511 && proj.type <= 513)
                    {
                        Main.EntitySpriteDraw(TextureAssets.Projectile[proj.type].Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY), new Rectangle(0, 0, TextureAssets.Projectile[proj.type].Width(), TextureAssets.Projectile[proj.type].Height()), proj.GetAlpha(projectileColor) * 0.25f, proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale * (1f + proj.Opacity * 1.75f), dir);
                    }
                    ModProjectile modProjectile2 = proj.ModProjectile;
                    if (modProjectile2 != null && ModContent.RequestIfExists(modProjectile2.GlowTexture, out Asset<Texture2D> glowTexture2, AssetRequestMode.AsyncLoad))
                    {
                        Main.EntitySpriteDraw(glowTexture2.Value, proj.position - Main.screenPosition + new Vector2(num138 + (float)num137, (float)(proj.height / 2) + proj.gfxOffY), new Rectangle(0, 0, TextureAssets.Projectile[proj.type].Width(), TextureAssets.Projectile[proj.type].Height()), new Color(250, 250, 250, proj.alpha), proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale, dir);
                    }
                    if (proj.type == 312)
                    {
                        ulong seed3 = Main.TileFrameSeed;
                        for (int num439 = 0; num439 < 4; num439++)
                        {
                            Vector2 vector115 = new Vector2(Utils.RandomInt(ref seed3, -2, 3), Utils.RandomInt(ref seed3, -2, 3));
                            Main.EntitySpriteDraw(TextureAssets.GlowMask[proj.glowMask].Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY) + vector115, new Rectangle(0, 0, TextureAssets.Projectile[proj.type].Width(), TextureAssets.Projectile[proj.type].Height()), new Color(255, 255, 255, 255) * 0.2f, proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale, dir);
                        }
                    }
                }
                if (proj.type == 106)
                {
                    Texture2D value115 = TextureAssets.LightDisc.Value;
                    Vector2 position26 = new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2));
                    Rectangle value116 = value115.Frame();
                    Main.EntitySpriteDraw(value115, position26, value116, new Color(200, 200, 200, 0), proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale, dir);
                }
                if (proj.type >= 326 && proj.type <= 328)
                {
                    ulong seed4 = Main.TileFrameSeed;
                    for (int num440 = 0; num440 < 4; num440++)
                    {
                        Vector2 vector116 = new Vector2(Utils.RandomInt(ref seed4, -2, 3), Utils.RandomInt(ref seed4, -2, 3));
                        Main.EntitySpriteDraw(TextureAssets.Projectile[proj.type].Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2)) + vector116 - proj.velocity * 0.25f * num440, new Rectangle(0, 0, TextureAssets.Projectile[proj.type].Width(), TextureAssets.Projectile[proj.type].Height()), new Color(120, 120, 120, 60) * 1f, proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale - (float)num440 * 0.2f, dir);
                    }
                }
                if (proj.type == 554 || proj.type == 603)
                {
                    for (int num441 = 1; num441 < 5; num441++)
                    {
                        float num442 = proj.velocity.X * (float)num441 * 0.5f;
                        float num443 = proj.velocity.Y * (float)num441 * 0.5f;
                        Color alpha17 = proj.GetAlpha(projectileColor);
                        float num444 = 0f;
                        if (num441 == 1)
                        {
                            num444 = 0.4f;
                        }
                        if (num441 == 2)
                        {
                            num444 = 0.3f;
                        }
                        if (num441 == 3)
                        {
                            num444 = 0.2f;
                        }
                        if (num441 == 4)
                        {
                            num444 = 0.1f;
                        }
                        alpha17.R = (byte)((float)(int)alpha17.R * num444);
                        alpha17.G = (byte)((float)(int)alpha17.G * num444);
                        alpha17.B = (byte)((float)(int)alpha17.B * num444);
                        alpha17.A = (byte)((float)(int)alpha17.A * num444);
                        Main.EntitySpriteDraw(TextureAssets.Projectile[proj.type].Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137 - num442, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY - num443), new Rectangle(0, 0, TextureAssets.Projectile[proj.type].Width(), TextureAssets.Projectile[proj.type].Height()), alpha17, proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale, dir);
                    }
                }
                else if (proj.type == 604)
                {
                    int num445 = (int)proj.ai[1] + 1;
                    if (num445 > 7)
                    {
                        num445 = 7;
                    }
                    for (int num446 = 1; num446 < num445; num446++)
                    {
                        float num447 = proj.velocity.X * (float)num446 * 1.5f;
                        float num448 = proj.velocity.Y * (float)num446 * 1.5f;
                        Color alpha18 = proj.GetAlpha(projectileColor);
                        float num449 = 0f;
                        if (num446 == 1)
                        {
                            num449 = 0.4f;
                        }
                        if (num446 == 2)
                        {
                            num449 = 0.3f;
                        }
                        if (num446 == 3)
                        {
                            num449 = 0.2f;
                        }
                        if (num446 == 4)
                        {
                            num449 = 0.1f;
                        }
                        num449 = 0.4f - (float)num446 * 0.06f;
                        num449 *= 1f - (float)proj.alpha / 255f;
                        alpha18.R = (byte)((float)(int)alpha18.R * num449);
                        alpha18.G = (byte)((float)(int)alpha18.G * num449);
                        alpha18.B = (byte)((float)(int)alpha18.B * num449);
                        alpha18.A = (byte)((float)(int)alpha18.A * num449 / 2f);
                        float scale15 = proj.scale;
                        scale15 -= (float)num446 * 0.1f;
                        Main.EntitySpriteDraw(TextureAssets.Projectile[proj.type].Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137 - num447, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY - num448), new Rectangle(0, 0, TextureAssets.Projectile[proj.type].Width(), TextureAssets.Projectile[proj.type].Height()), alpha18, proj.rotation, new Vector2(num138, proj.height / 2 + num136), scale15, dir);
                    }
                }
                else
                {
                    if (proj.type != 553)
                    {
                        return;
                    }
                    for (int num450 = 1; num450 < 5; num450++)
                    {
                        float num451 = proj.velocity.X * (float)num450 * 0.4f;
                        float num452 = proj.velocity.Y * (float)num450 * 0.4f;
                        Color alpha19 = proj.GetAlpha(projectileColor);
                        float num453 = 0f;
                        if (num450 == 1)
                        {
                            num453 = 0.4f;
                        }
                        if (num450 == 2)
                        {
                            num453 = 0.3f;
                        }
                        if (num450 == 3)
                        {
                            num453 = 0.2f;
                        }
                        if (num450 == 4)
                        {
                            num453 = 0.1f;
                        }
                        alpha19.R = (byte)((float)(int)alpha19.R * num453);
                        alpha19.G = (byte)((float)(int)alpha19.G * num453);
                        alpha19.B = (byte)((float)(int)alpha19.B * num453);
                        alpha19.A = (byte)((float)(int)alpha19.A * num453);
                        Main.EntitySpriteDraw(TextureAssets.Projectile[proj.type].Value, new Vector2(proj.position.X - Main.screenPosition.X + num138 + (float)num137 - num451, proj.position.Y - Main.screenPosition.Y + (float)(proj.height / 2) + proj.gfxOffY - num452), new Rectangle(0, 0, TextureAssets.Projectile[proj.type].Width(), TextureAssets.Projectile[proj.type].Height()), alpha19, proj.rotation, new Vector2(num138, proj.height / 2 + num136), proj.scale, dir);
                    }
                }
            }
        }

        */

    }
}
