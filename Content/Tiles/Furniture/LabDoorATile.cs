using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
	public class LabDoorATile : ModTile
	{
        public const int ElevatorReach = 20;

		public override void SetStaticDefaults() 
        {
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = false;
			TileID.Sets.FramesOnKillWall[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.AnchorWall = true;
            TileObjectData.addTile(Type);
			DustType = DustID.WoodFurniture;
            AddMapEntry(new Color(200, 200, 200), Language.GetText("Mods.ChangedSpecialMod.Tiles.Elevator"));
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            if (!player.IsWithinSnappngRangeToTile(i, j, 5 * 16))
                return false;

            SoundEngine.PlaySound(Assets.Sounds.SoundDoor);
            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            if (!player.IsWithinSnappngRangeToTile(i, j, 5 * 16))
                return;

            var coords = TileObjectData.TopLeft(i, j);
            var topLeftTile = Main.tile[coords.X, coords.Y];
            // Going up
            int itemType = ModContent.ItemType<Items.Placeable.Furniture.ElevatorUp>();

            if (j >= coords.Y + 2)
                itemType = ModContent.ItemType<Items.Placeable.Furniture.ElevatorDown>();

            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = itemType;
        }
    }
}
