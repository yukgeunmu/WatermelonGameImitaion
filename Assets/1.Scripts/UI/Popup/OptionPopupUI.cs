using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionPopupUI : PopupUI
{
    [Header("Volume Sliders")]
    [SerializeField]
    private Slider masterSlider;

    [SerializeField]
    private Slider bgmSlider;

    [SerializeField]
    private Slider sfxSlider;

    [Header("Buttons")]
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Button resetButton;
    [SerializeField]
    private Button quitButton;

    [Header("Text")]
    [SerializeField]
    private TextMeshProUGUI masterVolumeText;
    [SerializeField]
    private TextMeshProUGUI bgmVolumeText;
    [SerializeField]
    private TextMeshProUGUI sfxVolumeText;


    private void Awake()
    {
        BindEvents();

        InitializeSliders();
    }


    private void BindEvents()
    {
        closeButton.onClick.AddListener(OnClickCloseBtn);

        resetButton.onClick.AddListener(OnClickResetBtn);

        quitButton.onClick.AddListener(OnClickQuitBtn);

        masterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);

        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);

        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void InitializeSliders()
    {
        SoundManager soundManager =Game.Get<SoundManager>();

        masterSlider.SetValueWithoutNotify(soundManager.GetMasterVolume());

        bgmSlider.SetValueWithoutNotify(soundManager.GetBGMVolume());

        sfxSlider.SetValueWithoutNotify(soundManager.GetSFXVolume());

        RefreshText();
    }

    public override void Open()
    {
        base.Open();
        SetPoolKey(nameof(OptionPopupUI));
    }

    public override void Close()
    {
        base.Close();
        Game.Get<PoolManager>().Return(PoolKey, this);
    }

    private void OnMasterVolumeChanged(float value)
    {
        Game.Get<SoundManager>().SetMasterVolume(value);
        masterVolumeText.text = $"{Mathf.FloorToInt(value * 100)}%";
    }

    private void OnBGMVolumeChanged(float value)
    {
        Game.Get<SoundManager>().SetBGMVolume(value);
        bgmVolumeText.text = $"{Mathf.FloorToInt(value * 100)}%";
    }

    private void OnSFXVolumeChanged(float value)
    {
        Game.Get<SoundManager>().SetSFXVolume(value);
        sfxVolumeText.text = $"{Mathf.FloorToInt(value * 100)}%";
    }

    private void OnClickCloseBtn()
    {
        Game.Get<SoundManager>().PlaySFX(SoundType.BtnClickSFX);
        Game.Get<UIManager>().ClosePopup();
    }

    private void OnClickResetBtn()
    {
        Game.Get<SoundManager>().PlaySFX(SoundType.BtnClickSFX);
        Game.Get<GameManager>().RestartGame();
    }

    private void RefreshText()
    {
        masterVolumeText.text = $"{masterSlider.value * 100}%";
        bgmVolumeText.text = $"{bgmSlider.value * 100}%";
        sfxVolumeText.text = $"{sfxSlider.value * 100}%";
    }

    private void OnClickQuitBtn()
    {
        Game.Get<SoundManager>().PlaySFX(SoundType.BtnClickSFX);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }





}
