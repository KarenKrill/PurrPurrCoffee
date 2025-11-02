#nullable enable

using System;

using KarenKrill.UniCore.UI.Presenters.Abstractions;

namespace PurrPurrCoffee.UI.Presenters.Abstractions
{
    using Views.Abstractions;

    public interface IDialoguePresenter : IPresenter<IDialogueView>
    {
        public bool ShowInteractionTooltip { set; }

        public event Action<int>? ChoiceMade;
        public event Action? Continued;
        public event Action? Skipped;
    }
}
