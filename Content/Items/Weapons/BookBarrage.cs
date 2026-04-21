using ChangedSpecialMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Mono.Cecil;
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
            Item.width = 28; // The item texture's width.
            Item.height = 32; // The item texture's height.
            Item.scale = 1.25f;
            Item.rare = ItemRarityID.Pink;

            Item.useStyle = ItemUseStyleID.Swing; // The useStyle of the Item.
            Item.useTime = 25; // The time span of using the weapon. Remember in terraria, 60 frames is a second.
            Item.useAnimation = 35; // The time span of the using animation of the weapon, suggest setting it the same as useTime.
            Item.autoReuse = false; // Whether the weapon can be used more than once automatically by holding the use button.

            Item.DamageType = DamageClass.Melee; // Whether your item is part of the melee class.
            Item.damage = 80; // The damage your item deals.
            Item.knockBack = 9; // The force of knockback of the weapon. Maximum is 20
            Item.crit = 4; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.

            Item.value = Item.buyPrice(0, 6, 0, 0);
            //Item.rare = ModContent.RarityType<ExampleModRarity>(); // Give this item our custom rarity.
            Item.UseSound = SoundID.Item1; // The sound when the weapon is being used.
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
