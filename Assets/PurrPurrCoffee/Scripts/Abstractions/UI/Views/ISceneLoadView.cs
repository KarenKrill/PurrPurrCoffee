#nullable enable

using System;

using KarenKrill.UI.Views.Abstractions;

namespace PurrPurrCoffee.UI.Views.Abstractions
{
    public interface ISceneLoadView : IView
    {
        public float Progress { set; }
        public event Action StartConfirmed;
    }
}