using TMPro;
using UnityEngine;

public class ScoreManager :  IManager
{
    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private TMP_Text bestScoreText;

    private int currentScore;

    public int CurrentScore => currentScore;

    private int bestScore;

    public int BestScore => bestScore;

    private const string BEST_SCORE_KEY = "BEST_SCORE";


    public void Initialize()
    {  
        currentScore = 0;
        GameEventBus.Subscribe<FruitMergedEvent>(OnFruitMerged);
    }


    public void Dispose()
    {
        GameEventBus.Unsubscribe<FruitMergedEvent>(OnFruitMerged);
    }


    private void OnFruitMerged(FruitMergedEvent evt)
    {
        float multiplier = Game.Get<ComboManager>().GetComboMultiplier();

        int finalScore = Mathf.RoundToInt(evt.FruitData.Score * multiplier);

        AddScore(finalScore);
    }

    public void AddScore(int score)
    {
        currentScore += score;

        if (currentScore > bestScore)
        {
            bestScore = currentScore;

            SaveBestScore();
        }

        GameEventBus.Publish(new ScoreChangedEvent(currentScore, bestScore));
    }

    public void LoadBestScore()
    {
        bestScore = PlayerPrefs.GetInt( BEST_SCORE_KEY, 0);
        GameEventBus.Publish(new ScoreChangedEvent(currentScore, bestScore));
    }

    private void SaveBestScore()
    {
        PlayerPrefs.SetInt( BEST_SCORE_KEY, bestScore);

        PlayerPrefs.Save();
    }

    public void ResetScore()
    {
        currentScore = 0;
        GameEventBus.Publish(new ScoreChangedEvent(currentScore, bestScore));
    }


}