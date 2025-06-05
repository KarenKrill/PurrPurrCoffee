using UnityEngine;

namespace KarenKrill.Storytelling
{
    [CreateAssetMenu(fileName = "JaneDoe", menuName = nameof(Storytelling) +"/Actor")]
    public class ActorScriptableObject : ScriptableObject
    {
        public string firstname = "Jane";
        public string secondname = "Doe";
        public uint age = 27;
        public Texture2D icon = null;
        public string description = string.Empty;
        [Header("Is this actor known to the player?")]
        public bool isKnown = false;
    }
}
