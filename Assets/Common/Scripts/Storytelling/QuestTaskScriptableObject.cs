using UnityEngine;

namespace KarenKrill.Storytelling
{
    [CreateAssetMenu(fileName = "QuestTask", menuName = nameof(Storytelling) +"/QuestTask")]
    public class QuestTaskScriptableObject : ScriptableObject
    {
        public string description = string.Empty;
    }
}
