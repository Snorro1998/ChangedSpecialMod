using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Weapons
{
    public class Dreadhorn : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40; // The item texture's width.
            Item.height = 44; // The item texture's height.
            Item.scale = 1.25f;
            Item.rare = ItemRarityID.Green;

            Item.useStyle = ItemUseStyleID.Swing; // The useStyle of the Item.
            Item.useTime = 40; // The time span of using the weapon. Remember in terraria, 60 frames is a second. (40)
            Item.useAnimation = 40; // The time span of the using animation of the weapon, suggest setting it the same as useTime. (40)
            Item.autoReuse = false; // Whether the weapon can be used more than once automatically by holding the use button.

            Item.DamageType = DamageClass.Melee; // Whether your item is part of the melee class.
            Item.damage = 30; // The damage your item deals.
            Item.knockBack = 8; // The force of knockback of the weapon. Maximum is 20
            Item.crit = 4; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.

            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.UseSound = SoundID.Item1; // The sound when the weapon is being used.

            Item.shoot = ProjectileID.WandOfFrostingFrost;
            Item.shootSpeed = 8;
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

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            knockback = 0f;
        }


        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            //searched so long for this. It was in player.cs wtf
            /*
             * 		if (sItem.type == 121)
		{
			for (int i = 0; i < 2; i++)
			{
				this.GetPointOnSwungItemPath(70f, 70f, 0.2f + 0.8f * Main.rand.NextFloat(), this.GetAdjustedItemScale(sItem), out var location, out var outwardDirection);
				Vector2 vector = outwardDirection.RotatedBy((float)Math.PI / 2f * (float)base.direction * this.gravDir);
				Dust.NewDustPerfect(location, 6, vector * 4f, 100, default(Color), 2.5f).noGravity = true;
			}
		}
            */

            if (Main.rand.NextBool(3))
            {

                GetPointOnSwungItemPath(player, 70f, 70f, 0.2f + 0.8f * Main.rand.NextFloat(), 1, out var location, out var outwardDirection);
                Vector2 vector = outwardDirection.RotatedBy((float)Math.PI / 2f * (float)player.direction * player.gravDir);
                //Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Frost);
                Dust.NewDustPerfect(location, DustID.Frost, vector * 4f, 100, default(Color), 1.25f).noGravity = true;
            }
        }


        /*
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Inflict the OnFire debuff for 1 second onto any NPC/Monster that this hits.
            // 60 frames = 1 second
            target.AddBuff(BuffID.Frostburn, 120);
        }
        */
    }
}
