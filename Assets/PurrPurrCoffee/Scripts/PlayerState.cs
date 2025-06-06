namespace PurrPurrCoffee
{
    public enum PlayerState
    {
        Sanity,
        DeathNear, // when player can be reanimated by another player (incapacitated/downed), can include some death timer
        Death,
        Disconnected // perhabs for other players it should be like blackout
    }
}
