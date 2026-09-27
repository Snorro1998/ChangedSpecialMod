using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Debug
{
    public class DebugItem : ModItem
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

        public bool HasGuardian(Player player)
        {
            if (ModSupportSystem.modTerraGuardians == null)
                return false;

            try
            {
                Type playerModType = ModSupportSystem.modTerraGuardians.Code.GetType("terraguardians.PlayerMod");

                if (playerModType == null)
                    return false;

                // Find TerraGuardians' PlayerMod attached to this player.
                ModPlayer modPlayer = null;

                foreach (ModPlayer mp in player.ModPlayers)
                {
                    if (mp.GetType() == playerModType)
                    {
                        modPlayer = mp;
                        break;
                    }
                }

                if (modPlayer == null)
                    return false;

                // Get the private SummonedCompanions field.
                var field = playerModType.GetField(
                    "SummonedCompanions",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic
                );

                if (field == null)
                    return false;

                Array companions = field.GetValue(modPlayer) as Array;

                if (companions == null)
                    return false;

                // Check whether any companion is actually present.
                for (int i = 0; i < companions.Length; i++)
                {
                    if (companions.GetValue(i) != null)
                        return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                //ModContent.GetInstance<YourMod>().Logger.Warn(
                //    $"Failed to check TerraGuardians: {ex}"
                //);

                return false;
            }
        }

        public List<string> GetCompanionNames(Player player)
        {
            var result = new List<string>();

            if (ModSupportSystem.modTerraGuardians == null)
                return result;

            try
            {
                Type playerModType = ModSupportSystem.modTerraGuardians.Code.GetType("terraguardians.PlayerMod");

                if (playerModType == null)
                    return result;

                // Find TerraGuardians' PlayerMod attached to this player.
                ModPlayer modPlayer = null;

                foreach (ModPlayer mp in player.ModPlayers)
                {
                    if (mp.GetType() == playerModType)
                    {
                        modPlayer = mp;
                        break;
                    }
                }

                if (modPlayer == null)
                    return result;

                // Get the private SummonedCompanions field.
                var field = playerModType.GetField(
                    "SummonedCompanions",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic
                );

                if (field == null)
                    return result;

                Array companions = field.GetValue(modPlayer) as Array;

                if (companions == null)
                    return result;

                // Check whether any companion is actually present.
                for (int i = 0; i < companions.Length; i++)
                {
                    var companion = companions.GetValue(i);
                    if (companion != null)
                    {
                        var nameField = playerModType.GetField(
                            "SummonedCompanions",
                            System.Reflection.BindingFlags.Instance |
                            System.Reflection.BindingFlags.NonPublic
                        );

                        var nameProperty = companion.GetType().GetProperty("GetRealName");
                        var scaleField = companion.GetType().GetField("Scale");

                        if (nameProperty != null)
                        {
                            string name = nameProperty.GetValue(companion) as string;
                            if (name != null)
                            {
                                if (scaleField != null)
                                {
                                    object scaleObject = scaleField.GetValue(companion);
                                    if (scaleObject != null)
                                    {
                                        var scale = (float)scaleObject;
                                        name += $", scale {scale}";
                                    }
                                }
                                result.Add(name);
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                //ModContent.GetInstance<YourMod>().Logger.Warn(
                //    $"Failed to check TerraGuardians: {ex}"
                //);

                return result;
            }
        }

        public override bool? UseItem(Player player)
        {
            if (ModSupportSystem.modTerraGuardians != null)
            {
                try
                {
                    var companionNames = GetCompanionNames(player);
                    if (companionNames.Count > 0)
                    {
                        foreach (var n in companionNames)
                        {
                            Main.NewText(n);
                        }
                    }
                    /*
                    var hasGuardian = HasGuardian(player);

                    if (hasGuardian)
                    {
                        Main.NewText($"{player.name}, hasGuardian");
                        
                    }
                    */
                }
                catch
                {

                }

                /*
                {


                    FieldInfo boolField = myWorldType?.GetField(
                        fieldName,
                        BindingFlags.Public | BindingFlags.Static
                    );

                    if (boolField != null)
                        boolField.SetValue(null, newValue);
                }

        var str = "";
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    var tmpPlayer = Main.player[i];
                    if (tmpPlayer == null)
                        continue;

                    

                    var IsCompanion = (bool)ModSupportSystem.modTerraGuardians.Call("IsCompanion", tmpPlayer);
                    if (IsCompanion)
                    {
                        player.TryGetModPlayer
                    }

                    str += $"{tmpPlayer.name} {IsCompanion}";
                    //var playerName = player.name != null ? player.name : "none";

                }

                Main.NewText(str);
                */
            }

            return true;
        }
    }
}