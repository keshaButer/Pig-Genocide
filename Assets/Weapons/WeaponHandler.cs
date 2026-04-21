using VContainer;
using VContainer.Unity;
using UnityEngine;
using System.Collections.Generic;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private float _rotateAccelaration;
    [SerializeField] private KeyCode _useWeaponKey = KeyCode.Mouse0;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private InventoryInput _inventoryInput;
    
    [Inject] private IObjectResolver _objectResolver;

    public event System.Action<bool> OnExpand;

    private bool _wasLeft;
    private float _angle;
    private float _nextTimeFire;
    private bool _needUpdateNextTimeFire = false;
    private Weapon _activeWeapon;
    private Dictionary<string, Weapon> _mountedWeapons = new Dictionary<string, Weapon>();

    private void OnEnable()
    {
        _inventory.OnAddItem += SetInventoryWeapons;
        _inventoryInput.OnSelectSlot += SetActiveWeaponSlot;
    }
    private void OnDisable()
    {
        _inventoryInput.OnSelectSlot -= SetActiveWeaponSlot;
    }

    public void SetActiveWeaponSlot(int index)
    {
        if (transform.childCount < index)
        {
            Debug.Log("Нет предмета на этой ячейке");
            return;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        Weapon weapon = transform.GetChild(index - 1).GetComponent<Weapon>();
        weapon.gameObject.SetActive(true);

        _activeWeapon = weapon;
    }
    private void SetInventoryWeapons()
    {
        foreach (Item item in _inventory.CurrentItems)
        {
            if (item.Weapon != null)
            {
                SetWeapon(item.Weapon);
            }
        }
    }
    private void SetWeapon(Weapon weaponPrefab)
    {
        if (weaponPrefab == null)
            return;

        System.Type type = weaponPrefab.GetType();
        string key = type.Name;

        if (!_mountedWeapons.ContainsKey(key))
        {
            Weapon spawnedWeapon = _objectResolver.Instantiate(weaponPrefab, transform);
            spawnedWeapon.transform.localPosition = Vector3.zero;
            spawnedWeapon.transform.localRotation = Quaternion.Euler(0, 0, 0);

            spawnedWeapon.gameObject.SetActive(false);

            _mountedWeapons.Add(key, spawnedWeapon);
            _activeWeapon = spawnedWeapon;

            print($"Weapon: {key} was set.");
        }
        else 
        {
            Debug.Log("ТАКОЙ ТИП ОРУЖИЯ УЖЕ ЕСТЬ");
        }

        // FlipSprite();
    }
    private void FlipSprite()
    {
        if (_activeWeapon != null)
        {
            if ((_angle > -180 && _angle < -90) || (_angle < 180 && _angle > 90))
            {
                SpriteRenderer sprite;
                if (_activeWeapon.TryGetComponent<SpriteRenderer>(out sprite))
                {
                    sprite.flipY = true;
                }
            }
        }
    }
    private void SetDirectionByMouse()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        _angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion rot = Quaternion.Euler(0, 0, _angle);
        transform.localRotation = Quaternion.Lerp(transform.rotation, rot, _rotateAccelaration);

        if (((_angle > -180 && _angle < -90) || (_angle < 180 && _angle > 90)) && !_wasLeft)
        {
            _wasLeft = true;
            OnExpand?.Invoke(true);

            if (_activeWeapon != null && _activeWeapon.gameObject.activeSelf)
                _activeWeapon.GetComponent<SpriteRenderer>().flipY = true;
        }
        else if (_angle < 90 && _angle > -90 && _wasLeft)
        {
            _wasLeft = false;
            OnExpand?.Invoke(false);

            if (_activeWeapon != null && _activeWeapon.gameObject.activeSelf)
                _activeWeapon.GetComponent<SpriteRenderer>().flipY = false;
        }
    }
    private void Update()
    {
        SetDirectionByMouse();
        HandleInput();
    }
    private void HandleInput()
    {
        if (Input.GetKey(_useWeaponKey))
        {
            if (_needUpdateNextTimeFire)
            {
                _nextTimeFire = Time.time + _activeWeapon.FireInterval;
                _needUpdateNextTimeFire = false;
            }

            if (_activeWeapon != null && Time.time >= _nextTimeFire)
            {
                _activeWeapon.UseAttack();

                _needUpdateNextTimeFire = true;
            }
        }
    }
}
