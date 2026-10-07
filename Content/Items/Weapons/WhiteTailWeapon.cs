using ChangedSpecialMod.Content.Items.Placeable.Crystals;
using ChangedSpecialMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Weapons
{
    public class WhiteTailWeapon : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.knockBack = 2f;
            Item.mana = 10;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Meowmere;//Sounds.SoundCat;
            Item.noMelee = true;

            Item.DamageType = DamageClass.Summon;
            //Item.buffType = ModContent.BuffType<Buffs.TabbyStaffBuff>();
            Item.shoot = ModContent.ProjectileType<WhiteTailProjectile>();
        }

        /*
        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        { 
            Projectile.NewProjectile( source, position, velocity, type, damage, knockback, player.whoAmI, 0f ); 
            Projectile.NewProjectile( source, position, velocity, type, damage, knockback, player.whoAmI, MathHelper.Pi * (1.0f / 0.1f) ); 
            return false; 
        }
        */

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
