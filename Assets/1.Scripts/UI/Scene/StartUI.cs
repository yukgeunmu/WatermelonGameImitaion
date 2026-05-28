using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUI : SceneUI
{
    [SerializeField]
    private Button startBtn;

    private void Awake()
    {
        startBtn.onClick.AddListener(OnClickStartBtn);
    }


    public override void Close()
    {
        base.Close();
        this.gameObject.SetActive(false);
    }

    private void OnClickStartBtn()
    {
        Game.Get<SoundManager>().PlaySFX(SoundType.BtnClickSFX);
        SceneManager.LoadSceneAsync("LoadingScene");
    }
}
