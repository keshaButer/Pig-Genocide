using System;
using UnityEngine;

public class InventoryInput : MonoBehaviour
{
    [Header("Controls")]
    [SerializeField] private KeyCode _1key = KeyCode.Alpha1;
    [SerializeField] private KeyCode _2key = KeyCode.Alpha2;
    [SerializeField] private KeyCode _3key = KeyCode.Alpha3;
    [SerializeField] private KeyCode _4key = KeyCode.Alpha4;

    public event Action<int> OnSelectSlot;

    private void Update()
    {
        HandleInput();
    }
    private void HandleInput()
    {
        if (Input.GetKeyDown(_1key))
            OnSelectSlot?.Invoke(1);
        else if (Input.GetKeyDown(_2key))
            OnSelectSlot?.Invoke(2);
        else if (Input.GetKeyDown(_3key))
            OnSelectSlot?.Invoke(3);
        else if (Input.GetKeyDown(_4key))
            OnSelectSlot?.Invoke(4);
    }
}
