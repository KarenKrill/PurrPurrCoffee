using KarenKrill.UniCore.Storytelling;
using System;

namespace PurrPurrCoffee
{
    public class QuestController
    {
        public event Action ClientApproaching;
        public event Action DemonTime;
        public event Action ScreamerTime;

        public QuestController()
        {
        }
        public void StartWith(QuestScriptableObject quest)
        {
        }
    }
}
