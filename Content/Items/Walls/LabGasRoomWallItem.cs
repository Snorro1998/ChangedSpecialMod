using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Walls
{
    public class LabGasRoomWallItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<Content.Walls.LabGasRoomWall>());
        }
    }
}
