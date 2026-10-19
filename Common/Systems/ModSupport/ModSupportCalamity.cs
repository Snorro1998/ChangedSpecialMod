using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems.ModSupport
{
    public class ModSupportCalamity : ModSupportData
    {
        public ModSupportCalamity(Mod mod, string modName) : base(mod, modName) { }

        public override void AddTownNPCs()
        {
            AddTownNPC("DILF", "CalamityArchmage");
            AddTownNPC("THIEF", "CalamityBandit");
            AddTownNPC("SEAHOE", "CalamitySeaKing");
            AddTownNPC("WITCH", "CalamityBrimstoneWitch");
        }

        public override void AddTiles()
        {
            // Draedon Lab
            AddTileType("LaboratoryPlating", true);

            // Sunken sea
            AddTileType("EutrophicSand", true);
            AddTileType("Navystone", true);

            // Sulphuric sea
            AddTileType("SulphurousSand", true);
            AddTileType("SulphurousSandstone", true);
            AddTileType("SulphurousShale", true);
            AddTileType("HardenedSulphurousSandstone", true);

            // Abyss
            AddTileType("AbyssGravel", true);
            AddTileType("PyreMantle", true);
            AddTileType("Voidstone", true);
        }

        public override void AddItemsFruit()
        {
            AddItemFruit("Jackfruit");
            AddItemFruit("Salak");
            AddItemFruit("Barberry");   // Main.hardMode
            AddItemFruit("Cometfruit");   // Main.hardMode
            AddItemFruit("Lotus");   // Main.hardMode
            AddItemFruit("Mangosteen");   // Main.hardMode
        }

        public override void AddItemsSeed()
        {
            AddItemSeed("CinderBlossomSeeds"); // Main.hardMode
            AddItemSeed("AstralGrassSeeds"); // Main.hardMode
        }
    }
}
