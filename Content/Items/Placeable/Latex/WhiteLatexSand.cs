using ChangedSpecialMod.Content.Projectiles.Latex;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Latex
{
    public class WhiteLatexSand : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
            ItemID.Sets.SandgunAmmoProjectileData[Type] = new(ModContent.ProjectileType<WhiteLatexSandBallGunProjectile>(), 10);
            Item.value = Item.buyPrice(0, 0, 0, 5);
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Latex.WhiteLatexSandTile>());
            Item.width = 12;
            Item.height = 12;
            Item.ammo = AmmoID.Sand;
            Item.notAmmo = true;
        }

        public override void ExtractinatorUse(int extractinatorBlockType, ref int resultType, ref int resultStack)
        {
            resultType = ItemID.SandBlock;
            resultStack = 1;
        }
    }
}
