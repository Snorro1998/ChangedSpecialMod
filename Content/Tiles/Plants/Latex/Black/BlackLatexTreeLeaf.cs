using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.Plants.Latex.Black
{
    public class BlackLatexTreeLeaf : ModGore
    {
        public override void SetStaticDefaults()
        {
            ChildSafety.SafeGore[Type] = true;
        }

        public override void OnSpawn(Gore gore, IEntitySource source)
        {
            base.OnSpawn(gore, source);
            var direction = gore.velocity.SafeNormalize(Vector2.Zero);
            gore.velocity = direction * Main.rand.NextFloat(0.1f, 1f);
        }
    }
}
