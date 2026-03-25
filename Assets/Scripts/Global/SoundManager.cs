using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager SingleTone;

    private AudioSource audioSource;
    
    private void Awake()
    {
        if (SingleTone == null)
            SingleTone = this;
        else if (SingleTone != null)
            Destroy(this);
    }

    private void OnDisable() => PlayerSpawner.OnPlayerSpawned -= Initialize;
    public void Subscribe() => PlayerSpawner.OnPlayerSpawned += Initialize;

    private void Initialize(GameObject player)
    {
        if (player == null)
        {
            Debug.LogError("PLAYER is NULL");
            return;
        }
        Debug.LogError("PLAYER is not NULL");
        audioSource = player.GetComponent<AudioSource>();
    }
    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (audioSource == null)
        {
            Debug.LogError("audioSource is NULL");
            return;
        }

        audioSource.volume = volume;
        audioSource.PlayOneShot(clip);
    }
}
