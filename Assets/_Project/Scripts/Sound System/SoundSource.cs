using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSource : MonoBehaviour
{
    [field:SerializeField] public float SoundRadius { get;  private set; }
    private AudioSource _audioSource;
    private void Awake() => _audioSource = GetComponent<AudioSource>();
    public void PlaySound(AudioClip audioClip, float soundRadius = 1)
    {
        // _audioSource.clip = audioClip;
        // _audioSource.Play();
        
        if (soundRadius == 1) soundRadius = SoundRadius;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, soundRadius);

        foreach (Collider2D collider in colliders)
        {
            collider.gameObject.GetComponent<ISoundListener>()?.HandleSound(transform);
        }
        // print("звук!");
    }
}