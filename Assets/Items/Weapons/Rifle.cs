using UnityEngine;

[CreateAssetMenu(fileName = "Rifle", menuName = "Item/Rifle")]
public class Rifle : Item
{
    public int BulletCount;
    public float RadiusShotSound;
    public AudioClip ShotSound;
    public bool CanSpam;
}
