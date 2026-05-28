using UnityEngine;

public class UIRoot : MonoBehaviour
{
    [field: SerializeField]
    public Transform SceneRoot { get; private set; }

    [field: SerializeField]
    public Transform PopupRoot { get; private set; }

    public void SetRoots(Transform sceneRoot,Transform popupRoot)
    {
        SceneRoot = sceneRoot;
        PopupRoot = popupRoot;
    }
}
