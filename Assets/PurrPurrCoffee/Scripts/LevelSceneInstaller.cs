using UnityEngine;
using Zenject;

using KarenKrill.UniCore.Input.Abstractions;
using KarenKrill.UniCore.Interactions.Abstractions;
using KarenKrill.UniCore.Interactions;
using KarenKrill.UniCore.Movement;

namespace PurrPurrCoffee
{
    public class LevelSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var playerActionsProvider = Container.Resolve<IBasicPlayerActionsProvider>();
            var interactionTargetRegistry = Container.Resolve<IInteractionTargetRegistry>();
            _raycastInteractionDetector.Initialize(playerActionsProvider, interactionTargetRegistry);
            _inputCharacterMoveController.Initialize(playerActionsProvider);
        }

        [SerializeField]
        private RaycastInteractionDetector _raycastInteractionDetector;
        [SerializeField]
        private InputCharacterMoveContoller _inputCharacterMoveController;
    }
}
