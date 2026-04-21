using ChangedSpecialMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Mono.Cecil;
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
            Item.width = 28; // The item texture's width.
            Item.height = 32; // The item texture's height.
            Item.scale = 1.25f;
            Item.rare = ItemRarityID.Green;

            Item.useStyle = ItemUseStyleID.Swing; // The useStyle of the Item.
            Item.useTime = 40; // The time span of using the weapon. Remember in terraria, 60 frames is a second.
            Item.useAnimation = 40; // The time span of the using animation of the weapon, suggest setting it the same as useTime.
            Item.autoReuse = false; // Whether the weapon can be used more than once automatically by holding the use button.

            Item.DamageType = DamageClass.Melee; // Whether your item is part of the melee class.
            Item.damage = 30; // The damage your item deals.
            Item.knockBack = 8; // The force of knockback of the weapon. Maximum is 20
            Item.crit = 4; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.

            Item.value = Item.buyPrice(0, 2, 0, 0);
            //Item.rare = ModContent.RarityType<ExampleModRarity>(); // Give this item our custom rarity.
            Item.UseSound = SoundID.Item1; // The sound when the weapon is being used.
            Item.shoot = ModContent.ProjectileType<PaperPlane>();
            Item.shootsEveryUse = true;
            Item.shootSpeed = 10;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, new Vector2(player.direction * Item.shootSpeed, -2), Item.shoot, projectileDamage, projectileKnockback, player.whoAmI);
            return false;
        }
        /*
        public override void UseItemFrame(Player player)
        {
            var t = player.itemTime;

            if (t == 0.3 * player.itemTimeMax || t == 0.6 * player.itemTimeMax)
                Projectile.NewProjectile(player.GetSource_FromAI(), player.position, new Vector2(player.direction * Item.shootSpeed, -2), Item.shoot, projectileDamage, projectileKnockback, player.whoAmI);

        }
        */

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
