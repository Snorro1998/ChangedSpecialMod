using ChangedSpecialMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Weapons
{
    public class Twintacle : ModItem
    {
        float projectileKnockback = 1;

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 50;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.LightRed;

            Item.shoot = ModContent.ProjectileType<TwintacleProjectile>();
            Item.shootSpeed = 4;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.UseSound = SoundID.Item152;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var len = velocity.Length();
            var rot = velocity.ToRotation() + 0.1f * Math.PI * Math.Sign(velocity.X);
            var vel = new Vector2((float)Math.Cos(rot), (float)Math.Sin(rot)) * len;
            Projectile.NewProjectile(player.GetSource_FromAI(), position, vel, Item.shoot, Item.damage, projectileKnockback, player.whoAmI);
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<Twintacle>());
            recipe.AddIngredient(ModContent.ItemType<Mollash>());
            recipe.AddIngredient(ItemID.BlackInk, 5);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        public override bool MeleePrefix()
        {
            return true;
        }
    }
}
