using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
    }
    public static void PauseGame(bool turn)
    {
        Time.timeScale = turn ? 0 : 1;
    }
}
