using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingUI : SceneUI
{
    [SerializeField]
    private Slider progressBar;

    public void SetProgress(float progress)
    {
        progressBar.value = progress;
    }
}