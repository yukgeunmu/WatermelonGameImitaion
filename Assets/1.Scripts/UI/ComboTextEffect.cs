using UnityEngine;

public class ComboTextEffect : MonoBehaviour
{
    [SerializeField]
    private float scaleMultiplier = 1.3f;

    [SerializeField]
    private float speed = 10f;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale =  originalScale * scaleMultiplier;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp( transform.localScale,  originalScale, Time.deltaTime * speed);
    }
}