using ChangedSpecialMod.Content.Achievements;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static ChangedSpecialMod.ChangedSpecialMod;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
	public class Generator : ModTile
	{
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 4;
            TileObjectData.addTile(Type);
            DustType = DustID.WoodFurniture;

            AnimationFrameHeight = 72;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter >= 10)
            {
                frameCounter = 0;
                frame = (frame + 1) % 3;
            }
        }

        public override bool RightClick(int i, int j)
        {
            Main.NewText(Language.GetTextValue("Mods.ChangedSpecialMod.Messages.TogglePower"));
            SoundEngine.PlaySound(Assets.Sounds.SoundBuzzer2);
            return true;
        }

        public override void HitWire(int i, int j)
        {
            var coords = TileObjectData.TopLeft(i, j);
            var topLeftTile = Main.tile[coords.X, coords.Y];
            var tileFrameX = topLeftTile.TileFrameX == 0 ? 72 : 0;

            bool toggledOn = tileFrameX != 0;

            if (Main.netMode == NetmodeID.SinglePlayer)
                SoundEngine.PlaySound(toggledOn ? Assets.Sounds.SoundChime2 : Assets.Sounds.SoundBuzzer2);

            else if (Main.netMode == NetmodeID.Server)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)MessageType.PlaySwitchSound);
                packet.Write((short)i);
                packet.Write((short)j);
                packet.Write(toggledOn);
                packet.Send();
            }

            ModContent.GetInstance<AATurnPowerOnAchievement>().ConditionTurnPowerOn.Complete();
            bool wireSkip = Wiring.running;

            for (int y = 0; y < 4; y++)
            {
                var yPos = coords.Y + y;
                for (int x = 0; x < 4; x++)
                {
                    var xPos = coords.X + x;
                    var tile = Main.tile[xPos, yPos];
                    tile.TileFrameX = (short)(tileFrameX + x * 18);
                    if (wireSkip) Wiring.SkipWire(xPos, yPos);
                }
            }

            if (Main.netMode != NetmodeID.SinglePlayer)
                NetMessage.SendTileSquare(-1, coords.X, coords.Y, 4, TileChangeType.None);
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<Items.Placeable.Furniture.Generator>();
        }

        // Ignore style nonsense and always drop the correct item
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<Items.Placeable.Furniture.Generator>());
        }
    }
}
