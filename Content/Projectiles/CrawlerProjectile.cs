using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
    public class CrawlerProjectile : ModProjectile
    {
        public enum MoveDirection
        {
            Invalid,
            Left,
            Right,
            Up,
            Down
        }

        private MoveDirection moveDirectionOld = MoveDirection.Invalid;
        private MoveDirection moveDirection = MoveDirection.Down;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
        }

        private void ChangeDirection(MoveDirection newDirection)
        {
            moveDirectionOld = moveDirection;
            moveDirection = newDirection;
        }

        public override void OnSpawn(IEntitySource source)
        {
            var tilePos = Projectile.position / 16;
            var xPos = (int)tilePos.X;
            var yPos = (int)tilePos.Y;
            Projectile.position = new Vector2(xPos * 16, yPos * 16);
            Projectile.velocity = Vector2.Zero;
        }

        private void UpdateDir()
        {
            var tilePos = Projectile.position / 16;
            var xPos = (int)tilePos.X;
            var yPos = (int)tilePos.Y;
            Projectile.position = new Vector2(xPos * 16, yPos * 16);

            bool left = IsSolid(new Vector2(xPos - 1, yPos));
            bool right = IsSolid(new Vector2(xPos + 1, yPos));
            bool up = IsSolid(new Vector2(xPos, yPos - 1));
            bool down = IsSolid(new Vector2(xPos, yPos + 1));

            bool upLeft = IsSolid(new Vector2(xPos - 1, yPos - 1));
            bool upRight = IsSolid(new Vector2(xPos + 1, yPos - 1));
            bool downLeft = IsSolid(new Vector2(xPos - 1, yPos + 1));
            bool downRight = IsSolid(new Vector2(xPos + 1, yPos + 1));

            // Going down
            if (moveDirection == MoveDirection.Down)
            {
                // Can go around a ceiling corner
                if (!left && upLeft)
                    ChangeDirection(MoveDirection.Left);
                // Can go around a ceiling corner
                else if (!right && upRight)
                    ChangeDirection(MoveDirection.Right);

                // Hitting the floor
                if (down)
                {
                    if (right)
                        ChangeDirection(MoveDirection.Left);
                    else
                        ChangeDirection(MoveDirection.Right);
                }
            }

            // Going up
            else if (moveDirection == MoveDirection.Up)
            {
                // Can go around a ceiling corner
                if (!left && downLeft)
                    ChangeDirection(MoveDirection.Left);
                // Can go around a ceiling corner
                else if (!right && downRight)
                    ChangeDirection(MoveDirection.Right);

                // hitting the ceiling
                if (up)
                {
                    if (right)
                        ChangeDirection(MoveDirection.Left);
                    else
                        ChangeDirection(MoveDirection.Right);
                }
            }

            // Going right 
            else if (moveDirection == MoveDirection.Right)
            {
                // Can go around a corner
                if (!up && (upLeft || upRight))
                    ChangeDirection(MoveDirection.Up);
                else if (!down && (downLeft || downRight))
                    ChangeDirection(MoveDirection.Down);

                // Hitting a wall
                if (right)
                {
                    if (down)
                        ChangeDirection(MoveDirection.Up);
                    else
                        ChangeDirection(MoveDirection.Down);
                }

            }

            // Going left 
            else if (moveDirection == MoveDirection.Left)
            {
                // Can go around a corner
                if (!up && (upLeft || upRight))
                    ChangeDirection(MoveDirection.Up);
                else if (!down && (downLeft || downRight))
                    ChangeDirection(MoveDirection.Down);

                // Hitting a wall
                if (left)
                {
                    if (down)
                        ChangeDirection(MoveDirection.Up);
                    else
                        ChangeDirection(MoveDirection.Down);
                }
            }
        }

        public override void AI()
        {
            if (Projectile.ai[1] == 0)
                UpdateDir();
            else
            {
                var movement = new Vector2(0, 0);
                switch (moveDirection)
                {
                    case MoveDirection.Left:
                        movement = new Vector2(-8, 0);
                        break;
                    case MoveDirection.Right:
                        movement = new Vector2(8, 0);
                        break;
                    case MoveDirection.Up:
                        movement = new Vector2(0, -8);
                        break;
                    case MoveDirection.Down:
                        movement = new Vector2(0, 8);
                        break;
                }

                Projectile.position += movement;
            }

            if (Projectile.ai[1] >= 2)
                Projectile.ai[1] = 0;
            else
                Projectile.ai[1]++;
        }

        private bool IsSolid(Vector2 worldPosition)
        {
            int tileX = (int)(worldPosition.X);
            int tileY = (int)(worldPosition.Y);

            if (!WorldGen.InWorld(tileX, tileY))
                return false;

            Tile tile = Framing.GetTileSafely(tileX, tileY);

            return tile.HasTile &&
                   Main.tileSolid[tile.TileType] &&
                   !Main.tileSolidTop[tile.TileType];
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Don't destroy the projectile when it hits a tile.
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            ChangedUtils.DrawProjectileCentered(Projectile, lightColor);
            return false;
        }
    }
}