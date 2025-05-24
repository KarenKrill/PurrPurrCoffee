namespace PurrPurrCoffee.Abstractions
{
    public enum GameState
    {
        Initial,
        SceneLoad, // Load scene (with progress bar), show arts/tips
        MainMenu, // New, load, settings, [exit]
        CutScene, // Show some plot intro or cutscene
        Gameplay, // All gameplay stuff (Exploring, Stealth, Chase/Escape, Combat/Defence, Inventory, Puzzle/MiniGame)
        GameEnd,
        Pause,
        Exit
    }
}
