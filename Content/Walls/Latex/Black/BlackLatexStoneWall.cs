using Terraria.ID;

namespace ChangedSpecialMod.Content.Walls.Latex.Black
{
    public class BlackLatexStoneWall : BaseBlackLatexWall
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            VanillaFallbackOnModDeletion = WallID.Stone;
        }
    }
}
