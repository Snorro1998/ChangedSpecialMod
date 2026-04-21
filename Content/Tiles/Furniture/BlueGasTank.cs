using ChangedSpecialMod.Content.Projectiles;
using ChangedSpecialMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
	public class BlueGasTank : ModTile
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
                ChangedUtils.CreateFlyingGasTank(ModContent.ProjectileType<BlueFlyingGasTank>(), i, j);
            }
        }

        // Ignore style nonsense and always drop the correct item
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<Items.Placeable.Furniture.BlueGasTank>());
        }
    }
}
