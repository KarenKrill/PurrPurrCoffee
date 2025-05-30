namespace KarenKrill.InteractionSystem.Abstractions
{
    public interface IInteractionDetector
    {
        void Register(IInteractionTarget interactionTarget);
        void Unregister(IInteractionTarget interactionTarget);
    }
}
