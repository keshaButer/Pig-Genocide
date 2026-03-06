using UnityEngine;

public class ParryInput : MonoBehaviour
{
    [SerializeField] private InputPlayerMovementConfig inputConfig; 

    private void Update() => GetInput(); 

    private void GetInput()
    {
        if (Input.GetKeyDown(inputConfig.parryKey))
        {
            print("Handle parry");
        }
    }
}
