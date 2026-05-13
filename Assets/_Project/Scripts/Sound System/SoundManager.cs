using VContainer;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour, ISoundManager
{
    private AudioSource _audioSource;

    private IEnemyEvents _enemyEvents;
    private IPlayerMovementEvents _playerMovementEvents;
    private IPlayerCombatEvents _playerCombatEvents;
    private IPlayerStateEvents _playerStateEvents;

    private SoundManagerConfig _config;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    [Inject]
    public void Construct(
        IEnemyEvents enemyEvents,
        IPlayerMovementEvents playerMovementEvents,
        IPlayerCombatEvents playerCombatEvents,
        IPlayerStateEvents playerStateEvents,
        SoundManagerConfig config
    )
    {
        _enemyEvents = enemyEvents;
        _enemyEvents.OnEnemyDied += PlayOnEnemyDeath;

        _playerMovementEvents = playerMovementEvents;
        _playerMovementEvents.OnDash += PlayOnDash;

        _playerCombatEvents = playerCombatEvents;
        _playerCombatEvents.OnParry += PlayOnParry;

        _playerStateEvents = playerStateEvents;
        _playerStateEvents.OnDied += PlayOnPlayerDeath;

        _config = config;
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (_audioSource == null)
        {
            Debug.LogError("audioSource is NULL");
            return;
        }

        _audioSource.PlayOneShot(clip, volume);
    }

    private void PlayOnEnemyDeath() => PlaySound(_config.EnemyDeath);
    private void PlayOnDash() => PlaySound(_config.Dash);
    private void PlayOnParry() => PlaySound(_config.Parry);
    private void PlayOnPlayerDeath() => PlaySound(_config.PlayerDeath);

    private void OnDisable()
    {
        if (_enemyEvents != null)
            _enemyEvents.OnEnemyDied -= PlayOnEnemyDeath;

        if (_playerMovementEvents != null)
            _playerMovementEvents.OnDash -= PlayOnDash;

        if (_playerCombatEvents != null)
            _playerCombatEvents.OnParry -= PlayOnParry;

        if (_playerStateEvents != null)
            _playerStateEvents.OnDied -= PlayOnPlayerDeath;
    }
}
