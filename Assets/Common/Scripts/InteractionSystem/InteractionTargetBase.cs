using UnityEngine;
using Zenject;

namespace KarenKrill.InteractionSystem
{
    using Abstractions;

    public abstract class InteractionTargetBase : MonoBehaviour, IInteractionTarget
    {
        public abstract IInteractable Interactable { get; }
        
        [Inject]
        public void Initialize(IInteractionDetector interactionDetector)
        {
            _interactionDetector = interactionDetector;
        }

        protected virtual void OnEnable()
        {
            _interactionDetector?.Register(this);
        }
        protected virtual void OnDisable()
        {
            _interactionDetector?.Unregister(this);
        }

        private IInteractionDetector _interactionDetector;
    }
}
