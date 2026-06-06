using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ChangedSpecialMod.Common.Configs
{
    public sealed class ChangedSpecialModClientConfig : ModConfig
    {
        public static ChangedSpecialModClientConfig Instance;
        public override void OnLoaded()
        {
            Instance = this;
        }

        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("Audio")]
        [DefaultValue(true)]
        public bool TransfurSound { get; set; }

        [DefaultValue(true)]
        public bool WhipCrackSound { get; set; }

        [Header("Npc")]
        [DefaultValue(true)]
        public bool NPCsCanUseChangedEmotes { get; set; }

        [DefaultValue(true)]
        public bool ExtraDialogue { get; set; }

        [DefaultValue(true)]
        public bool DialoguePortraits { get; set; }

        [Header("Transfur")]
        [DefaultValue(false)]
        public bool NPCsCanTransfurPlayer { get; set; }

        [DefaultValue(false)]
        public bool CustomHealthBar { get; set; }

        [Header("WorldGeneration")]
        [Range(0, 20)]
        [DefaultValue(1)]
        public int NumberOfLabs { get; set; }

        [Range(4, 15)]
        [DefaultValue(4)]
        public int MaximumFloors { get; set; }

        [Range(5, 8)]
        [DefaultValue(5)]
        public int MaximumRoomsHorizontal { get; set; }

        [DefaultValue(true)]
        public bool LatexSpread { get; set; }

        [Range(1, 30)]
        [DefaultValue(10)]
        public int LatexSpreadChance { get; set; }

        [Header("Misc")]
        [DefaultValue(true)]
        public bool Evolution { get; set; }

        [DefaultValue(false)]
        public bool DebugItems { get; set; }
    }
}
