using UnityEngine;

public struct ScoreChangedEvent
{
    public int Score;

    public int BestScore;

    public ScoreChangedEvent( int score, int bestScore)
    {
        Score = score;
        BestScore = bestScore;
    }
}
