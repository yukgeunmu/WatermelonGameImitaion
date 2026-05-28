using System.Collections;
using UnityEngine;

public class GameOverLine : MonoBehaviour
{
    [SerializeField]
    private float gameOverDelay = 2f;

    private Coroutine gameOverCoroutine;

    private int fruitCount;

    private void OnTriggerEnter2D(
        Collider2D collision)
    {
        if (!collision.TryGetComponent( out Fruit fruit))
        {
            return;
        }

        fruitCount++;

        if (gameOverCoroutine == null)
        {
            gameOverCoroutine = StartCoroutine(GameOverRoutine());
        }
    }

    private void OnTriggerExit2D( Collider2D collision)
    {
        if (!collision.TryGetComponent(out Fruit fruit))
        {
            return;
        }

        fruitCount--;

        if (fruitCount <= 0)
        {
            fruitCount = 0;

            if (gameOverCoroutine != null)
            {
                StopCoroutine(gameOverCoroutine);

                gameOverCoroutine = null;
            }
        }
    }


    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(gameOverDelay);

        Game.Get<GameManager>().GameOver();
    }
}