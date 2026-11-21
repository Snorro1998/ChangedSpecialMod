using ChangedSpecialMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Weapons
{
    public class Encyclopedia : ModItem
    {
        int projectileDamage = 20;
        float projectileKnockback = 1;

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.scale = 1.25f;
            Item.rare = ItemRarityID.Green;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.autoReuse = false;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 30;
            Item.knockBack = 8;
            Item.crit = 4;

            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<PaperPlane>();
            Item.shootsEveryUse = true;
            Item.shootSpeed = 10;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, new Vector2(player.direction * Item.shootSpeed, -2), Item.shoot, projectileDamage, projectileKnockback, player.whoAmI);
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
    }
}
