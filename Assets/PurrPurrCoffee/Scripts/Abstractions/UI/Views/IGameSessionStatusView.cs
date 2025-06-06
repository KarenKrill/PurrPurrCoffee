#nullable enable

using KarenKrill.UI.Views.Abstractions;

namespace PurrPurrCoffee.UI.Views.Abstractions
{
    public interface IGameSessionStatusView : IView
    {
        public string Reputation { set; }
        public string Money { set; }
    }
}