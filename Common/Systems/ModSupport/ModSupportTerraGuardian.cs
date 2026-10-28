using ChangedSpecialMod.Content.NPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems.ModSupport
{
    public class TerraGuardianCompanion
    {
        public string name;
        public string nickname;
        public float scale;
        public bool male;
        public bool inPlayerParty;  // If they are currently following the player
        public bool hasBeenMet;     // If they have moved in, aka are listed in the housing menu
        public bool IsFurry()
        {
            var nonFurryList = new List<string>()
            {
                "michelle",
                "nemesis"
            };

            return !nonFurryList.Contains(name.ToLower());
        }

        public bool IsSmall()
        {
            return scale < 0.8f;
        }

        public bool IsNormalSized()
        {
            return scale >= 0.8f;
        }

        public bool IsMacro()
        {
            return scale >= 3;
        }
    }

    public class ModSupportTerraGuardian : ModSupportData
    {
        public ModSupportTerraGuardian(Mod mod, string modName) : base(mod, modName) { }

        public List<TerraGuardianCompanion> GetAllTerraGuardians()
        {
            var companionsInParty = GetCompanionsInPlayerParty();
            var otherCompanions = GetTerraGuardianCompanions();

            var allCompanions = companionsInParty;
            foreach (var companion in otherCompanions)
            {
                if (!allCompanions.Any(x => x.name == companion.name))
                    allCompanions.Add(companion);
            }

            return allCompanions;
        }

        public List<TerraGuardianCompanion> GetTerraGuardianCompanions()
        {
            var result = new List<TerraGuardianCompanion>();

            if (mod == null)
                return result;

            try
            {
                Type worldModType = mod.Code.GetType("terraguardians.WorldMod");

                FieldInfo companionNPCsfield = worldModType.GetField(
                    "CompanionNPCs",
                    BindingFlags.Public | BindingFlags.Static
                );

                if (companionNPCsfield != null)
                {
                    var companions = companionNPCsfield.GetValue(null) as System.Collections.IList;
                    var companionNamesInParty = GetCompanionNamesInPlayerParty();

                    if (companions == null)
                        return result;

                    for (int i = 0; i < companions.Count; i++)
                    {
                        var companion = companions[i];
                        if (companion != null)
                        {
                            var hasBeenMetProperty = companion.GetType().GetProperty("HasBeenMet");
                            bool hasBeenMet = (bool)(hasBeenMetProperty?.GetValue(companion) ?? false);

                            var nameProperty = companion.GetType().GetProperty("GetRealName");
                            if (nameProperty != null)
                            {
                                string name = nameProperty.GetValue(companion) as string;
                                if (name != null)
                                {
                                    var nickname = name;
                                    var companionPlayer = companion as Player;

                                    if (companionPlayer != null)
                                        nickname = companionPlayer.name;

                                    var scaleField = companion.GetType().GetField("Scale");
                                    float scale = (float)(scaleField?.GetValue(companion) ?? 1f);

                                    int gender = 0;
                                    var genderProperty = companion.GetType().GetProperty("Genders");
                                    if (genderProperty != null)
                                    {
                                        object genderObject = genderProperty.GetValue(companion);
                                        gender = Convert.ToInt32(genderObject);
                                    }

                                    var companionObj = new TerraGuardianCompanion()
                                    {
                                        name = name,
                                        nickname = nickname,
                                        scale = scale,
                                        male = gender == 0,
                                        hasBeenMet = hasBeenMet,
                                        inPlayerParty = false
                                    };

                                    result.Add(companionObj);
                                }
                            }
                        }
                    }
                }

                return result;
            }
            catch
            {
                return result;
            }
        }

        public List<TerraGuardianCompanion> GetCompanionsInPlayerParty()
        {
            var result = new List<TerraGuardianCompanion>();

            if (mod == null)
                return result;

            // NPC chats are only done locally, so this is okay to do
            var player = Main.LocalPlayer;

            try
            {
                Type playerModType = mod.Code.GetType("terraguardians.PlayerMod");

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
                    BindingFlags.Instance |
                    BindingFlags.NonPublic
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
                        var hasBeenMetProperty = companion.GetType().GetProperty("HasBeenMet");
                        bool hasBeenMet = (bool)(hasBeenMetProperty?.GetValue(companion) ?? false);

                        var nameProperty = companion.GetType().GetProperty("GetRealName");
                        if (nameProperty != null)
                        {
                            string name = nameProperty.GetValue(companion) as string;

                            if (name != null)
                            {
                                var nickname = name;
                                var companionPlayer = companion as Player;

                                if (companionPlayer != null)
                                    nickname = companionPlayer.name;

                                var scaleField = companion.GetType().GetField("Scale");
                                float scale = (float)(scaleField?.GetValue(companion) ?? 1f);

                                int gender = 0;
                                var genderProperty = companion.GetType().GetProperty("Genders");
                                if (genderProperty != null)
                                {
                                    object genderObject = genderProperty.GetValue(companion);
                                    gender = Convert.ToInt32(genderObject);
                                }

                                var companionObj = new TerraGuardianCompanion()
                                {
                                    name = name,
                                    nickname = nickname,
                                    scale = scale,
                                    male = gender == 0,
                                    hasBeenMet = hasBeenMet,
                                    inPlayerParty = true
                                };

                                result.Add(companionObj);
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public List<string> GetCompanionNamesInPlayerParty()
        {
            var result = new List<string>();

            if (mod == null)
                return result;

            // NPC chats are only done locally, so this is okay to do
            var player = Main.LocalPlayer;

            try
            {
                Type playerModType = mod.Code.GetType("terraguardians.PlayerMod");

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
                    BindingFlags.Instance |
                    BindingFlags.NonPublic
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
                        var nameProperty = companion.GetType().GetProperty("GetRealName");

                        if (nameProperty != null)
                        {
                            string name = nameProperty.GetValue(companion) as string;
                            if (name != null)
                            {
                                result.Add(name);
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        private Dictionary<string, string> GetTerraGuardianKeywords()
        {
            // Checks terraguardians modplayer (PlayerMod) if he has any companions
            // Companion is an extended player class and can be used to retrieve
            // info like the name, nickname and scale.
            // The names can be used in chats and scale can be used to determine if they are big
            // Terraguardian doesnt check the scale as far as I know
            // For example, Leona can call the player "shortie" even if you shrunk her
            // It could also be intentional, because normally she is much larger than the player
            // The companions are stored int the SummonedCompanions array

            var result = new Dictionary<string, string>();  

            if (mod == null)
                return result;

            // NPC chats are only done locally, so this is okay to do
            var player = Main.LocalPlayer;

            try
            {
                Type playerModType = mod.Code.GetType("terraguardians.PlayerMod");

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
                        var nameProperty = companion.GetType().GetProperty("GetRealName");
                        var scaleField = companion.GetType().GetField("Scale");

                        if (nameProperty != null)
                        {
                            string name = nameProperty.GetValue(companion) as string;
                            if (name != null)
                            {
                                var nickname = name;
                                var companionPlayer = companion as Player;

                                if (companionPlayer != null)
                                {
                                    nickname = companionPlayer.name;
                                }


                                if (scaleField != null)
                                {
                                    object scaleObject = scaleField.GetValue(companion);
                                    if (scaleObject != null)
                                    {
                                        var scale = (float)scaleObject;
                                        name += $", scale {scale}";
                                    }
                                }
                                if (!result.ContainsKey(name))
                                {
                                    result.Add(name, nickname);
                                }
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }
    }
}
