using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
    public class SweeperPuroCage : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.CritterCageLidStyle[Type] = TileID.Sets.CritterCageLidStyle[TileID.SquirrelCage];
            Main.tileFrameImportant[Type] = Main.tileFrameImportant[TileID.SquirrelCage];
            Main.tileLavaDeath[Type] = Main.tileLavaDeath[TileID.SquirrelCage];
            Main.tileSolidTop[Type] = Main.tileSolidTop[TileID.SquirrelCage];
            Main.tileTable[Type] = Main.tileTable[TileID.SquirrelCage];
            AnimationFrameHeight = 54;

            TileObjectData.newTile.CopyFrom(TileObjectData.GetTileData(TileID.SquirrelCage, 0));
            TileObjectData.addTile(Type);
            DustType = DustID.Glass;

            // Since this tile is only used for a single item, we can reuse the item localization for the map entry.
            //AddMapEntry(new Color(122, 217, 232), ModContent.GetInstance<ExampleCritterCageItem>().DisplayName);
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            offsetY = 2; // From vanilla
            //Main.critterCage = true; // Vanilla doesn't run the animation code for critters unless this is checked
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            Tile tile = Main.tile[i, j];
            var coords = TileObjectData.TopLeft(i, j);
            var topLeftTile = Main.tile[coords.X, coords.Y];

            var index = coords.X + coords.Y;
            var animationObject = ModContent.GetInstance<TileAnimationSystem>().sweeperPuroCageAnimation;
            var frameIndex = animationObject.GetFrame(index % animationObject.nVariations);

            frameYOffset = frameIndex * AnimationFrameHeight;
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            if (!player.IsWithinSnappngRangeToTile(i, j, 5 * 16))
                return false;

            SoundEngine.PlaySound(Assets.Sounds.SoundPlush);
            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            if (!player.IsWithinSnappngRangeToTile(i, j, 5 * 16))
                return;

            int itemType = ModContent.ItemType<Items.Placeable.Furniture.PuroPlush>();

            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = itemType;
        }
    }
}