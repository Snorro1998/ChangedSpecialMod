using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace ChangedSpecialMod.Common.Systems.ModSupport
{
    public class ModSupportAncientsAwakened : ModSupportData
    {
        public ModSupportAncientsAwakened(Mod mod, string modName) : base(mod, modName) { }

        public override void Init()
        {
            VanillaPassAfterBiomeGeneration = Utilities.WorldPass.WaterPlants;
        }

        public override void AddTownNPCs()
        {
            //AddTownNPC("Legendscribe", "AncientsAwakenedAnubis");
            AddTownNPC("LegendscribeUnofficial", "AncientsAwakenedAnubis");
        }

        public override void AddTiles()
        {
            // Mire
            AddTileType("AbyssWood_Tile", true);
            AddTileType("AbyssWoodSolid_Tile", true);
            AddTileType("AbyssLeaves_Tile", true);
            AddTileType("Depthstone_Tile", true);
            AddTileType("Darkmud_Tile", true);
            AddTileType("MireGrass_Tile", true);

            // Inferno
            AddTileType("InfernoGrass_Tile", true);
            AddTileType("Torchstone_Tile", true);
            AddTileType("TorchAsh_Tile", true);
            AddTileType("Torchice_Tile", true);

            AddTileType("ScorchedDynastyWoodUnsafe_Tile", true);
            AddTileType("ScorchedPlatform_Tile", true);
            AddTileType("ScorchedShingles_Tile", true);
        }
    }
}
