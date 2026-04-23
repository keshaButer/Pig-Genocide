using VContainer;
using System.Collections;
using UnityEngine;

public class SlowTimeManager : MonoBehaviour
{
    public static SlowTimeManager SingleTon;

    [Inject] private IPlayerEvents _playerEvents;

    private void Awake()
    {
        if (SingleTon == null)
            SingleTon = this;
        else Destroy(this);

        Initialize();
    }

    public void Initialize()
    {
        _playerEvents.OnParry += () => SlowTime(0, 0.2f);
    }
    private IEnumerator SlowTimeCorutine(float timeScale, float duration)
    {
        Time.timeScale = timeScale;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1;
    }
    public void SlowTime(float timeScale, float duration)
    {
        Debug.Log("SLOW TIME");
        StartCoroutine(SlowTimeCorutine(timeScale, duration));
    }
}
