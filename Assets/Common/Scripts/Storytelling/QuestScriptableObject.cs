using System.Collections.Generic;
using UnityEngine;

namespace KarenKrill.Storytelling
{
    [CreateAssetMenu(fileName = "Quest", menuName = nameof(Storytelling) +"/Quest")]
    public class QuestScriptableObject : ScriptableObject
    {
        public string questName = string.Empty;
        public string description = string.Empty;
        public List<QuestTaskScriptableObject> questTasks = new();
    }
}
