namespace ChangedSpecialMod.Content.NPCs
{
    public class DialogueElement
    {
        public string Dialogue;
        public string Emotion;

        public DialogueElement(string dialogue, string emotion = "Talk")
        {
            Dialogue = dialogue;
            Emotion = emotion;
        }
    }
}
