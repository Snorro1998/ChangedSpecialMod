using ChangedSpecialMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Weapons
{
    public class BookBarrage : ModItem
    {
        private const int projectileDamage = 60;
        private const float projectileKnockback = 0.3f;
        private const int nBooks = 4;

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.scale = 1.25f;
            Item.rare = ItemRarityID.Pink;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.useAnimation = 35;
            Item.autoReuse = false;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 80;
            Item.knockBack = 9;
            Item.crit = 4;

            Item.value = Item.buyPrice(0, 6, 0, 0);
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<BookProjectile>();
            Item.shootsEveryUse = true;
            Item.shootSpeed = 15;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(SoundID.Item8, player.Center);
            var pos = position + new Vector2(0, 16 * -30);
            var mousePos = Main.MouseWorld;

            if (mousePos.X < player.Center.X)
                pos.X = mousePos.X + 16 * 10;
            else
                pos.X = mousePos.X + 16 * -10;

            for (int i = 0; i < nBooks; i++)
            {
                float dirX = mousePos.X - pos.X;
                float dirY = mousePos.Y - pos.Y;
                var targetRotation = (float)Math.Atan2(dirY, dirX);
                targetRotation += (float)(Main.rand.NextFloat(-0.1f, 0.1f) * Math.PI);
                var moveVector = new Vector2((float)Math.Cos(targetRotation), (float)Math.Sin(targetRotation)) * Item.shootSpeed;
                Projectile.NewProjectile(source, pos, moveVector, Item.shoot, projectileDamage, projectileKnockback, player.whoAmI);
                pos.Y -= 5 * 16;
            }

            return false;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(2))
            {
                target.AddBuff(BuffID.Slow, 60);
            }

            base.OnHitNPC(player, target, hit, damageDone);
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<BookBarrage>());

            recipe.AddIngredient(ModContent.ItemType<Literature>());
            recipe.AddIngredient(ItemID.Book, 10);
            recipe.AddIngredient(ItemID.SoulofFright, 5);
            recipe.AddIngredient(ItemID.SoulofMight, 5);
            recipe.AddIngredient(ItemID.SoulofSight, 5);
            recipe.AddTile(TileID.Bookcases);

            recipe.Register();
        }
    }
}
