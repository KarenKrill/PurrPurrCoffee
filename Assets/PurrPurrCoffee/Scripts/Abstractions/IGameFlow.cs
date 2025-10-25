namespace PurrPurrCoffee.Abstractions
{
    public interface IGameFlow
    {
        GameState State { get; }

        void LoadMainMenu();
        void StartGame();
        void RestartGame();
        void EndGame();
        void WinGame();
        void LoseGame();
        void Exit();
        void LoadLevel(long index);
        void PlayLevel();
        void PauseLevel();
        void FinishLevel();
        void PlayCutscene(string id);
    }
}
