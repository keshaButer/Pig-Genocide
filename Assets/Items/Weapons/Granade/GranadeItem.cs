using UnityEngine;

[CreateAssetMenu(fileName = "Granade", menuName = "Item/Granade")]
public class GranadeItem : Item
{
    [SerializeField] int damage;
    [SerializeField] float radius;
}
