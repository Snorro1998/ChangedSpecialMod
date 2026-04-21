using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Weapons
{
    public class Daemonspike : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 44;
            Item.scale = 1.25f;
            Item.rare = ItemRarityID.Pink;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 90;
            Item.knockBack = 8;
            Item.crit = 4;

            Item.value = Item.buyPrice(0, 6, 0, 0);
            Item.UseSound = SoundID.Item1;

            //BallofFrost, bouncing ball
            //Frostboltstaff
            //IceBolt
            //FrostBlastFriendly, fast
            //FrostBeam, dont use, from ice golem
            //FrostHydra, dont use

            Item.shoot = ProjectileID.WandOfFrostingFrost;
            Item.shootSpeed = 10;
        }

        private void GetPointOnSwungItemPath(Player player, float spriteWidth, float spriteHeight, float normalizedPointOnPath, float itemScale, out Vector2 location, out Vector2 outwardDirection)
        {
            float num = (float)Math.Sqrt(spriteWidth * spriteWidth + spriteHeight * spriteHeight);
            float num2 = (float)(player.direction == 1).ToInt() * ((float)Math.PI / 2f);
            if (player.gravDir == -1f)
            {
                num2 += (float)Math.PI / 2f * (float)player.direction;
            }
            outwardDirection = player.itemRotation.ToRotationVector2().RotatedBy(3.926991f + num2);
            location = player.RotatedRelativePoint(player.itemLocation + outwardDirection * num * normalizedPointOnPath * itemScale);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(player.GetSource_FromAI(), position, velocity, ProjectileID.BallofFrost, Item.damage, knockback, player.whoAmI);
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            //searched so long for this. It was in player.cs wtf
            if (Main.rand.NextBool(3))
            {
                GetPointOnSwungItemPath(player, 70f, 70f, 0.2f + 0.8f * Main.rand.NextFloat(), 1, out var location, out var outwardDirection);
                Vector2 vector = outwardDirection.RotatedBy((float)Math.PI / 2f * (float)player.direction * player.gravDir);
                //Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Frost);
                Dust.NewDustPerfect(location, DustID.Frost, vector * 4f, 100, default(Color), 1.25f).noGravity = true;
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 240);
            target.AddBuff(BuffID.Frostburn2, 240);
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<Daemonspike>());
            recipe.AddIngredient(ModContent.ItemType<Bashskewer>());
            recipe.AddIngredient(ItemID.SoulofFright, 5);
            recipe.AddIngredient(ItemID.SoulofMight, 5);
            recipe.AddIngredient(ItemID.SoulofSight, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
