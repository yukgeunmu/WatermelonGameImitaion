using UnityEngine;

[CreateAssetMenu(menuName = "Game/Fruit Data")]
public class FruitData : ScriptableObject, IBaseResource<FruitType>
{
    //현재 과일 종류
    public FruitType Type;

    public FruitType Key => Type;

    public Sprite sprite;

    //순서
    public int Order;

    // 머지 시 획득 점수
    public int Score;

    //과일 크기
    public float Radius;

    //다음 진화 과일
    public FruitType NextFruit;

    public Fruit Prefab;
}