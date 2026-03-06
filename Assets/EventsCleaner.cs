using UnityEngine;

public class EventsCleaner : MonoBehaviour
{
    private void OnDestroy()
    {
        EventManager.CleanEvents();
    }
}
