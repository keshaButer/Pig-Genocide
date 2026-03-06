using UnityEngine;

[CreateAssetMenu(fileName = "Rifle", menuName = "Item/Rifle")]
public class Rifle : Item
{
    public int bulletCount;
    public float radiusSoundShot;
    public AudioClip audioClipShot;
    public bool canSpam;
}
