using TMPro;
using UnityEngine;

public class UIBinder : MonoBehaviour
{
    [field: SerializeField]
    public TMP_Text ScoreText { get; private set; }

    [field: SerializeField]
    public TMP_Text BestScoreText { get; private set; }

    [field: SerializeField]
    public TMP_Text ComboText { get; private set; }

    [field: SerializeField]
    public GameObject GameOverPanel { get; private set; }
}