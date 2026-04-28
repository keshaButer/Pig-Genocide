using UnityEngine;
using System.Collections;

public class SwitchInventory : MonoBehaviour
{
    [SerializeField] KeyCode inventoryKey;
    [SerializeField] float posDisable;
    [SerializeField] float time;
    [SerializeField] private RectTransform rect1;
    [SerializeField] private RectTransform rect2;
    private Coroutine coroutine;
    private float timer;
    private bool isActive = true;
    private void Turn(bool _turn)
    {
        if (_turn)
        {
            timer = 0;
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(SwitchIt(false));

            isActive = true;
        }
        else
        {
            timer = 0;
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(SwitchIt(true));

            isActive = false;
        }
    }

    private IEnumerator SwitchIt(bool sm)
    {
        while (true)
        {
            timer += Time.deltaTime;
            if (!sm)
            {
                rect1.anchoredPosition = Vector3.Lerp(rect1.anchoredPosition, new Vector3(posDisable, 0, 0), time * 100 * Time.deltaTime);
                rect2.anchoredPosition = Vector3.Lerp(rect2.anchoredPosition, new Vector3(posDisable, 0, 0), time * 100 * Time.deltaTime);
            }
            else
            {
                rect1.anchoredPosition = Vector3.Lerp(rect1.anchoredPosition, new Vector3(0, 0, 0), time * 100 * Time.deltaTime);
                rect2.anchoredPosition = Vector3.Lerp(rect2.anchoredPosition, new Vector3(0, 0, 0), time * 100 * Time.deltaTime);
            }
            if (timer > 1)
            {
                timer = 0;
                StopCoroutine(coroutine);
            }
            yield return null;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(inventoryKey))
        {
            if (!isActive)
            {
                CursorManager.Disable();
                Turn(true);
            }
            else
            {
                CursorManager.Enable();
                Turn(false); 
            }
        }
    }
}
