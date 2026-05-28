using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class UIManager : IManager
{
    private SceneUI currentSceneUI;

    public SceneUI CurrentSceneUI => currentSceneUI;

    private Stack<PopupUI> popupStack = new();

    private Transform sceneRoot;

    private Transform popupRoot;

    public bool IsInputBlocked { get; private set; }

    public void Initialize()
    {
        BindRoots();

        SubscribeEvents();
    }

    public void Dispose()
    {
        UnsubscribeEvents();
    }


    private void BindRoots()
    {
        UIRoot root = Object.FindFirstObjectByType<UIRoot>();


        if(root == null)
        {
            GameObject rootObject = new GameObject("UIRoot");

            rootObject.transform.SetParent(Manager.Instance.transform);

            root = rootObject.AddComponent<UIRoot>();

            CreateCanvasRoots(root);
        }

        sceneRoot = root.SceneRoot;
        popupRoot = root.PopupRoot;

    }

    private void CreateCanvasRoots(UIRoot root)
    {
        GameObject sceneCanvas = CreateCanvas("SceneCanvas", 10 );

        GameObject popupCanvas =  CreateCanvas("PopupCanvas", 11);

        sceneCanvas.transform.SetParent(root.transform);

        popupCanvas.transform.SetParent(root.transform);

        root.SetRoots( sceneCanvas.transform, popupCanvas.transform);
    }

    private GameObject CreateCanvas(string name, int order)
    {
        GameObject canvasObject = new GameObject(name);

        Canvas canvas = canvasObject.AddComponent<Canvas>();

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        canvas.sortingOrder = order;

        canvasObject.AddComponent<UnityEngine.UI.CanvasScaler>();

        canvasObject.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        return canvasObject;
    }


    public void SetSceneUI<T>(string label) where T : SceneUI
    {
        string key = typeof(T).Name;

        GameObject prefab = Game.Get<ResourceManager>().GetAsset<GameObject>(label, key);


        GameObject instance = Object.Instantiate(prefab, sceneRoot);

        currentSceneUI = instance.GetComponent<SceneUI>();
    }


    public void CreatePopupUIPool<T>(string label) where T : PopupUI
    {
        string key = typeof(T).Name;

        T prefab = Game.Get<ResourceManager>().GetAsset<GameObject>(label, key).GetComponent<T>();

        Game.Get<PoolManager>().CreatePool<T>(key,
            () =>
            {
               return Object.Instantiate(prefab);
            }, 1,  popupRoot
            );
    }


    public T ShowPopup<T>() where T : PopupUI
    {
        string key = typeof(T).Name;

        T popup = Game.Get<PoolManager>().Get<T>(key);

        popup.Open();

        popupStack.Push(popup);

        IsInputBlocked = true;

        return popup;
    }

    public void ClosePopup()
    {
        if (popupStack.Count <= 0)
            return;

        PopupUI popup = popupStack.Pop();

        IsInputBlocked = popupStack.Count > 0;

        popup.Close();
    }

    private void SubscribeEvents()
    {
        GameEventBus.Subscribe<ScoreChangedEvent>(OnScoreChanged);

        GameEventBus.Subscribe<ComboChangedEvent>(OnComboChanged);

        GameEventBus.Subscribe<GameOverEvent>(OnGameOver);

        GameEventBus.Subscribe<FruitChangedEvent>(OnNextFruitChanged);
    }

    private void UnsubscribeEvents()
    {
        GameEventBus.Unsubscribe<ScoreChangedEvent>(OnScoreChanged);

        GameEventBus.Unsubscribe<ComboChangedEvent>(OnComboChanged);

        GameEventBus.Unsubscribe<GameOverEvent>(OnGameOver);

        GameEventBus.Unsubscribe<FruitChangedEvent>(OnNextFruitChanged);
    }

    private void OnScoreChanged(ScoreChangedEvent evt)
    {
        if (currentSceneUI is InGameUI ui)
        {
            ui.SetScore(evt.Score);
            ui.SetBestScore(evt.BestScore);
        }
    }

    private void OnComboChanged(ComboChangedEvent evt)
    {
        if (currentSceneUI is InGameUI ui)
        {
            ui.SetCombo(evt.Combo);
        }
    }

    private void OnNextFruitChanged(FruitChangedEvent evt)
    {
        if (currentSceneUI is InGameUI ui)
        {
            ui.SetNextImage(evt.FruitData);
        }
    }

    public void CloseAllPopup()
    {
        while (popupStack.Count > 0)
        {
            ClosePopup();
        }
    }

    private void OnGameOver(GameOverEvent evt)
    {
        Game.Get<UIManager>().ShowPopup<ResultPopupUI>().SetScore(evt.Score);
    }
}