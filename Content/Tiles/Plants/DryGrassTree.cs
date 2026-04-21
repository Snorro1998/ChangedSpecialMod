using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Content.Projectiles;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace ChangedSpecialMod.Content.Tiles.Plants
{
    public class DryGrassTree : ModTree
    {
        private Asset<Texture2D> texture;
        private Asset<Texture2D> branchesTexture;
        private Asset<Texture2D> topsTexture;

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
            GrowsOnTileId = [ModContent.TileType<DryDirt>()];
            texture = ModContent.Request<Texture2D>("ChangedSpecialMod/Content/Tiles/Plants/DryGrassTree");
            branchesTexture = ModContent.Request<Texture2D>("ChangedSpecialMod/Content/Tiles/Plants/DryGrassTree_Branches");
            topsTexture = ModContent.Request<Texture2D>("ChangedSpecialMod/Content/Tiles/Plants/DryGrassTree_Tops");
        }

        // This is the primary texture for the trunk. Branches and foliage use different settings.
        public override Asset<Texture2D> GetTexture()
        {
            return texture;
        }

        public override int SaplingGrowthType(ref int style)
        {
            style = 0;
            return ModContent.TileType<DryGrassTreeSapling>();
        }

        // Branch Textures
        public override Asset<Texture2D> GetBranchTextures() => branchesTexture;

        // Top Textures
        public override Asset<Texture2D> GetTopTextures() => topsTexture;


        private void SpawnOranges(IEntitySource source, Player player, int x, int y)
        {
            for (int i = 0; i < 30; i++)
            {
                var index = Projectile.NewProjectile(source, x * 16, y * 16, 0, 0, ModContent.ProjectileType<OrangeProjectile>(), 0, 0, player.whoAmI, 0f, 0f);
                if (index >= 0 && index < Main.projectile.Length)
                {
                    var angle = Main.rand.NextFloat((float)Math.PI * 2);
                    Main.projectile[index].velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 5;
                }
            }
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
            var blackBlockID = ModContent.ItemType<Items.Placeable.BlackLatexBlock>();
            items.Add(ModContent.ItemType<Items.Placeable.BlackLatexBlock>(), 1);
            var whiteBlockID = ModContent.ItemType<Items.Placeable.WhiteLatexBlock>();
            items.Add(ModContent.ItemType<Items.Placeable.WhiteLatexBlock>(), 1);
            items.Add(ModContent.ItemType<Items.Orange>(), 1);
            var itemId = items.Get();

            if (itemId == blackBlockID || itemId == whiteBlockID)
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
            var npcType = !isDrunk ? ChangedUtils.Choose(ModContent.NPCType<BlackGoop>(), ModContent.NPCType<WhiteGoop>(), ModContent.NPCType<Raccoon>()) :
                ChangedUtils.Choose(ModContent.NPCType<QuackLatex>(), ModContent.NPCType<FlightLatex>());
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
                        SpawnOranges(source, player, x, y);
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
            return ModContent.GoreType<DryGrassTreeLeaf>();
        }
    }
}
