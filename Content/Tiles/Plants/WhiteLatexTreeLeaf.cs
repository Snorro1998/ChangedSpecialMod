using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.Plants
{
    public class WhiteLatexTreeLeaf : ModGore
    {
        public override string Texture => "ChangedSpecialMod/Content/Tiles/Plants/WhiteLatexTreeLeaf";

        public override void SetStaticDefaults()
        {
            ChildSafety.SafeGore[Type] = true;
            //GoreID.Sets.LiquidDroplet[Type] = true;
            //GoreID.Sets.SpecialAI[Type] = 1;

            // Rather than copy in all the droplet specific gore logic, this gore will pretend to be another gore to inherit that logic.
            //UpdateType = GoreID.WaterDrip;
            //ChildSafety.SafeGore[Type] = true; // Leaf gore should appear regardless of the "Blood and Gore" setting
            //GoreID.Sets.SpecialAI[Type] = 1; // Falling leaf behavior
            //GoreID.Sets.PaintedFallingLeaf[Type] = true; // This is used for all vanilla tree leaves, related to the bigger spritesheet for tile paints
        }

        public override void OnSpawn(Gore gore, IEntitySource source)
        {
            base.OnSpawn(gore, source);
            var direction = gore.velocity.SafeNormalize(Vector2.Zero);
            gore.velocity = direction * Main.rand.NextFloat(0.1f, 1f);
        }
    }
}
