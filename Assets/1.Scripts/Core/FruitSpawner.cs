using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Threading.Tasks;

public class FruitSpawner : MonoBehaviour
{

    [SerializeField] private float minX = -2.8f;
    [SerializeField] private float maxX = 2.8f;

    [SerializeField] private float dropCooldown = 0.5f;

   private BoxCollider2D dropArea;

    private PlayerInputActions inputActions;

    List<FruitData> fruits;


    private FruitData currentFruitData;
    private FruitData nextFruitData;

    public SpriteRenderer currentPreviewFruit;

    private bool canDrop = true;

    private bool isDragging;


    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    public void Initialize(BoxCollider2D collider2D)
    {
        fruits = Game.Get<ResourceManager>().GetAllResource<FruitData, FruitType>();

        fruits.Sort((a, b) => a.Order.CompareTo(b.Order));

        Game.Get<GameManager>().Spawner = this;

        dropArea = collider2D;
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Drop.started += OnStartDrag;

        inputActions.Player.Drop.canceled += OnRelease;
    }

    private void OnDisable()
    {
        inputActions.Player.Drop.started -= OnStartDrag;

        inputActions.Player.Drop.canceled -= OnRelease;

        inputActions.Disable();
    }

    private void Start()
    {
        SelectNextFruit();
        CreatePreviewFruit();
    }

    private void Update()
    {
        if (!isDragging)
            return;

        MoveSpawner();
    }

    private void OnStartDrag(InputAction.CallbackContext ctx)
    {
        isDragging = true;
    }

    private void OnRelease(InputAction.CallbackContext ctx)
    {
        if (!isDragging)
            return;

        isDragging = false;

        DropFruit();
    }


    public void Initialize()
    {
        canDrop = true;

        currentFruitData = null;

        SelectNextFruit();
        CreatePreviewFruit();
    }


    // 스포너 이동 구현
    private void MoveSpawner()
    {
        if (Game.Get<GameManager>().CurrentState != GameState.Playing)
            return;


        Vector2 screenPosition = inputActions.Player.Move.ReadValue<Vector2>();

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        float clampedX = Mathf.Clamp(worldPosition.x, minX, maxX);

        transform.position = new Vector3(clampedX, transform.position.y, 0);

        if (currentPreviewFruit != null)
        {
            currentPreviewFruit.transform.position = transform.position;
        }
    }


    // 랜덤 과일 선택 구현
    private FruitData GetRandomFruit()
    {
        int randomIndex = Random.Range(0, 5);

        return fruits[randomIndex];
    }


    //다음 과일 선택 구현
    private void SelectNextFruit()
    {
        currentFruitData = nextFruitData;

        nextFruitData = GetRandomFruit();

        GameEventBus.Publish(new FruitChangedEvent(nextFruitData));

        if (currentFruitData == null)
        {
            currentFruitData = GetRandomFruit();
        }
    }


    //프리뷰 과일 생성 구현
    private void CreatePreviewFruit()
    {
        currentPreviewFruit.sprite = currentFruitData.sprite;

        currentPreviewFruit.transform.localScale = Vector3.one * currentFruitData.Radius * 2f;

        currentPreviewFruit.transform.position = transform.position;
    }


    //드롭 구현
    private async void DropFruit()
    {
        if (!canDrop)
            return;

        if (Game.Get<UIManager>().IsInputBlocked)
            return;

        if (!IsInsideDropArea())
            return;

        canDrop = false;

        FruitFactory.CreateFruit(currentFruitData, transform.position);

        Game.Get<SoundManager>().PlaySFX(SoundType.DropSFX);

        currentPreviewFruit.sprite = null;

        SelectNextFruit();

        CreatePreviewFruit();

        await WaitDropCooldown();
    }

    private bool IsInsideDropArea()
    {
        Vector2 screenPos = inputActions.Player.Move.ReadValue<Vector2>();

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        return dropArea.OverlapPoint(worldPos);
    }

    private async Task WaitDropCooldown()
    {
        int milliseconds = Mathf.RoundToInt( dropCooldown * 1000f);

        await Task.Delay(milliseconds);

        canDrop = true;
    }
}