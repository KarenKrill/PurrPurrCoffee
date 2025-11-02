#nullable enable
using Cysharp.Threading.Tasks;
using KarenKrill.UniCore.Storytelling.Abstractions;
using PurrPurrCoffee.Abstractions;
using System.Threading;
using Unity.Cinemachine;
using UnityEngine;

using Zenject;

namespace PurrPurrCoffee
{
    public class ZoomController : MonoBehaviour
    {
        [Inject]
        public void Initialize(IDialogueProvider dialogueProvider, IGameFlow gameFlow)
        {
            _gameFlow = gameFlow;
            dialogueProvider.DialogueStarted += OnDialogueStarted;
            dialogueProvider.DialogueEnded += OnDialogueEnded;
        }

        public async UniTask ZoomAsync(bool zoomIn)
        {
            lock (_ctsLock)
            {
                _cts?.Cancel();
            }
            while (_isZoomStarted)
            {
                await UniTask.NextFrame();
            }
            try
            {
                _isZoomStarted = true;
                _cts = new CancellationTokenSource();
                await UniTask.SwitchToMainThread();
                var startFov = _cinemachineCamera.Lens.FieldOfView;
                float prevTime = Time.realtimeSinceStartup;
                await UniTask.SwitchToThreadPool();
                var currentFov = startFov;
                var targetFov = zoomIn ? currentFov - _zoomInDelta : currentFov + _zoomInDelta;
                while (Mathf.Abs(targetFov - currentFov) > float.Epsilon && _cts != null && !_cts.IsCancellationRequested)
                {
                    await UniTask.SwitchToMainThread();
                    var curTime = Time.realtimeSinceStartup;
                    var deltaTime = curTime - prevTime;
                    prevTime = curTime;
                    currentFov = Mathf.Lerp(currentFov, targetFov, deltaTime * _zoomSpeed);
                    _cinemachineCamera.Lens.FieldOfView = currentFov;
                    await UniTask.SwitchToThreadPool();
                    await UniTask.NextFrame();
                }
                await UniTask.SwitchToMainThread();
                _cinemachineCamera.Lens.FieldOfView = targetFov;
            }
            finally
            {
                lock (_ctsLock)
                {
                    _cts?.Dispose();
                    _cts = null;
                }
                _isZoomStarted = false;
            }
        }

        [SerializeField]
        private CinemachineCamera _cinemachineCamera;
        [SerializeField]
        private float _zoomInDelta = 10;
        [SerializeField]
        private float _zoomSpeed = 1;
        private IGameFlow? _gameFlow;
        private CancellationTokenSource? _cts = null;
        private readonly object _ctsLock = new();
        private bool _isZoomStarted = false;

        private void OnDialogueStarted(string id)
        {
            if (_gameFlow!.State == GameState.Gameplay && id != "player_end_shift_1")
            {
                //_cinemachineCamera.Lens.FieldOfView -= 10;
                _ = UniTask.RunOnThreadPool(async () => await ZoomAsync(true));
            }
        }
        private void OnDialogueEnded(string id)
        {
            if(_gameFlow!.State == GameState.Gameplay && id != "player_end_shift_1")
            {
                //_cinemachineCamera.Lens.FieldOfView += 10;
                _ = UniTask.RunOnThreadPool(async () => await ZoomAsync(false));
            }
        }
    }
}
