using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems.ModSupport
{
    public class ModSupportSpooky : ModSupportData
    {
        public ModSupportSpooky(Mod mod, string modName) : base(mod, modName) { }

        public override void AddTiles()
        {
            // Spooky forest
            AddTileType("SpookyGrass", true);
            AddTileType("SpookyGrassGreen", true);
            AddTileType("GourdBlockOrange", true);
            AddTileType("GourdBlockLime", true);
            AddTileType("GourdBlockYellow", true);
            AddTileType("MushroomMoss", true);
            
            // Cemetery
            AddTileType("SpookyDirt", true);
            AddTileType("SpookyDirt2", true);
            AddTileType("BloomSoil", true);
            AddTileType("SpookyStone", true);
            AddTileType("CemeteryGrass", true);
            AddTileType("CemeteryDirt", true);
            AddTileType("CemeteryStone", true);
            AddTileType("SpookyWood", true);

            // spider
            AddTileType("DampSoil", true);
            AddTileType("DampGrass", true);
            AddTileType("DampStone", true);
            AddTileType("DampMushroomGrass", true);
            AddTileType("WebBlock", true);
            AddTileType("OldHunterBrick", true);

            // Catacomb
            AddTileType("CatacombBrick1", true);
            AddTileType("CatacombBrick2", true);
            AddTileType("CatacombBrick2Arena", true);
            AddTileType("CatacombFlooring", true);
            AddTileType("GildedBrick", true);

            // Hell biome
            AddTileType("SpookyMush", true);
            AddTileType("SpookyMushGrass", true);
            AddTileType("EyeBlock", true);
            AddTileType("LivingFlesh", true);
            AddTileType("ValleyStone", true);

            // Building in hell
            AddTileType("NoseTempleBrickPurple", true);
            AddTileType("NoseTempleFancyBrickRed", true);

            // Thing under the ocean
            AddTileType("OceanRock", true);
            AddTileType("OceanSand", true);
            AddTileType("OceanBiomass", true);
            AddTileType("LabMetalPlate", true);

            // Santa Dungeon (snow)
            AddTileType("ChristmasBrickRed", true);
            AddTileType("ChristmasSlabBlue", true);
            AddTileType("ChristmasWood", true);
            AddTileType("ChristmasCarpet", true);

            // Vegetable garden (jungle)
            AddTileType("JungleSoil", true);
            AddTileType("JungleMoss", true);

            // tarpit (desert)
            AddTileType("DesertSand", true);
            AddTileType("DesertSandstone", true);
        }
    }
}
