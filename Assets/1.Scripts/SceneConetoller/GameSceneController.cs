using UnityEngine;

public class GameSceneController : MonoBehaviour
{
    private void Awake()
    {
        Game.Get<SoundManager>().StartGamSceneBGM();

        Manager.Instance.CreatePoolRoot();

        Game.Get<UIManager>().SetSceneUI<InGameUI>("InGame");

        Manager.Instance.SpawnGameplayObjects();

        Game.Get<ScoreManager>().LoadBestScore();
    }

    private void Update()
    {
        Game.Get<ComboManager>().Tick(Time.deltaTime);
    }
}
