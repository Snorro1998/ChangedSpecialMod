using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
	public class Elevator : ModTile
	{
        public const int ElevatorReach = 20;

		public override void SetStaticDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = false;
			TileID.Sets.FramesOnKillWall[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.AnchorWall = true;
            TileObjectData.addTile(Type);
			DustType = DustID.WoodFurniture;
		}

        public override bool RightClick(int i, int j)
        {
            // Disable usage if you are fighting the wolf king or behemoth and play a buzzer sound
            if (ChangedUtils.HasWolfKingFightStarted()/* || NPC.AnyNPCs(ModContent.NPCType<Behemoth>())*/)
            {
                SoundEngine.PlaySound(Assets.Sounds.SoundBuzzer1);
                return true;
            }

            Player player = Main.LocalPlayer;
            var coords = TileObjectData.TopLeft(i, j);
            var topLeftTile = Main.tile[coords.X, coords.Y];
            // Going up
            int direction = -1;

            if (j >= coords.Y + 2)
                direction = 1;

            for (int y = 0; y < ElevatorReach; y++)
            {
                var yPos = coords.Y + direction * y;
                var topLeft = TileObjectData.TopLeft(i, yPos);
                // The tile is a different elevator and is at the same x position
                if (Main.tile[i, yPos].TileType == ModContent.TileType<Elevator>() && topLeft != Point16.NegativeOne && topLeft.X == coords.X && topLeft.Y != coords.Y)
                {
                    SoundEngine.PlaySound(Assets.Sounds.SoundElevator);
                    player.RemoveAllGrapplingHooks();
                    player.position = new Vector2((topLeft.X + 1) * 16, topLeft.Y * 16);
                    return true;
                }
            }

            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
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
