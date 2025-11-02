#nullable enable

using KarenKrill.UniCore.UI.Views.Abstractions;

namespace PurrPurrCoffee.UI.Views.Abstractions
{
    public interface IDiagnosticInfoView : IView
    {
        public string FpsText { set; }
    }
}