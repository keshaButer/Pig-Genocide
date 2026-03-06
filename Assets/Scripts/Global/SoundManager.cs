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

    private void Start() => Initialize();

    private void Initialize()
    {
        audioSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(clip);
    }
}
