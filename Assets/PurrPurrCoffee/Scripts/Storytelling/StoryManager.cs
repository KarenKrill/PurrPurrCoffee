using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KarenKrill.Storytelling.Abstractions;

namespace PurrPurrCoffee.Storytelling
{
    public class StoryManager : IStoryManager
    {
        public StoryManager(IDialogueService dialogueService)
        {
            _dialogueService = dialogueService;
        }
        public void StartDialogue(int id)
        {
            _dialogueService.StartDialogue(id);
        }
        private readonly IDialogueService _dialogueService;
    }
}
