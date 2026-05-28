using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPopupUI : PopupUI
{
    [SerializeField]
    private TMP_Text scoreText;


    [SerializeField]
    private Button restartButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(OnClickRestart);
    }

    private void OnDestroy()
    {
        restartButton.onClick.RemoveListener(OnClickRestart);
    }

    public override void Open()
    {
        base.Open();
        SetPoolKey(nameof(ResultPopupUI));
    }

    public override void Close()
    {
        base.Close();
        Game.Get<PoolManager>().Return(PoolKey, this);
    }

    public void SetScore(int score)
    {
        scoreText.text = $"{score}";
    }

    private void OnClickRestart()
    {
        Game.Get<GameManager>().RestartGame();
    }


}
