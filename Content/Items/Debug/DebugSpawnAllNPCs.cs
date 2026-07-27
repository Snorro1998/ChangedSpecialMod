using ChangedSpecialMod.Utilities;
using Terraria;
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