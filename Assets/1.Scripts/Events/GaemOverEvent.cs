public struct GameOverEvent
{
    public int Score { get; }

    public GameOverEvent(int score)
    {
        Score = score;
    }
}