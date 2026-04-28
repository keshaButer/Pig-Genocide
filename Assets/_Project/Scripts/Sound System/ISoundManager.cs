using UnityEngine;

public interface ISoundManager
{
    public void PlaySound(AudioClip clip, float volume = 1f);
}