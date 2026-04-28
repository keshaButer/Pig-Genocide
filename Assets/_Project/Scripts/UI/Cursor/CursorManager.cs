using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Sprite sprite;

    public static bool isActive;

    private void Awake()
    {
        Enable();
        GetComponent<Image>().sprite = sprite;
    }
    public static void Enable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        isActive = true;
    }
    public static void Disable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        isActive = false;
    }
    private void Update() => CursorFollowing();
    private void CursorFollowing() => transform.position = Input.mousePosition;
}
