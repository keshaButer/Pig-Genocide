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
    public Vector2 MouseDirection = Vector2.left;

    private bool _wasLeft;
    private float _angle;
    private float _nextTimeFire;
    private bool _needUpdateNextTimeFire = false;
    private Weapon _activeWeapon;
    private Camera _mainCamera;
    private Dictionary<System.Type, Weapon> _mountedWeapons = new Dictionary<System.Type, Weapon>();
    private Weapon[] _weaponSlots = new Weapon[5];
    private int _weaponsCount = 0;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _inventory.OnAddItem += SetInventoryWeapons;
        _inventoryInput.OnSelectSlot += SetActiveWeaponSlot;
    }
    private void OnDisable()
    {
        _inventory.OnAddItem -= SetInventoryWeapons;
        _inventoryInput.OnSelectSlot -= SetActiveWeaponSlot;
    }

    public void SetActiveWeaponSlot(int index)
    {
        if (_weaponsCount < index)
        {
            Debug.Log("Нет предмета на этой ячейке");
            return;
        }

        for (int i = 0; i < _weaponsCount; i++)
        {
            _weaponSlots[i].gameObject.SetActive(false);
        }

        Weapon weapon = _weaponSlots[index - 1];
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

        if (!_mountedWeapons.ContainsKey(type))
        {
            if (_weaponsCount >= _weaponSlots.Length)
            {
                Debug.Log("В инвентаре нет места для оружия.");
                return;
            }

            Weapon spawnedWeapon = _objectResolver.Instantiate(weaponPrefab, transform);
            spawnedWeapon.transform.localPosition = Vector3.zero;
            spawnedWeapon.transform.localRotation = Quaternion.Euler(0, 0, 0);

            spawnedWeapon.gameObject.SetActive(false);

            _mountedWeapons.Add(type, spawnedWeapon);

            _weaponSlots[_weaponsCount] = spawnedWeapon;
            _weaponsCount++;

            _activeWeapon = spawnedWeapon;

            Debug.Log($"Weapon of type: {type}, was set in slot number: {_weaponsCount}.");
        }
        else 
        {
            Debug.Log($"Weapon of type: {type} already in inventory.");
        }

        FlipSprite();
    }
    private void FlipSprite()
    {
        if (_activeWeapon != null)
        {
            if ((_angle > -180 && _angle < -90) || (_angle < 180 && _angle > 90))
            {
                SpriteRenderer sprite;
                if (_activeWeapon.TryGetComponent(out sprite))
                {
                    sprite.flipY = true;
                }
            }
        }
    }
    private void SetDirectionByMouse()
    {
        MouseDirection = _mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        _angle = Mathf.Atan2(MouseDirection.y, MouseDirection.x) * Mathf.Rad2Deg;

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
