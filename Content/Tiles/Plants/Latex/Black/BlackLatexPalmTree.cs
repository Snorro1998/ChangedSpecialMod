using ChangedSpecialMod.Content.Items.Food;
using ChangedSpecialMod.Content.Items.Placeable.Latex.Black;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Content.Projectiles;
using ChangedSpecialMod.Content.Tiles.Latex.Black;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace ChangedSpecialMod.Content.Tiles.Plants.Latex.Black
{
    public class BlackLatexPalmTree : ModPalmTree
    {
        private Asset<Texture2D> texture;
        private Asset<Texture2D> topsTexture;
        private Asset<Texture2D> oasisTopsTexture;

        public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings
        {
            UseSpecialGroups = true,
            SpecialGroupMinimalHueValue = 11f / 72f,
            SpecialGroupMaximumHueValue = 0.25f,
            SpecialGroupMinimumSaturationValue = 0.88f,
            SpecialGroupMaximumSaturationValue = 1f
        };

        public override void SetStaticDefaults()
        {
            GrowsOnTileId = [ModContent.TileType<BlackLatexSandTile>()];
            texture = ModContent.Request<Texture2D>("ChangedSpecialMod/Content/Tiles/Plants/Latex/Black/BlackLatexPalmTree");
            topsTexture = ModContent.Request<Texture2D>("ChangedSpecialMod/Content/Tiles/Plants/Latex/Black/BlackLatexTree_Tops");
            oasisTopsTexture = ModContent.Request<Texture2D>("ChangedSpecialMod/Content/Tiles/Plants/Latex/Black/BlackLatexPalmTree_Tops");
        }

        // This is the primary texture for the trunk. Branches and foliage use different settings.
        public override Asset<Texture2D> GetTexture() => texture;

        // Top Textures
        public override Asset<Texture2D> GetTopTextures() => topsTexture;

        public override Asset<Texture2D> GetOasisTopTextures() => oasisTopsTexture;

        public override int SaplingGrowthType(ref int style)
        {
            style = 0;
            return ModContent.TileType<BlackLatexPalmTreeSapling>();
        }

        private void SpawnGasTank(IEntitySource source, Player player, int x, int y)
        {
            var projectileType = ChangedUtils.Choose(ModContent.ProjectileType<RedFlyingGasTank>(), ModContent.ProjectileType<BlueFlyingGasTank>());
            Projectile.NewProjectile(source, x * 16, y * 16, 0, 0, projectileType, 0, 0, player.whoAmI, 0f, 0f);
        }

        private void DropItem(IEntitySource source, Player player, int x, int y)
        {
            var items = new WeightedRandom<int>();
            var amount = 1;
            items.Add(ItemID.Wood, 2);
            items.Add(ItemID.Acorn, 2);
            var blockItemType = ModContent.ItemType<BlackLatexSand>();
            items.Add(ModContent.ItemType<BlackLatexSand>(), 1);
            items.Add(ModContent.ItemType<Orange>(), 1);
            var itemId = items.Get();

            if (itemId == blockItemType)
                amount = Main.rand.Next(15, 31);
            else if (itemId == ItemID.Wood)
                amount = Main.rand.Next(4, 10);
            else if (itemId == ItemID.Acorn)
                amount = Main.rand.Next(1, 3);

            Item.NewItem(source, new Vector2(x, y) * 16, itemId, amount);
        }

        private void SpawnLatex(IEntitySource source, Player player, int x, int y, bool isDrunk)
        {
            var xPos = x * 16;
            var yPos = y * 16;
            var npcType = !isDrunk ? ChangedUtils.Choose(ModContent.NPCType<BlackGoop>()) : ChangedUtils.Choose(ModContent.NPCType<QuackLatex>());
            NPC.NewNPCDirect(source, xPos, yPos, npcType, player.whoAmI);
        }

        public override int DropWood()
        {
            return ItemID.Wood;
        }

        public override bool Shake(int x, int y, ref bool createLeaves)
        {
            var isDrunk = false;
            var player = ChangedUtils.GetClosestPlayer(x, y);
            if (player != null && ChangedUtils.IsDrunk(player))
                isDrunk = true;

            var action = Main.rand.Next(0, 3);
            var source = WorldGen.GetItemSource_FromTileBreak(x, y);
            switch (action)
            {
                // Do nothing
                default:
                    if (isDrunk)
                        SpawnGasTank(source, player, x, y);
                    break;
                // Drop item
                case 1:
                    if (isDrunk)
                        ChangedUtils.SpawnOranges(source, player, x, y);
                    DropItem(source, player, x, y);
                    break;
                // Spawn monster
                case 2:
                    SpawnLatex(source, player, x, y, isDrunk);
                    break;
            }

            return false;
        }

        public override int TreeLeaf()
        {
            return ModContent.GoreType<BlackLatexTreeLeaf>();
        }
    }
}
