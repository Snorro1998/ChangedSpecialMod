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
            AddItemFruit("Barberry", ItemSellCondition.HardMode);
            AddItemFruit("Cometfruit", ItemSellCondition.HardMode);
            AddItemFruit("Lotus", ItemSellCondition.HardMode);
            AddItemFruit("Mangosteen", ItemSellCondition.HardMode);
        }

        public override void AddItemsSeed()
        {
            AddItemSeed("CinderBlossomSeeds", ItemSellCondition.HardMode);
            AddItemSeed("AstralGrassSeeds", ItemSellCondition.HardMode);
        }
    }
}
