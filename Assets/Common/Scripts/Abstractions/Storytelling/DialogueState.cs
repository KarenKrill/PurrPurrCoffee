namespace KarenKrill.Storytelling.Abstractions
{
    public class DialogueState
    {
        public string Name_Temp { get; }
        public string Line { get; }
        public string[] Choices { get; }
        public DialogueState(string line, string[] choices, string name_Temp)
        {
            Line = line;
            Choices = choices;
            Name_Temp = name_Temp;
            Name_Temp = name_Temp;
        }
    }
}