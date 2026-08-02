using ChangedSpecialMod.Content.Items.Placeable.Latex.Black;
using ChangedSpecialMod.Content.Projectiles.Latex;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Latex.White
{
    public class WhiteLatexSand : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
            ItemID.Sets.SandgunAmmoProjectileData[Type] = new(ModContent.ProjectileType<WhiteLatexSandBallGunProjectile>(), 10);
            Item.value = Item.buyPrice(0, 0, 0, 5);
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<BlackLatexSand>();
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Latex.White.WhiteLatexSandTile>());
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

        public override void AddRecipes()
        {
            CreateRecipe(10)
                .AddIngredient(ModContent.ItemType<WhiteGoo>(), 1)
                .AddIngredient(ItemID.SandBlock, 10)
                .AddTile(TileID.Solidifier)
                .Register();
        }
    }
}
