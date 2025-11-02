using Zenject;

using KarenKrill.UniCore.Interactions.Abstractions;
using KarenKrill.UniCore.Interactions;

namespace PurrPurrCoffee.Interactions
{
    public class RaycastInteractionTargetDiWrap : RaycastInteractionTarget, IInteractionTarget
    {
        [Inject]
        public override void Initialize(IInteractionTargetRegistry interactionTargetRegistry)
        {
            base.Initialize(interactionTargetRegistry);
        }
    }
}
