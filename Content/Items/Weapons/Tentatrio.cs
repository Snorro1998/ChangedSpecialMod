using ChangedSpecialMod.Content.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace ChangedSpecialMod.Content.Items.Weapons
{
    public class Tentatrio : ModItem
    {
        private float projectileKnockback = 1;
        private int nExtraTentacles = 2;

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 70;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Pink;

            Item.shoot = ModContent.ProjectileType<TentatrioProjectile>();
            Item.shootSpeed = 4;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item152;
            Item.value = Item.buyPrice(0, 6, 0, 0);
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var length = velocity.Length();
            var rotation = velocity.ToRotation();

            for (int i = 0; i < nExtraTentacles; i++)
            {
                var rot = velocity.ToRotation() + 0.1f * Math.PI * Math.Sign(velocity.X) * (i + 1);
                var vel = new Vector2((float)Math.Cos(rot), (float)Math.Sin(rot)) * length;
                Projectile.NewProjectile(player.GetSource_FromAI(), position, vel, Item.shoot, Item.damage, projectileKnockback, player.whoAmI);
            }
            
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<Tentatrio>());

            recipe.AddIngredient(ModContent.ItemType<Twintacle>());
            recipe.AddIngredient(ItemID.SoulofFright, 5);
            recipe.AddIngredient(ItemID.SoulofMight, 5);
            recipe.AddIngredient(ItemID.SoulofSight, 5);
            recipe.AddTile(TileID.Anvils);

            recipe.Register();
        }

        public override bool MeleePrefix()
        {
            return true;
        }
    }
}
