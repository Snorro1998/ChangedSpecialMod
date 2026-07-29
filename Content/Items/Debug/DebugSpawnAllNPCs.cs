using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Utilities;
using Terraria;
using Terraria.GameContent.Skies;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Debug
{
    public class DebugSpawnAllNPCs : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return true;
            /*
            for (int i = 0; i < 10; i++)
            {
                var entityType = Main.rand.Next(0, 18);
                entityType = ChangedUtils.Choose(
                    //(int)Terraria.GameContent.Ambience.SkyEntityType.AirBalloon,
                    (int)Terraria.GameContent.Ambience.SkyEntityType.BirdsV,
                    (int)Terraria.GameContent.Ambience.SkyEntityType.Airship,
                    (int)Terraria.GameContent.Ambience.SkyEntityType.SlimeBalloons
                    );
                var seed = Main.rand.Next();
                //Terraria.GameContent.Ambience.SkyEntityType.BirdsV

                ((AmbientSky)SkyManager.Instance["Ambience"]).Spawn(Main.LocalPlayer, (Terraria.GameContent.Ambience.SkyEntityType)entityType, seed);
            }

            return true;
            */

            /*
            Main.NewText($"nNormal: {CityRuinsBiomeTileCount.BlackLatexBlockCount} nDesert: {CityRuinsBiomeTileCount.BlackLatexDesertBlockCount} nJungle nSnow: {CityRuinsBiomeTileCount.BlackLatexSnowBlockCount}");

            if (CityRuinsBiomeTileCount.ActiveBiomeType == CityRuinsBiomeTileCount.BiomeType.Normal)
                Main.NewText("Normal");

            if (CityRuinsBiomeTileCount.ActiveBiomeType == CityRuinsBiomeTileCount.BiomeType.Desert)
                Main.NewText("Desert");

            if (CityRuinsBiomeTileCount.ActiveBiomeType == CityRuinsBiomeTileCount.BiomeType.Jungle)
                Main.NewText("Jungle");

            if (CityRuinsBiomeTileCount.ActiveBiomeType == CityRuinsBiomeTileCount.BiomeType.Snow)
                Main.NewText("Snow");

            return false;
            */

            /*
            if (ModSupportSystem.modMrPlagueRaces != null)
            {
                //var cont = ModSupportSystem.modMrPlagueRaces.GetContent<ModSystem>().ToList();
                ModSystem system = ModSupportSystem.modMrPlagueRaces.GetContent<ModSystem>().FirstOrDefault(s => s.GetType().Name == "RaceChangeUISystem");

                if (system != null)
                {
                    MethodInfo showMethod = system.GetType().GetMethod(
                        "ShowMyUI",
                        BindingFlags.Instance | BindingFlags.Public);

                    showMethod?.Invoke(system, null);
                }

                return true;
            }
            */

            ChangedUtils.SpawnAllNPCs(player);

            return true;
        }
    }
}