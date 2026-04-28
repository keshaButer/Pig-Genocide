using VContainer;
using UnityEngine;

public class SoundManager : MonoBehaviour, ISoundManager
{
    private AudioSource audioSource;
    
    [Inject]
    public void Construct(IPlayerProvider playerProvider)
    {
        playerProvider.OnPlayerSpawned += OnPlayerSpawned;
        
        if (playerProvider.Player != null)
            OnPlayerSpawned(playerProvider.Player);
    }

    private void OnPlayerSpawned(GameObject player)
    {
        if (player == null)
        {
            Debug.LogError("PLAYER is NULL");
            return;
        }
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
