#nullable enable

using System;

using KarenKrill.UI.Presenters.Abstractions;

namespace PurrPurrCoffee.UI.Presenters.Abstractions
{
    using Views.Abstractions;

    public interface IDialoguePresenter : IPresenter<IDialogueView>
    {
        public event Action<int>? ChoiceMade;
        public event Action? NextLineRequested;
        public event Action? SkipRequested;
    }
}
