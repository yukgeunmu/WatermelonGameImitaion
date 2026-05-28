using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : SceneUI
{
    [field: SerializeField]
    private Image NextFruitImage;

    [field: SerializeField]
    private TextMeshProUGUI ScoreText;

    [field: SerializeField]
    private TextMeshProUGUI BestScoreText;

    [field: SerializeField]
    private TextMeshProUGUI ComboText;


    [field: SerializeField]
    private Button OptionBtn;


    private void Awake()
    {
        OptionBtn.onClick.AddListener(OnClickOption);
    }


    public void SetScore(int score)
    {
        ScoreText.text = $"{score}";
    }

    public void SetBestScore(int bestScore)
    {
        BestScoreText.text = $"{bestScore}";
    }

    public void SetNextImage(FruitData fruitData)
    {
        NextFruitImage.sprite = fruitData.sprite;
    }

    public void SetCombo(int combo)
    {
        if (combo <= 0)
        {
            ComboText.gameObject.SetActive(false);

            return;
        }

        ComboText.gameObject.SetActive(true);

        ComboText.text = $"Combo x{combo}";

        PlayComboEffect();

    }

    private void PlayComboEffect()
    {
        ComboText.transform.DOKill();

        ComboText.transform.localScale =
            Vector3.one;

        ComboText.color = Random.ColorHSV(
                0f,
                1f,
                0.7f,
                1f,
                0.2f,
                0.6f);

        ComboText.transform.localRotation =
            Quaternion.identity;

        ComboText.transform
            .DOPunchScale(
                Vector3.one * 0.4f,
                0.3f,
                8,
                0.8f);

        ComboText.transform
            .DOPunchRotation(
                new Vector3(0f, 0f, 15f),
                0.3f,
                10,
                1f);
    }

    private void OnClickOption()
    {
        Game.Get<SoundManager>().PlaySFX(SoundType.BtnClickSFX);
        Game.Get<UIManager>().ShowPopup<OptionPopupUI>();
    }


}
