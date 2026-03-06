using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [field: SerializeField] public Item item { get; protected set; }
    public float fireInterval;
    public abstract void WeaponAttack();
    public abstract void Initialize();
}