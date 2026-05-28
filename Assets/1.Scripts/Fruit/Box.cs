using UnityEngine;

public class Box : MonoBehaviour
{
    void Start()
    {
        float worldHeight = Camera.main.orthographicSize * 2f;

        float worldWidth =  worldHeight * Screen.width / Screen.height;

        transform.position = new Vector3(worldWidth, worldHeight, 0);

    }
}
