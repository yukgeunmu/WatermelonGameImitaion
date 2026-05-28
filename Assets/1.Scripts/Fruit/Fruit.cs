using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Fruit : MonoBehaviour
{
    public FruitData Data { get; private set; }

    private bool isMerged;

    public bool IsMerged => isMerged;

    private CircleCollider2D circleCollider;
    private Rigidbody2D rigi2D;


    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        rigi2D = GetComponent<Rigidbody2D>();
    }

    public void Initialize(FruitData data)
    {
        Data = data;

        isMerged = false;

        transform.localScale = Vector3.one * data.Radius * 2f;

        circleCollider.radius = 0.5f;

        rigi2D.linearVelocity = Vector2.zero;
        rigi2D.angularVelocity = 0f;

        rigi2D.simulated = true;

        rigi2D.WakeUp();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isMerged)
            return;

        if (!collision.gameObject.TryGetComponent(out Fruit otherFruit))
            return;

        if (otherFruit.IsMerged)
            return;

        if (Data.Type != otherFruit.Data.Type)
            return;

        Merge(otherFruit);
    }

    private void Merge(Fruit otherFruit)
    {
        if (isMerged || otherFruit.IsMerged)
            return;

        isMerged = true;
        otherFruit.isMerged = true;

        Vector3 mergePosition = (transform.position + otherFruit.transform.position) * 0.5f;

        Game.Get<MergeManager>().Merge(this, otherFruit, mergePosition);
    }

    public void ReturnPool()
    {
        rigi2D.linearVelocity = Vector2.zero;
        rigi2D.angularVelocity = 0f;
        rigi2D.simulated = false;

        Game.Get<PoolManager>().Return(Data.Key.ToString(), this);
    }
}