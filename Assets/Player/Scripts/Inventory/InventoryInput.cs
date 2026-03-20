using UnityEngine;

public class InventoryInput : MonoBehaviour
{
    [SerializeField] private WeaponHandler _weaponHandler;

    [Header("Controls")]
    [SerializeField] private KeyCode
    _1key = KeyCode.Alpha1, _2key = KeyCode.Alpha2, _3key = KeyCode.Alpha3, _4key = KeyCode.Alpha4;

    private void Update()
    {
        HandleInput();
    }
    private void HandleInput()
    {
        if (_weaponHandler == null)
        {
            Debug.LogError("Нету блять ссылки на weapon handler");
            return;
        }

        if (Input.GetKeyDown(_1key))
            _weaponHandler.SetActiveWeaponSlot(1);
        else if (Input.GetKeyDown(_2key))
            _weaponHandler.SetActiveWeaponSlot(2);
        else if (Input.GetKeyDown(_3key))
            _weaponHandler.SetActiveWeaponSlot(3);
        else if (Input.GetKeyDown(_4key))
            _weaponHandler.SetActiveWeaponSlot(4);
    }
}
