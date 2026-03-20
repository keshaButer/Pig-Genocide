using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public string Name = "Unnamed";
    public float FireInterval;
    public abstract void UseAttack();
}
