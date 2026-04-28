using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string ItemName;
    public Sprite Icon;
    public AudioClip TakeSounde;
    public float SoundVolume = 1f; // убрать потом
    public Weapon Weapon;
}
