using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{

    public LoadingUI loadingUI;


    private async void Start()
    {
        await LoadAsync();
    }


    private async Task LoadAsync()
    {
        ResourceManager resourceManager = Game.Get<ResourceManager>();

        Game.Get<UIManager>().CurrentSceneUI.Close();

        resourceManager.ReleaseAsset("Start");

        await Manager.Instance.LoadGameAssetResources();

        AsyncOperation operation =  SceneManager.LoadSceneAsync("GameScene");

        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            float progress =  operation.progress;

            loadingUI.SetProgress(progress);

            await Task.Yield();
        }

        loadingUI.SetProgress(1f);

        await Task.Delay(500);

        operation.allowSceneActivation = true;

        Game.Get<SoundManager>().StopBGM();
    }

}
