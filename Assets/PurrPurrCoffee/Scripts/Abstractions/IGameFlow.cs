namespace PurrPurrCoffee.Abstractions
{
    public interface IGameFlow
    {
        GameState State { get; }

        void LoadMainMenu();
        void LoadLevel(long index);
        void RestartGame();
        void PlayLevel();
        void PauseLevel();
        void FinishLevel();
        void StartGame();
        void EndGame();
        void WinGame();
        void LoseGame();
        void Exit();
    }
}
