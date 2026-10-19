using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems.ModSupport
{
    public class ModSupportStarsModPack : ModSupportData
    {
        public ModSupportStarsModPack(Mod mod, string modName) : base(mod, modName) {  }

        public override void AddTownNPCs()
        {
            AddTownNPC("Farmer", "StarsModPackFarmer");
            AddTownNPC("Jellybean", "StarsModPackJellybean");
            AddTownNPC("Scarfy", "StarsModPackScarfy");
            AddTownNPC("ScrapyardGeek", "StarsModPackScrapyardGeek");
            AddTownNPC("SubzeroSpecialist", "StarsModPackSubzeroSpecialist");
        }
    }
}
