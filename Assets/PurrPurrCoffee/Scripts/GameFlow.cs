using UnityEngine;
using UnityEngine.SceneManagement;

using KarenKrill.StateSystem.Abstractions;

namespace PurrPurrCoffee
{
    using Abstractions;

    public class GameFlow : IGameFlow
    {
        public GameState State => _stateSwitcher.State;

        public GameFlow(IStateSwitcher<GameState> stateSwitcher)
        {
            _stateSwitcher = stateSwitcher;
        }
        public void EndGame()
        {
            _stateSwitcher.TransitTo(GameState.GameEnd);
        }
        public void Exit()
        {
            throw new System.NotImplementedException();
        }
        public void FinishLevel()
        {
            throw new System.NotImplementedException();
        }
        public void LoadLevel(long index)
        {
            throw new System.NotImplementedException();
        }
        AsyncOperation _loadSceneAwaiter;
        public void LoadMainMenu()
        {
            _loadSceneAwaiter = SceneManager.LoadSceneAsync("MenuScene", LoadSceneMode.Single);
            _loadSceneAwaiter.completed += OnMainMenuLoadCompleted;
            //_stateSwitcher.TransitTo(GameState.SceneLoad);
        }

        private void OnMainMenuLoadCompleted(AsyncOperation obj)
        {
            _stateSwitcher.TransitTo(GameState.MainMenu);
        }

        public void LoseGame()
        {
            throw new System.NotImplementedException();
        }
        public void PauseLevel()
        {
            throw new System.NotImplementedException();
        }
        public void PlayLevel()
        {
            throw new System.NotImplementedException();
        }
        public void RestartGame()
        {
            throw new System.NotImplementedException();
        }
        public void StartGame()
        {
            _loadSceneAwaiter = SceneManager.LoadSceneAsync("LevelScene", LoadSceneMode.Single);
            _loadSceneAwaiter.completed += OnLevelLoadCompleted;
            //_stateSwitcher.TransitTo(GameState.SceneLoad);
        }
        private void OnLevelLoadCompleted(AsyncOperation obj)
        {
            _stateSwitcher.TransitTo(GameState.Gameplay);
        }
        public void WinGame()
        {
            throw new System.NotImplementedException();
        }

        private readonly IStateSwitcher<GameState> _stateSwitcher;
    }
}
