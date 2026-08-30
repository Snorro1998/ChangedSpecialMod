using ChangedSpecialMod.Content.Items.Placeable.Crystals;
using ChangedSpecialMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Weapons
{
    public class GreenCrystalStaff : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 42;
            Item.damage = 18;
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 3.5f;
            Item.mana = 7;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.staff[Type] = true;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<GreenCrystalMagicWeaponProjectile>();
            Item.shootSpeed = 4f;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 1, 50, 0);
            Item.UseSound = SoundID.Item20;
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        { 
            Projectile.NewProjectile( source, position, velocity, type, damage, knockback, player.whoAmI, 0f ); 
            Projectile.NewProjectile( source, position, velocity, type, damage, knockback, player.whoAmI, MathHelper.Pi * (1.0f / 0.1f) ); 
            return false; 
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SilverBar, 8)
                .AddIngredient(ModContent.ItemType<CrystalGreen>(), 10)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.TungstenBar, 8)
                .AddIngredient(ModContent.ItemType<CrystalGreen>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
