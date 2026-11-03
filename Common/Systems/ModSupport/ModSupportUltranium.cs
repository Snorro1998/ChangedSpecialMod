using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems.ModSupport
{
    public class ModSupportUltranium : ModSupportData
    {
        public ModSupportUltranium(Mod mod, string modName) : base(mod, modName) { }

        public override void Init()
        {
            // Micro Biomes
            // -> Depths
            // Water Plants
            VanillaPassAfterBiomeGeneration = Utilities.WorldPass.WaterPlants;
        }

        public override void AddTiles()
        {
            AddTileType("ShadowGrass", true);
            AddTileType("ShadowStoneTile", true);
            AddTileType("DarkStone", true);
            AddTileType("AbyssRock", true);
            AddTileType("PurpleShadowGrass", true);
        }
    }
}
