using Terraria.ID;

namespace ChangedSpecialMod.Content.Walls.Latex
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
