using System;
using System.Diagnostics;

namespace KarenKrill.Storytelling.Abstractions
{
    public interface IDialogueService
    {
        public event Action<int, int, float> ClientServed; // конечно здесь не должно быть такиех конкретных событий, но сроки душат
        void StartDialogue(int id);
        void MakeDialogueChoice(int index);
        void NextDialogueLine();
        void SkipDialogue();
        void SetVariable(string name, bool state = true);
    }
}
