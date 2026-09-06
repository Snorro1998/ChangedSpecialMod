using ChangedSpecialMod.Content.Items.Weapons;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
    // This projectile showcases advanced AI code. Of particular note is a showcase on how projectiles can stick to NPCs in a manner similar to the behavior of vanilla weapons such as Bone Javelin, Daybreak, Blood Butcherer, Stardust Cell Minion, and Tentacle Spike. This code is modeled closely after Bone Javelin.
    public class ToyBlockProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16; // The width of projectile hitbox
            Projectile.height = 16; // The height of projectile hitbox
            Projectile.aiStyle = 0; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.DamageType = DamageClass.Ranged; // Makes the projectile deal ranged damage. You can set in to DamageClass.Throwing, but that is not used by any vanilla items
            Projectile.penetrate = 2; // How many monsters the projectile can penetrate.
            Projectile.timeLeft = 600; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = true; // Can the projectile collide with tiles?
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = Main.rand.Next(Main.projFrames[Projectile.type]);
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.35f;

            var rotationSpeed = 0.05f;
            if (Projectile.velocity.X < 0)
                rotationSpeed *= -1;

            Projectile.rotation += rotationSpeed;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            ChangedUtils.DrawProjectileCentered(Projectile, lightColor);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            Vector2 usePos = Projectile.position;

            Color toyBlockColor = Color.White;
            if (Projectile.frame == 0)
                toyBlockColor = new Color(252, 56, 39);
            else if (Projectile.frame == 1)
                toyBlockColor = new Color(255, 240, 199);
            else if (Projectile.frame == 2)
                toyBlockColor = new Color(61, 111, 201);
            else if (Projectile.frame == 3)
                toyBlockColor = new Color(240, 136, 31);
            else if (Projectile.frame == 4)
                toyBlockColor = new Color(135, 153, 26);

            // Spawn some dusts upon javelin death
            for (int i = 0; i < 20; i++)
            {
                // Create a new dust
                Dust dust = Dust.NewDustDirect(usePos, Projectile.width, Projectile.height, DustID.TintableDust);
                dust.position = (dust.position + Projectile.Center) / 2f;
                dust.noGravity = true;
                dust.color = toyBlockColor;
            }

            if (Projectile.owner == Main.myPlayer)
            {
                int item = 0;
                if (Main.rand.NextBool(5))
                    item = Item.NewItem(Projectile.GetSource_DropAsItem(), Projectile.getRect(), ModContent.ItemType<ToyBlock>());

                if (Main.netMode == NetmodeID.MultiplayerClient && item >= 0)
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f);
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 10;
            return true;
        }
    }
}
