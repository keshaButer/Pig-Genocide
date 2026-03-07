using UnityEngine;

public class ParryInput : MonoBehaviour
{
    [SerializeField] private InputPlayerMovementConfig inputConfig; 
    public bool CanParry { get; private set; }
    [SerializeField] private float intervalPress = 1f;
    private float timer = 0;

    private void Update()
    {
        if (timer < intervalPress)
            timer += Time.deltaTime;
        else
            CanParry = false;

        if (Input.GetKeyDown(inputConfig.parryKey))
        {
            CanParry = true;
            timer = 0;
        }
    }
}
