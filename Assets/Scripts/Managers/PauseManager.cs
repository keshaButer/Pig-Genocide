using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static void PauseGame(bool turn)
    {
        Time.timeScale = turn ? 0 : 1;
    }
}
