using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    [SerializeField]
    private float duration = 0.15f;

    [SerializeField]
    private float magnitude = 0.15f;

    private Vector3 originalPosition;

    private void Awake()
    {
        Instance = this;

        originalPosition = transform.localPosition;
    }

    public void Shake()
    {
        StopAllCoroutines();

        StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x =
                Random.Range(-1f, 1f) * magnitude;

            float y =
                Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}