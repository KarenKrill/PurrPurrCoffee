using KarenKrill.Input.Abstractions;

namespace PurrPurrCoffee.Input.Abstractions
{
    public interface IInputActionService : IBasicActionsProvider, IBasicPlayerActionsProvider, IBasicUIActionsProvider
    {
    }
}
