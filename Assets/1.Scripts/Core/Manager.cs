using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.U2D;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set; }

    private async void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }

        Game.Initialize();

        await LoadStartAsset();

        Game.Get<UIManager>().SetSceneUI<StartUI>("Start");

        Game.Get<SoundManager>().SetStartSound();

    }


    private void OnDestroy()
    {
        if (Instance != this)
            return;

        Instance = null;
    }

    private void OnApplicationQuit()
    {
        Game.Dispose();
    }

    public void CreatePoolRoot()
    {
        GameObject poolRoot = new GameObject("PoolRoot");

        poolRoot.transform.SetParent(this.transform);

        Game.Get<PoolManager>().SetRoot(poolRoot.transform);

        Game.Get<GameManager>().InitializeRuntime();

    }

    private async Task LoadStartAsset()
    {
        await Game.Get<ResourceManager>().LoadAssetsAsync<GameObject>("Start");

        await Game.Get<ResourceManager>().LoadAssetsAsync<AudioMixer>("Audio");

        await Game.Get<ResourceManager>().LoadDataAsync<SoundData, SoundType>("SoundData");
    }

    public async Task LoadGameAssetResources()
    {
        ResourceManager resourceManager = Game.Get<ResourceManager>();

        await resourceManager.LoadDataAsync<FruitData, FruitType>("FruitData");

        await resourceManager.LoadDataAsync<EffectData, EffectType>("EffectData");

        await resourceManager.LoadAssetsAsync<GameObject>("InGame");
    }

    public void SpawnGameplayObjects()
    {

        GameObject spawnPrefab = Game.Get<ResourceManager>().GetAsset<GameObject>("InGame", "FruitSpawner");

        GameObject boxPrefab = Game.Get<ResourceManager>().GetAsset<GameObject>("InGame","Box");

        GameObject box = Instantiate(boxPrefab);

        GameObject spawner = Instantiate(spawnPrefab);

        BoxCollider2D boxCollider = box.GetComponent<BoxCollider2D>();

        FruitSpawner fruitSpawner =spawner.GetComponent<FruitSpawner>();

        fruitSpawner.Initialize(boxCollider);

    }

}