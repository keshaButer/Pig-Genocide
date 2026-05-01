using System;
using UnityEngine;

public class InventoryInput : MonoBehaviour
{
    [Header("Controls")]
    [SerializeField] private KeyCode _slot1 = KeyCode.Alpha1;
    [SerializeField] private KeyCode _slot2 = KeyCode.Alpha2;
    [SerializeField] private KeyCode _slot3 = KeyCode.Alpha3;
    [SerializeField] private KeyCode _slot4 = KeyCode.Alpha4;

    public event Action<int> OnSelectSlot;

    private void Update()
    {
        HandleInput();
    }
    private void HandleInput()
    {
        if (Input.GetKeyDown(_slot1))
            OnSelectSlot?.Invoke(1);
        else if (Input.GetKeyDown(_slot2))
            OnSelectSlot?.Invoke(2);
        else if (Input.GetKeyDown(_slot3))
            OnSelectSlot?.Invoke(3);
        else if (Input.GetKeyDown(_slot4))
            OnSelectSlot?.Invoke(4);
    }
}
