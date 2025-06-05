using UnityEngine;
using TMPro;

using KarenKrill.UI.Views;

namespace PurrPurrCoffee.UI.Views
{
    using Abstractions;
    
    public class GameSessionStatusView : ViewBehaviour, IGameSessionStatusView
    {
        public string Reputation { set => _reputationText.text = value; }
        public string Money { set => _moneyText.text = value; }

        [SerializeField]
        private TextMeshProUGUI _reputationText;
        [SerializeField]
        private TextMeshProUGUI _moneyText;
    }
}