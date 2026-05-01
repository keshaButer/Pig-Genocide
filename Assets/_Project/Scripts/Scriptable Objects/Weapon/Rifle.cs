using UnityEngine;

[CreateAssetMenu(fileName = "Rifle", menuName = "Item/Rifle")]
public class Rifle : Item
{
    public AudioClip ShotSound;
    public bool CanSpam;
}
