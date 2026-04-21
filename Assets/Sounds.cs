using Terraria.Audio;

namespace ChangedSpecialMod.Assets
{
    public static class Sounds
    {
        // Base paths
        private const string BasePathMusic = "Assets/Music";
        private const string BasePathSound = $"{nameof(ChangedSpecialMod)}/Assets/Sounds";

        private static string Music(string name) => $"{BasePathMusic}/{name}";
        private static string Sound(string name) => $"{BasePathSound}/{name}";

        // Music
        public static readonly string MusicBlackLatexZone = Music("BlackGooZone");
        public static readonly string MusicBlackLatexZone2 = Music("ScarletCrystalMine");
        public static readonly string MusicCrystalZone = Music("CrystalZone");
        public static readonly string MusicWhiteLatexZone = Music("WhiteGooJungle");
        public static readonly string MusicLibrary = Music("Library");
        public static readonly string MusicLabSlow = Music("LaboratorySlow");
        public static readonly string MusicLab = Music("Laboratory");
        public static readonly string MusicVents = Music("VentPipe");
        public static readonly string MusicPuro = Music("PuroTheBlackGoo");
        public static readonly string MusicGreenhouse = Music("Greenhouse");

        // Party
        public static readonly string MusicHappyBirthday = Music("BirthdaySong");
        public static readonly string MusicPuroDance = Music("PuroDance");

        // Drunk
        public static readonly string MusicWideLatex = Music("SongForDenise");

        // Vents chase sequence
        public static readonly string MusicRun = Music("Run");

        // Bosses
        public static readonly string MusicWhiteTailChase2 = Music("TheWhiteTailChasePart2");
        public static readonly string MusicWolfKing = Music("WolfKing");
        public static readonly string MusicBehemoth = Music("Behemoth");
        public static readonly string MusicShark = Music("Shark");
        public static readonly string MusicSquidDog = Music("SquidDog");

        // Sounds
        public static readonly SoundStyle SoundTransfur = new(Sound("Poison"));       // Transfur
        public static readonly SoundStyle SoundSlam = new(Sound("Blow6"));            // Behemoth slamming fist
        public static readonly SoundStyle SoundBuzzer1 = new(Sound("Buzzer1"));       // Can't use elevator
        public static readonly SoundStyle SoundBuzzer2 = new(Sound("Buzzer2"));       // Generator turning off
        public static readonly SoundStyle SoundElevator = new(Sound("Chime1"));       // Using the elevator
        public static readonly SoundStyle SoundChime2 = new(Sound("Chime2"));         // Generator turning on
        public static readonly SoundStyle SoundHahaha = new(Sound("Hahaha"));         // Pelo laugh
        public static readonly SoundStyle SoundAwoo = new(Sound("Wolf"));             // Cheerleaders
        public static readonly SoundStyle SoundCat = new(Sound("Cat"));               // Cat summon weapon
        public static readonly SoundStyle SoundBook = new(Sound("Book"));             // Flipping through pictures
        public static readonly SoundStyle SoundDoor = new(Sound("Close2"));           // Door sound, currently unused because I couldn't figure out how to overwrite the vanilla door sound properly
        public static readonly SoundStyle SoundSave = new(Sound("Save"));             // Iris scanner
        public static readonly SoundStyle SoundLoad = new(Sound("Load"));             // Iris scanner

        // Boss fight
        public static readonly SoundStyle SoundSword1 = new(Sound("Sword1"));         // Wolf King snapping fingers
        public static readonly SoundStyle SoundPush = new(Sound("Push"));             // Wolf king turning his chair
        public static readonly SoundStyle SoundSpike = new(Sound("Ice2"));            // Spike shooting up
        public static readonly SoundStyle SoundMonster = new(Sound("Monster2"));      // Tiger shark angry

        // Drunk
        public static readonly SoundStyle SoundBalloon = new(Sound("Balloon"));       // Gas tank flying
        public static readonly SoundStyle SoundNom = new(Sound("Nom"));               // Hungry locker and Puro worm
        public static readonly SoundStyle SoundAroo = new(Sound("Aroo"));             // Cheerleaders on the drunk world seed
    }
}
