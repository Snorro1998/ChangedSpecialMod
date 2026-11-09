using ChangedSpecialMod.Content.Gores;
using ChangedSpecialMod.Content.Projectiles;
using ChangedSpecialMod.Content.Walls;
using ChangedSpecialMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
	public class RedGasTank : ModTile
	{
		public override void SetStaticDefaults() 
        {
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1xX);
			TileObjectData.newTile.RandomStyleRange = 5;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.addTile(Type);
			DustType = DustID.WoodFurniture;
            AddMapEntry(new Color(255, 20, 20));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            base.KillMultiTile(i, j, frameX, frameY);

            // Rarely make it fly away with a fart sound or always on the drunk world seed
            var rareChance = Main.drunkWorld ? 1 : 30;
            var player = ChangedUtils.GetClosestPlayer(i, j);
            if (player != null && ChangedUtils.IsDrunk(player))
            {
                rareChance = 1;
            }
            if (Main.rand.Next(rareChance) == 0)
            {
                ChangedUtils.CreateFlyingGasTank(ModContent.ProjectileType<RedFlyingGasTank>(), i, j);
            }
        }

        // Ignore style nonsense and always drop the correct item
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<Items.Placeable.Furniture.RedGasTank>());
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (Main.gameInactive || Main.gamePaused)
                return;

            var tile = Framing.GetTileSafely(i, j);
            var isInGasRoom = tile.WallType == ModContent.WallType<LabGasRoomWall>();

            if (!isInGasRoom)
                return;

            var closestPlayer = ChangedUtils.GetClosestPlayer(i, j);
            var isDrunk = ChangedUtils.IsDrunk(closestPlayer);

            if (Main.rand.NextBool(100))
            {
                // Vanilla fog machine code
                Vector2 position = new Vector2(i, j).ToWorldCoordinates();
                int type = ModContent.GoreType<GasCloudGore>();
                float scale = 8f + Main.rand.NextFloat() * 1.6f;
                position = position + new Vector2(0f, -18f);
                Vector2 velocity = Main.rand.NextVector2Circular(0.7f, 0.25f) * 0.4f + Main.rand.NextVector2CircularEdge(1f, 0.4f) * 0.1f;
                velocity *= 4f;
                Gore.NewGorePerfect(new EntitySource_TileUpdate(i, j), position, velocity, type, scale);
            }

            if (Main.rand.NextBool(100))
            {
                var coords = TileObjectData.TopLeft(i, j);
                if (coords == Point16.NegativeOne)
                    return;
                var particlePos = coords.ToWorldCoordinates(-8, -16);

                // For some reason, only gore ids greater than 60 are defined
                // These are the ids for clouds
                var particleType = Utils.SelectRandom(Main.rand, 11, 12, 13);
                //var particleType = Utils.SelectRandom(Main.rand, GoreID.FartCloud1, GoreID.FartCloud2, GoreID.FartCloud3);
                var particle = Gore.NewGorePerfect(new EntitySource_TileUpdate(coords.X, coords.Y), particlePos, new Vector2(0, -1), particleType, Main.rand.NextFloat(0.1f, 0.9f));

                if (isDrunk && Main.rand.NextBool(5))
                {
                    if (Main.rand.NextBool(2))
                    {
                        particle.scale *= 4;
                        SoundEngine.PlaySound(SoundID.NPCDeath64, new Vector2(i * 16, j * 16));
                    }
                    else
                    {
                        particle.scale *= 2;
                        SoundEngine.PlaySound(SoundID.Item16, new Vector2(i * 16, j * 16));
                    }
                }
            }
        }
    }
}
