using VContainer;
using System.Collections;
using UnityEngine;

public class SlowTimeManager : MonoBehaviour
{
    public static SlowTimeManager SingleTon;

    [SerializeField] private float _parryStopTimeDuration;

    [Inject] private IPlayerCombatEvents _playerCombatEvents;

    private void Awake()
    {
        if (SingleTon == null)
            SingleTon = this;
        else Destroy(this);

        Initialize();
    }

    public void Initialize()
    {
        _playerCombatEvents.OnParry += () => SlowTime(0, _parryStopTimeDuration);
    }
    private IEnumerator SlowTimeCorutine(float timeScale, float duration)
    {
        Time.timeScale = timeScale;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1;
    }
    public void SlowTime(float timeScale, float duration)
    {
        // Debug.Log("SLOW TIME");
        StartCoroutine(SlowTimeCorutine(timeScale, duration));
    }
}
