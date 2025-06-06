#nullable enable

using System;

namespace KarenKrill.Storytelling.Abstractions
{
    public interface IDialogueProvider
    {
        DialogueState CurrentDialogueState { get; }

        event Action<DialogueState>? DialogueStateChanged;
        event Action<int>? DialogueStarting;
        event Action<int>? DialogueStarted;
        event Action<int>? DialogueEnded;
    }
}