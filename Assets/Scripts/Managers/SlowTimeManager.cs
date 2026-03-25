using System.Collections;
using UnityEngine;

public class SlowTimeManager : MonoBehaviour
{
    public static SlowTimeManager SingleTon;

    private void Awake()
    {
        if (SingleTon == null)
            SingleTon = this;
        else Destroy(this);
    }

    public void Initialize()
    {
        EventManager.Parry += () => SlowTime(0, 0.2f);
    }
    private IEnumerator SlowTimeCorutine(float timeScale, float duration)
    {
        Time.timeScale = timeScale;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1;
    }
    public void SlowTime(float timeScale, float duration)
    {
        StartCoroutine(SlowTimeCorutine(timeScale, duration));
    }
}
