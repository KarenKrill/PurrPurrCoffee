using System.Collections.Generic;
using UnityEngine;

namespace KarenKrill.Storytelling
{
    [CreateAssetMenu(fileName = "Storyline", menuName = nameof(Storytelling) + "/Storyline")]
    public class StorylineScriptableObject : ScriptableObject
    {
        public string storyLineName = string.Empty;
        public string description = string.Empty;
        public bool isMain = true;
        public List<QuestScriptableObject> quests = new();
    }
}
