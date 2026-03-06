using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public AudioClip takeSound;
    public float soundVolume = 1f;
}
