using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using ChangedSpecialMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Weapons
{
    public class WateringCanWeapon : ModItem
    {
        private int useType = 0;

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 42;
            Item.damage = 17;
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 3.5f;
            Item.mana = 7;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.staff[Type] = true;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WaterStream;
            Item.shootSpeed = 8f;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 1, 50, 0);
            Item.UseSound = null;
        }

        public override bool CanUseItem(Player player)
        {
            if (Main.projectile.Any(x => x.active && x.type == ModContent.ProjectileType<WateringCanWeaponProjectile>() && x.owner == player.whoAmI))
                return false;

            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (useType == 2)
            {
                damage = (int)(damage * 1.25f);
                velocity *= 1.5f;
                Projectile.NewProjectile(player.GetSource_FromAI(), position, velocity, ModContent.ProjectileType<WateringCanWeaponProjectile>(), damage, knockback, player.whoAmI);
                return false;
            }

            var projectileID = Projectile.NewProjectile(player.GetSource_FromAI(), position, velocity, ProjectileID.WaterStream, damage, knockback, player.whoAmI);
            if (projectileID != -1)
            {
                var projectile = Main.projectile[projectileID];
                projectile.penetrate = 1;
            }
            return false;
            //return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override bool? UseItem(Player player)
        {
            useType = player.altFunctionUse;

            if (useType == 2)
            {
                Item.noUseGraphic = true;
                SoundEngine.PlaySound(SoundID.Item1, player.Center);
            }
            else
            {
                Item.noUseGraphic = false;
                SoundEngine.PlaySound(SoundID.Item21, player.Center);
            }

                
            return null;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LeadBar, 8)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.IronBar, 8)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<WateringCan>(), 1)
                .Register();
        }
    }
}
