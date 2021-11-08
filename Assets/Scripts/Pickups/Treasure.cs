using UnityEngine;

[CreateAssetMenu(fileName = "Treasure", menuName = "Treasures/New treasure")]
public class Treasure : ScriptableObject
{
    public GameObject prefab;
    public Sprite iconUI;
    public new string name;
    public int currentAmount;
    public int maxAmount;
}