using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
    public class FaultyLightTile : ModTile
    {
        public override void SetStaticDefaults() => this.SetUpLantern(ModContent.ItemType<Items.Placeable.Furniture.FaultyLight>(), true);

        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, DustID.Stone, 0f, 0f, 1, new Color(100, 130, 150), 1f);
            return false;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].TileFrameX < 18)
            {
                r = 1f;
                g = 1f; //.2
                b = 1f; //.2
            }
            else
            {
                r = 0f;
                g = 0f;
                b = 0f;
            }
        }
    }
}
