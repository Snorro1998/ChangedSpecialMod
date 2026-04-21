using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
	public class Locker : ModTile
	{
		public override void SetStaticDefaults() 
        {
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.IgnoredByNpcStepUp[Type] = true; // This line makes NPCs not try to step up this tile during their movement. Only use this for furniture with solid tops.
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
		}

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            base.KillMultiTile(i, j, frameX, frameY);
            var rareChance = Main.drunkWorld ? 1 : 30;
            var player = ChangedUtils.GetClosestPlayer(i, j);
            if (player != null && ChangedUtils.IsDrunk(player))
                rareChance = 1;
            if (Main.rand.Next(rareChance) == 0)
                SpawnHungryLocker(i, j);
            else
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, (j + 1) * 16, 16, 16, ModContent.ItemType<Items.Placeable.Furniture.Locker>());
        }

        public override bool CanDrop(int i, int j)
        {
            return false;
        }

        private void SpawnHungryLocker(int i, int j)
        {
            var player = Main.LocalPlayer;
            var source = WorldGen.GetItemSource_FromTileBreak(i, j);
            NPC npcHand = NPC.NewNPCDirect(source, (i + 1) * 16, (j + 3) * 16, ModContent.NPCType<HungryLocker>());
        }

        public override void RandomUpdate(int i, int j)
        {
            var player = ChangedUtils.GetClosestPlayer(i, j);
            if (ChangedUtils.IsDrunk(player))
            {
                var dist = Vector2.DistanceSquared(player.Center, new Vector2(i * 16, j * 16));
                var minDist = 16 * 30;
                if (dist < minDist * minDist)
                    // Destroys itself, which spawns a hungry locker
                    WorldGen.KillTile(i, j, false, false);
            }
        }
    }
}
