using System.Collections.Generic;
using Terraria.GameContent.Generation;
using Terraria.Localization;
using Terraria.WorldBuilding;

namespace ChangedSpecialMod.Common.WorldGeneration.Passes
{
    public abstract class WorldGenerationPass
    {
        public string PassName = string.Empty;
        public string GenerationMessage = string.Empty;

        public virtual void Generate()
        {

        }

        // Generate directly if enabled
        public void GenerateIfEnabled()
        {
            if (Enabled())
                Generate();
        }

        public virtual bool Enabled()
        {
            return true;
        }

        // Add as a generation pass if enabled
        public void AddPassIfEnabled(ref List<GenPass> tasks, ref int taskIndex)
        {
            if (Enabled())
            {
                tasks.Insert(++taskIndex, new PassLegacy(PassName, (progress, config) =>
                {
                    progress.Message = Language.GetTextValue($"Mods.ChangedSpecialMod.WorldGenerationPasses.{GenerationMessage}");
                    Generate();
                }));
            }
        }
    }
}
