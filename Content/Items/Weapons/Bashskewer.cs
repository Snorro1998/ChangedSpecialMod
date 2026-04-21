using ChangedSpecialMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Weapons
{
    public class Bashskewer : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 44;
            Item.scale = 1.25f;
            Item.rare = ItemRarityID.LightRed;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 60;
            Item.knockBack = 8;
            Item.crit = 4;

            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.UseSound = SoundID.Item1;

            Item.shoot = ProjectileID.WandOfFrostingFrost;
            Item.shootSpeed = 10;
        }

        private void GetPointOnSwungItemPath(Player player, float spriteWidth, float spriteHeight, float normalizedPointOnPath, float itemScale, out Vector2 location, out Vector2 outwardDirection)
        {
            float num = (float)Math.Sqrt(spriteWidth * spriteWidth + spriteHeight * spriteHeight);
            float num2 = (float)(player.direction == 1).ToInt() * ((float)Math.PI / 2f);
            if (player.gravDir == -1f)
            {
                num2 += (float)Math.PI / 2f * (float)player.direction;
            }
            outwardDirection = player.itemRotation.ToRotationVector2().RotatedBy(3.926991f + num2);
            location = player.RotatedRelativePoint(player.itemLocation + outwardDirection * num * normalizedPointOnPath * itemScale);
        }


        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            //searched so long for this. It was in player.cs wtf
            if (Main.rand.NextBool(3))
            {
                GetPointOnSwungItemPath(player, 70f, 70f, 0.2f + 0.8f * Main.rand.NextFloat(), 1, out var location, out var outwardDirection);
                Vector2 vector = outwardDirection.RotatedBy((float)Math.PI / 2f * (float)player.direction * player.gravDir);
                //Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Frost);
                Dust.NewDustPerfect(location, DustID.Frost, vector * 4f, 100, default(Color), 1.25f).noGravity = true;
            }
        }



        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn2, 180);
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<Bashskewer>());
            recipe.AddIngredient(ModContent.ItemType<Dreadhorn>());
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
