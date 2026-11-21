using ChangedSpecialMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Weapons
{
    public class Literature : ModItem
    {
        private const int projectileDamage = 40;
        private const float projectileKnockback = 0.3f;

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.scale = 1.25f;
            Item.rare = ItemRarityID.LightRed;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.useAnimation = 35;
            Item.autoReuse = false;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 60;
            Item.knockBack = 9;
            Item.crit = 4;

            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<PaperPlane2>();
            Item.shootsEveryUse = true;
            Item.shootSpeed = 5;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(SoundID.Item8, player.Center);
            Projectile.NewProjectile(source, position, velocity, Item.shoot, projectileDamage, projectileKnockback, player.whoAmI);
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
            Recipe recipe = Recipe.Create(ModContent.ItemType<Literature>());

            recipe.AddIngredient(ModContent.ItemType<Encyclopedia>());
            recipe.AddIngredient(ItemID.Hellstone, 10);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.Bookcases);

            recipe.Register();
        }
    }
}
